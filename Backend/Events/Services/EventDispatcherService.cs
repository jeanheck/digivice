using Backend.Events.Data.Digimon;
using Backend.Events.Data.Party;
using Backend.Events.Data.Player;
using Backend.Events.Data.System;
using Backend.Events.Hubs;
using Backend.Models;
using Backend.Models.Digimons;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Services;

public class EventDispatcherService : Interfaces.IEventDispatcherService
{
    private State? _previousState;
    private bool? _previousConnectionStatus;
    private readonly IHubContext<GameHub> _hubContext;

    public EventDispatcherService(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void DispatchConnectionStatus(bool isConnected)
    {
        if (_previousConnectionStatus == isConnected) return;

        _previousConnectionStatus = isConnected;
        var ev = new ConnectionStatusChangedEvent(isConnected);

        // Fire and forget send
        _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);

        if (!isConnected)
        {
            // Reset state so that next connection will do a full sync
            _previousState = null;
        }
    }

    public void DispatchInitialStateToClient(string connectionId)
    {
        if (_previousState != null)
        {
            var ev = new InitialStateSyncEvent(_previousState);
            _ = _hubContext.Clients.Client(connectionId).SendAsync(ev.Type.ToString(), ev);
        }
    }

    public void ProcessGameState(State newState)
    {
        if (_previousState == null)
        {
            // Initial Sync
            var ev = new InitialStateSyncEvent(newState);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);

            _previousState = CloneState(newState);
            return;
        }

        // 1. Compare Player Bits
        if (newState.Player?.Bits != _previousState.Player?.Bits)
        {
            var ev = new PlayerBitsChangedEvent(newState.Player?.Bits ?? 0);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // 2. Compare Party
        bool partyChanged = false;
        var newSlots = newState.Party?.Slots ?? new List<Digimon?>();
        var oldSlots = _previousState.Party?.Slots ?? new List<Digimon?>();

        // Se a lista mudou de tamanho por algum motivo (embora sejam sempre 3 slots)
        if (newSlots.Count != oldSlots.Count)
        {
            partyChanged = true;
        }
        else
        {
            for (int i = 0; i < newSlots.Count; i++)
            {
                var newDigi = newSlots[i];
                var oldDigi = oldSlots[i];

                // Check for new digimon injected into a slot or removed
                if ((newDigi == null && oldDigi != null) || (newDigi != null && oldDigi == null))
                {
                    partyChanged = true;
                    break;
                }

                if (newDigi != null && oldDigi != null && newDigi.BasicInfo.Name != oldDigi.BasicInfo.Name)
                {
                    partyChanged = true;
                    break;
                }
            }
        }

        if (partyChanged)
        {
            // Filter out nulls for the event
            var activeDigimons = newSlots.Where(d => d != null).Select(d => d!).ToList();
            var ev = new PartySlotsChangedEvent(activeDigimons);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }
        else
        {
            // Compare individualized digimon metrics if party structure hasn't fundamentally changed
            for (int i = 0; i < newSlots.Count; i++)
            {
                if (newSlots[i] != null && oldSlots[i] != null)
                {
                    CompareDigimon(i, oldSlots[i]!, newSlots[i]!);
                }
            }
        }

        _previousState = CloneState(newState);
    }

