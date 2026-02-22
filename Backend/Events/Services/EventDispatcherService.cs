using Backend.Events.Data.Digimon;
using Backend.Events.Data.Party;
using Backend.Events.Data.Player;
using Backend.Events.Data.System;
using Backend.Events.Hubs;
using Backend.Models;
using Backend.Models.Digimons;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace Backend.Events.Services;

public class EventDispatcherService : Interfaces.IEventDispatcherService
{
    private Player? _previousState;
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

    public void ProcessGameState(Player newState)
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
        if (newState.Bits != _previousState.Bits)
        {
            var ev = new PlayerBitsChangedEvent(newState.Bits);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // 2. Compare Party
        bool partyChanged = false;
        if (newState.Party.Digimons.Count != _previousState.Party.Digimons.Count)
        {
            partyChanged = true;
        }
        else
        {
            for (int i = 0; i < newState.Party.Digimons.Count; i++)
            {
                if (newState.Party.Digimons[i].BasicInfo.Name != _previousState.Party.Digimons[i].BasicInfo.Name)
                {
                    partyChanged = true;
                    break;
                }
            }
        }

        if (partyChanged)
        {
            var ev = new PartySlotsChangedEvent(newState.Party.Digimons);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }
        else
        {
            // Compare individualized digimon metrics if party structure hasn't fundamentally changed
            for (int i = 0; i < newState.Party.Digimons.Count; i++)
            {
                CompareDigimon(i, _previousState.Party.Digimons[i], newState.Party.Digimons[i]);
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

        // Compare XP & Level
        if (oldDigi.BasicInfo.Level != newDigi.BasicInfo.Level ||
            oldDigi.BasicInfo.Experience != newDigi.BasicInfo.Experience)
        {
            if (newDigi.BasicInfo.Level > oldDigi.BasicInfo.Level)
            {
                var levelUpEv = new DigimonLevelUpEvent(index, oldDigi.BasicInfo.Level, newDigi.BasicInfo.Level);
                _ = _hubContext.Clients.All.SendAsync(levelUpEv.Type.ToString(), levelUpEv);
            }

            var ev = new DigimonXpGainedEvent(index, newDigi.BasicInfo.Level, newDigi.BasicInfo.Experience);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
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
    }

    // A simple Deep clone since records aren't being used
    private Player CloneState(Player p)
    {
        return new Player
        {
            Name = p.Name,
            Bits = p.Bits,
            Party = new Party
            {
                Digimons = p.Party.Digimons.Select(d => new Digimon
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
                    }
                }).ToList()
            }
        };
    }
}
