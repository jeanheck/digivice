using Backend.Events.Data.Digimon;
using Backend.Events.Data.Party;
using Backend.Events.Data.Player;
using Backend.Events.Data.System;
using Backend.Events.Hubs;
using Backend.Models;
using Backend.Models.Quests;
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

        // Combine Location and Player comparisons
        if (newState.Player != null)
        {
            if (_previousState.Player != null)
            {
                // 0. Compare Location
                if (newState.Player.MapId != _previousState.Player.MapId)
                {
                    var ev = new LocationChangedEvent(newState.Player.MapId);
                    _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
                }

                // 1. Compare Player
                if (!newState.Player.Equals(_previousState.Player))
                {
                    var ev = new PlayerBitsChangedEvent(newState.Player.Bits);
                    _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
                }
            }
            else
            {
                // Player just loaded in
                var ev = new PlayerBitsChangedEvent(newState.Player.Bits);
                _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
            }
        }

        // 2. Compare Party
        if (newState.Party != null && _previousState.Party != null)
        {
            if (!newState.Party.Equals(_previousState.Party))
            {
                // We could dispatch granular events here if we wanted, but for now, if ANY
                // Digimon sub-property changes anywhere in the party roster, we can trigger
                // a PartySlotsChangedEvent to safely sync the frontend, or trigger the granular
                // individual events by walking the slots again.

                // To maintain granular event dispatching:
                var newSlots = newState.Party.Slots;
                var oldSlots = _previousState.Party.Slots;

                bool partyRosterChanged = false;
                if (newSlots.Count != oldSlots.Count)
                {
                    partyRosterChanged = true;
                }
                else
                {
                    for (int i = 0; i < newSlots.Count; i++)
                    {
                        var newDigi = newSlots[i];
                        var oldDigi = oldSlots[i];

                        if ((newDigi == null && oldDigi != null) || (newDigi != null && oldDigi == null) ||
                            (newDigi != null && oldDigi != null && newDigi.BasicInfo.Name != oldDigi.BasicInfo.Name))
                        {
                            partyRosterChanged = true;
                            break;
                        }
                    }
                }

                if (partyRosterChanged)
                {
                    var activeDigimons = newSlots.Where(d => d != null).Select(d => d!).ToList();
                    var ev = new PartySlotsChangedEvent(activeDigimons);
                    _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
                }
                else
                {
                    for (int i = 0; i < newSlots.Count; i++)
                    {
                        if (newSlots[i] != null && oldSlots[i] != null)
                        {
                            if (!newSlots[i]!.Equals(oldSlots[i]))
                            {
                                // The Digimon itself is not equal, so compare down to fire specific events
                                CompareDigimon(i, oldSlots[i]!, newSlots[i]!);
                            }
                        }
                    }
                }
            }
        }
        else if (newState.Party != null && _previousState.Party == null)
        {
            var activeDigimons = newState.Party.Slots.Where(d => d != null).Select(d => d!).ToList();
            var ev = new PartySlotsChangedEvent(activeDigimons);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // 3. Compare Important Items
        bool itemsChanged = false;
        var newItems = newState.ImportantItems;
        var oldItems = _previousState.ImportantItems;

        if (newItems != null && oldItems == null || newItems == null && oldItems != null)
        {
            itemsChanged = true;
        }
        else if (newItems != null && oldItems != null)
        {
            if (!newItems.Equals(oldItems))
            {
                itemsChanged = true;
            }
        }

        if (itemsChanged)
        {
            var ev = new ImportantItemsChangedEvent(newItems);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // 4. Compare Journal
        bool journalChanged = false;
        var newJournal = newState.Journal;
        var oldJournal = _previousState.Journal;

        if (newJournal != null && oldJournal == null || newJournal == null && oldJournal != null)
        {
            journalChanged = true;
        }
        else if (newJournal != null && oldJournal != null)
        {
            if (!newJournal.Equals(oldJournal))
            {
                journalChanged = true;
            }
        }

        if (journalChanged)
        {
            var ev = new JournalChangedEvent(newState.Journal);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
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
        if (!oldDigi.Attributes.Equals(newDigi.Attributes))
        {
            var ev = new DigimonAttributesChangedEvent(index, newDigi.Attributes.Strength, newDigi.Attributes.Defense, newDigi.Attributes.Spirit, newDigi.Attributes.Wisdom, newDigi.Attributes.Speed, newDigi.Attributes.Charisma);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare Resistances
        if (!oldDigi.Resistances.Equals(newDigi.Resistances))
        {
            var ev = new DigimonResistancesChangedEvent(index, newDigi.Resistances.Fire, newDigi.Resistances.Water, newDigi.Resistances.Ice, newDigi.Resistances.Wind, newDigi.Resistances.Thunder, newDigi.Resistances.Machine, newDigi.Resistances.Dark);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare Equipments
        if (!oldDigi.Equipments.Equals(newDigi.Equipments))
        {
            var ev = new DigimonEquipmentsChangedEvent(index, newDigi.Equipments);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare Equipped Digievolutions
        if (!Enumerable.SequenceEqual(oldDigi.EquippedDigievolutions, newDigi.EquippedDigievolutions))
        {
            // Specifically dispatch level up events if needed by scanning them again briefly
            for (int i = 0; i < 3; i++)
            {
                var oldEvo = oldDigi.EquippedDigievolutions[i];
                var newEvo = newDigi.EquippedDigievolutions[i];

                if (oldEvo != null && newEvo != null && oldEvo.Id == newEvo.Id && newEvo.Level > oldEvo.Level)
                {
                    var levelUpEv = new DigimonDigievolutionLevelUpEvent(index, newEvo.Id, oldEvo.Level, newEvo.Level);
                    _ = _hubContext.Clients.All.SendAsync(levelUpEv.Type.ToString(), levelUpEv);
                }
            }

            var ev = new DigimonDigievolutionsChangedEvent(index, newDigi.EquippedDigievolutions);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }
    }

    // A simple Deep clone since records aren't being used
    private State CloneState(State s)
    {
        return new State
        {
            Player = s.Player == null ? null : new Player { Name = s.Player.Name, Bits = s.Player.Bits, MapId = s.Player.MapId },
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
                        Strength = d.Attributes.Strength,
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
                        Machine = d.Resistances.Machine,
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
                    },
                    EquippedDigievolutions = new Digievolution?[3]
                    {
                        d.EquippedDigievolutions[0] != null ? new Digievolution { Id = d.EquippedDigievolutions[0]!.Id, Level = d.EquippedDigievolutions[0]!.Level } : null,
                        d.EquippedDigievolutions[1] != null ? new Digievolution { Id = d.EquippedDigievolutions[1]!.Id, Level = d.EquippedDigievolutions[1]!.Level } : null,
                        d.EquippedDigievolutions[2] != null ? new Digievolution { Id = d.EquippedDigievolutions[2]!.Id, Level = d.EquippedDigievolutions[2]!.Level } : null
                    }
                }).ToList()
            },
            ImportantItems = s.ImportantItems != null ? new ImportantItems
            {
                FolderBag = s.ImportantItems.FolderBag != null ? new ImportantItem { Id = s.ImportantItems.FolderBag.Id, Name = s.ImportantItems.FolderBag.Name, Has = s.ImportantItems.FolderBag.Has } : null,
                TreeBoots = s.ImportantItems.TreeBoots != null ? new ImportantItem { Id = s.ImportantItems.TreeBoots.Id, Name = s.ImportantItems.TreeBoots.Name, Has = s.ImportantItems.TreeBoots.Has } : null,
                FishingPole = s.ImportantItems.FishingPole != null ? new ImportantItem { Id = s.ImportantItems.FishingPole.Id, Name = s.ImportantItems.FishingPole.Name, Has = s.ImportantItems.FishingPole.Has } : null,
                RedSnapper = s.ImportantItems.RedSnapper != null ? new ImportantItem { Id = s.ImportantItems.RedSnapper.Id, Name = s.ImportantItems.RedSnapper.Name, Has = s.ImportantItems.RedSnapper.Has } : null
            } : null,
            ConsumableItems = s.ConsumableItems != null ? new ConsumableItems
            {
                PowerCharge = s.ConsumableItems.PowerCharge != null ? new ConsumableItem { Id = s.ConsumableItems.PowerCharge.Id, Name = s.ConsumableItems.PowerCharge.Name, Quantity = s.ConsumableItems.PowerCharge.Quantity } : null,
                SpiderWeb = s.ConsumableItems.SpiderWeb != null ? new ConsumableItem { Id = s.ConsumableItems.SpiderWeb.Id, Name = s.ConsumableItems.SpiderWeb.Name, Quantity = s.ConsumableItems.SpiderWeb.Quantity } : null,
                BambooSpear = s.ConsumableItems.BambooSpear != null ? new ConsumableItem { Id = s.ConsumableItems.BambooSpear.Id, Name = s.ConsumableItems.BambooSpear.Name, Quantity = s.ConsumableItems.BambooSpear.Quantity } : null
            } : null,
            Journal = s.Journal != null ? new Journal
            {
                MainQuest = new MainQuest
                {
                    Id = s.Journal.MainQuest.Id,
                    Title = s.Journal.MainQuest.Title,
                    Description = s.Journal.MainQuest.Description,
                    Prerequisites = s.Journal.MainQuest.Prerequisites.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone }).ToList(),
                    Steps = s.Journal.MainQuest.Steps.Select(step => new QuestStep
                    {
                        Number = step.Number,
                        Description = step.Description,
                        IsCompleted = step.IsCompleted,
                        Prerequisites = step.Prerequisites?.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone }).ToList()
                    }).ToList()
                },
                SideQuests = s.Journal.SideQuests.Select(q => new SideQuest
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    Prerequisites = q.Prerequisites.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone }).ToList(),
                    Steps = q.Steps.Select(step => new QuestStep
                    {
                        Number = step.Number,
                        Description = step.Description,
                        IsCompleted = step.IsCompleted,
                        Prerequisites = step.Prerequisites?.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone }).ToList()
                    }).ToList()
                }).ToList()
            } : null
        };
    }
}
