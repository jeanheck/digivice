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

        // 0. Compare Location
        if (newState.CurrentLocation != _previousState.CurrentLocation)
        {
            var ev = new LocationChangedEvent(newState.CurrentLocation);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // 1. Compare Player
        if (newState.Player != null && _previousState.Player != null)
        {
            if (!newState.Player.Equals(_previousState.Player))
            {
                var ev = new PlayerBitsChangedEvent(newState.Player.Bits);
                _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
            }
        }
        else if (newState.Player != null && _previousState.Player == null)
        {
            var ev = new PlayerBitsChangedEvent(newState.Player.Bits);
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

        // 3. Compare Important Items
        bool itemsChanged = false;
        var newItems = newState.ImportantItems ?? new Dictionary<string, bool>();
        var oldItems = _previousState.ImportantItems ?? new Dictionary<string, bool>();

        if (newItems.Count != oldItems.Count)
        {
            itemsChanged = true;
        }
        else
        {
            foreach (var kvp in newItems)
            {
                if (!oldItems.TryGetValue(kvp.Key, out bool oldVal) || oldVal != kvp.Value)
                {
                    itemsChanged = true;
                    break;
                }
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
            // Diff MainQuest
            if (newJournal.MainQuest.Id != oldJournal.MainQuest.Id)
            {
                journalChanged = true;
            }
            if (!journalChanged)
            {
                if (newJournal.MainQuest.Prerequisites.Count != oldJournal.MainQuest.Prerequisites.Count)
                {
                    journalChanged = true;
                }
                else
                {
                    for (int j = 0; j < newJournal.MainQuest.Prerequisites.Count; j++)
                    {
                        if (newJournal.MainQuest.Prerequisites[j].IsDone != oldJournal.MainQuest.Prerequisites[j].IsDone)
                        {
                            journalChanged = true;
                            break;
                        }
                    }
                }
            }
            if (!journalChanged)
            {
                if (newJournal.MainQuest.Steps.Count != oldJournal.MainQuest.Steps.Count)
                {
                    journalChanged = true;
                }
                else
                {
                    for (int j = 0; j < newJournal.MainQuest.Steps.Count; j++)
                    {
                        if (newJournal.MainQuest.Steps[j].IsCompleted != oldJournal.MainQuest.Steps[j].IsCompleted)
                        {
                            journalChanged = true;
                            break;
                        }
                        // Compare step-level prerequisites
                        var newPrereqs = newJournal.MainQuest.Steps[j].Prerequisites;
                        var oldPrereqs = oldJournal.MainQuest.Steps[j].Prerequisites;
                        if (newPrereqs != null && oldPrereqs != null && newPrereqs.Count == oldPrereqs.Count)
                        {
                            for (int k = 0; k < newPrereqs.Count; k++)
                            {
                                if (newPrereqs[k].IsDone != oldPrereqs[k].IsDone)
                                {
                                    journalChanged = true;
                                    break;
                                }
                            }
                        }
                        else if (newPrereqs?.Count != oldPrereqs?.Count)
                        {
                            journalChanged = true;
                        }
                        if (journalChanged) break;
                    }
                }
            }

            // Diff SideQuests
            if (!journalChanged)
            {
                if (newJournal.SideQuests.Count != oldJournal.SideQuests.Count)
                {
                    journalChanged = true;
                }
                else
                {
                    for (int i = 0; i < newJournal.SideQuests.Count; i++)
                    {
                        var newQ = newJournal.SideQuests[i];
                        var oldQ = oldJournal.SideQuests[i];

                        if (newQ.Id != oldQ.Id)
                        {
                            journalChanged = true;
                            break;
                        }

                        if (newQ.Prerequisites.Count != oldQ.Prerequisites.Count)
                        {
                            journalChanged = true;
                            break;
                        }

                        for (int j = 0; j < newQ.Prerequisites.Count; j++)
                        {
                            if (newQ.Prerequisites[j].IsDone != oldQ.Prerequisites[j].IsDone)
                            {
                                journalChanged = true;
                                break;
                            }
                        }
                        if (journalChanged) break;

                        if (newQ.Steps.Count != oldQ.Steps.Count)
                        {
                            journalChanged = true;
                            break;
                        }

                        for (int j = 0; j < newQ.Steps.Count; j++)
                        {
                            if (newQ.Steps[j].IsCompleted != oldQ.Steps[j].IsCompleted)
                            {
                                journalChanged = true;
                                break;
                            }
                            // Compare step-level prerequisites
                            var newSPrereqs = newQ.Steps[j].Prerequisites;
                            var oldSPrereqs = oldQ.Steps[j].Prerequisites;
                            if (newSPrereqs != null && oldSPrereqs != null && newSPrereqs.Count == oldSPrereqs.Count)
                            {
                                for (int k = 0; k < newSPrereqs.Count; k++)
                                {
                                    if (newSPrereqs[k].IsDone != oldSPrereqs[k].IsDone)
                                    {
                                        journalChanged = true;
                                        break;
                                    }
                                }
                            }
                            else if (newSPrereqs?.Count != oldSPrereqs?.Count)
                            {
                                journalChanged = true;
                            }
                            if (journalChanged) break;
                        }
                        if (journalChanged) break;
                    }
                }
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
        if (oldDigi.Attributes.Strength != newDigi.Attributes.Strength ||
            oldDigi.Attributes.Defense != newDigi.Attributes.Defense ||
            oldDigi.Attributes.Spirit != newDigi.Attributes.Spirit ||
            oldDigi.Attributes.Wisdom != newDigi.Attributes.Wisdom ||
            oldDigi.Attributes.Speed != newDigi.Attributes.Speed ||
            oldDigi.Attributes.Charisma != newDigi.Attributes.Charisma)
        {
            var ev = new DigimonAttributesChangedEvent(index, newDigi.Attributes.Strength, newDigi.Attributes.Defense, newDigi.Attributes.Spirit, newDigi.Attributes.Wisdom, newDigi.Attributes.Speed, newDigi.Attributes.Charisma);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }

        // Compare Resistances
        if (oldDigi.Resistances.Fire != newDigi.Resistances.Fire ||
            oldDigi.Resistances.Water != newDigi.Resistances.Water ||
            oldDigi.Resistances.Ice != newDigi.Resistances.Ice ||
            oldDigi.Resistances.Wind != newDigi.Resistances.Wind ||
            oldDigi.Resistances.Thunder != newDigi.Resistances.Thunder ||
            oldDigi.Resistances.Machine != newDigi.Resistances.Machine ||
            oldDigi.Resistances.Dark != newDigi.Resistances.Dark)
        {
            var ev = new DigimonResistancesChangedEvent(index, newDigi.Resistances.Fire, newDigi.Resistances.Water, newDigi.Resistances.Ice, newDigi.Resistances.Wind, newDigi.Resistances.Thunder, newDigi.Resistances.Machine, newDigi.Resistances.Dark);
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

        // Compare Equipped Digievolutions
        bool evosChanged = false;
        for (int i = 0; i < 3; i++)
        {
            var oldEvo = oldDigi.EquippedDigievolutions[i];
            var newEvo = newDigi.EquippedDigievolutions[i];

            if ((oldEvo == null && newEvo != null) || (oldEvo != null && newEvo == null))
            {
                evosChanged = true;
                continue;
            }
            if (oldEvo != null && newEvo != null && (oldEvo.Id != newEvo.Id || oldEvo.Level != newEvo.Level))
            {
                if (oldEvo.Id == newEvo.Id && newEvo.Level > oldEvo.Level)
                {
                    var levelUpEv = new DigimonDigievolutionLevelUpEvent(index, newEvo.Id, oldEvo.Level, newEvo.Level);
                    _ = _hubContext.Clients.All.SendAsync(levelUpEv.Type.ToString(), levelUpEv);
                }
                evosChanged = true;
            }
        }

        if (evosChanged)
        {
            var ev = new DigimonDigievolutionsChangedEvent(index, newDigi.EquippedDigievolutions);
            _ = _hubContext.Clients.All.SendAsync(ev.Type.ToString(), ev);
        }
    }

    // A simple Deep clone since records aren't being used
    private State CloneState(State s)
    {
        return new State
        {
            CurrentLocation = s.CurrentLocation,
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
            ImportantItems = s.ImportantItems != null ? new Dictionary<string, bool>(s.ImportantItems) : new Dictionary<string, bool>(),
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