    private void CompareDigimon(int index, Digimon oldDigi, Digimon newDigi)
    {
        // Compare Vitals
        if (oldDigi.BasicInfo.CurrentHP != newDigi.BasicInfo.CurrentHP ||
            oldDigi.BasicInfo.MaxHP != newDigi.BasicInfo.MaxHP ||
            oldDigi.BasicInfo.CurrentMP != newDigi.BasicInfo.CurrentMP ||
            oldDigi.BasicInfo.MaxMP != newDigi.BasicInfo.MaxMP)
        {
            var ev = new DigimonVitalsChangedEvent(index, newDigi.BasicInfo.CurrentHP, newDigi.BasicInfo.MaxHP, newDigi.BasicInfo.CurrentMP, newDigi.BasicInfo.MaxMP);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare XP
        if (oldDigi.BasicInfo.Experience != newDigi.BasicInfo.Experience)
        {
            var ev = new DigimonXpGainedEvent(index, newDigi.BasicInfo.Level, newDigi.BasicInfo.Experience);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare Level (Level Up)
        if (newDigi.BasicInfo.Level > oldDigi.BasicInfo.Level)
        {
            var levelUpEv = new DigimonLevelUpEvent(index, oldDigi.BasicInfo.Level, newDigi.BasicInfo.Level);
            _ = _hubContext.Clients.All.SendAsync(levelUpEv.Type.ToString(), levelUpEv);
        }

        // Compare Attributes
        if (oldDigi.Attributes.Attack != newDigi.Attributes.Attack ||
            oldDigi.Attributes.Defense != newDigi.Attributes.Defense ||
            oldDigi.Attributes.Spirit != newDigi.Attributes.Spirit ||
            oldDigi.Attributes.Wisdom != newDigi.Attributes.Wisdom ||
            oldDigi.Attributes.Speed != newDigi.Attributes.Speed ||
            oldDigi.Attributes.Charisma != newDigi.Attributes.Charisma)
        {
            var ev = new DigimonAttributesChangedEvent(index, newDigi.Attributes.Attack, newDigi.Attributes.Defense, newDigi.Attributes.Spirit, newDigi.Attributes.Wisdom, newDigi.Attributes.Speed, newDigi.Attributes.Charisma);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare Resistances
        if (oldDigi.Resistances.Fire != newDigi.Resistances.Fire ||
            oldDigi.Resistances.Water != newDigi.Resistances.Water ||
            oldDigi.Resistances.Ice != newDigi.Resistances.Ice ||
            oldDigi.Resistances.Wind != newDigi.Resistances.Wind ||
            oldDigi.Resistances.Thunder != newDigi.Resistances.Thunder ||
            oldDigi.Resistances.Metal != newDigi.Resistances.Metal ||
            oldDigi.Resistances.Dark != newDigi.Resistances.Dark)
        {
            var ev = new DigimonResistancesChangedEvent(index, newDigi.Resistances.Fire, newDigi.Resistances.Water, newDigi.Resistances.Ice, newDigi.Resistances.Wind, newDigi.Resistances.Thunder, newDigi.Resistances.Metal, newDigi.Resistances.Dark);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare Equipments
        if (oldDigi.Equipments.Head != newDigi.Equipments.Head ||
            oldDigi.Equipments.Body != newDigi.Equipments.Body ||
            oldDigi.Equipments.RightHand != newDigi.Equipments.RightHand ||
            oldDigi.Equipments.LeftHand != newDigi.Equipments.LeftHand ||
            oldDigi.Equipments.Accessory1 != newDigi.Equipments.Accessory1 ||
            oldDigi.Equipments.Accessory2 != newDigi.Equipments.Accessory2)
        {
            var ev = new DigimonEquipmentsChangedEvent(index, newDigi.Equipments);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }
    }

    // A simple Deep clone since records aren't being used
    private State CloneState(State s)
    {
        return new State
        {
            Player = s.Player == null ? null : new Player { Name = s.Player.Name, Bits = s.Player.Bits },
            Party = s.Party == null ? null : new Party
            {
                Slots = s.Party.Slots.Select(d => d == null ? null : new Digimon
                {
                    SlotIndex = d.SlotIndex,
                    BasicInfo = new BasicInfo
                    {
                        Name = d.BasicInfo.Name,
                        Level = d.BasicInfo.Level,
                        Experience = d.BasicInfo.Experience,
                        MaxHP = d.BasicInfo.MaxHP,
                        MaxMP = d.BasicInfo.MaxMP,
                        CurrentHP = d.BasicInfo.CurrentHP,
                        CurrentMP = d.BasicInfo.CurrentMP
                    },
                    Attributes = new Attributes
                    {
                        Attack = d.Attributes.Attack,
                        Defense = d.Attributes.Defense,
                        Spirit = d.Attributes.Spirit,
                        Wisdom = d.Attributes.Wisdom,
                        Speed = d.Attributes.Speed,
                        Charisma = d.Attributes.Charisma
                    },
                    Resistances = new Resistances
                    {
                        Fire = d.Resistances.Fire,
                        Water = d.Resistances.Water,
                        Ice = d.Resistances.Ice,
                        Wind = d.Resistances.Wind,
                        Thunder = d.Resistances.Thunder,
                        Metal = d.Resistances.Metal,
                        Dark = d.Resistances.Dark
                    },
                    Equipments = new Equipments
                    {
                        Head = d.Equipments.Head,
                        Body = d.Equipments.Body,
                        RightHand = d.Equipments.RightHand,
                        LeftHand = d.Equipments.LeftHand,
                        Accessory1 = d.Equipments.Accessory1,
                        Accessory2 = d.Equipments.Accessory2
                    }
                }).ToList(),
                ActiveSlotIndex = s.Party.ActiveSlotIndex
            }
        };
    }
}
