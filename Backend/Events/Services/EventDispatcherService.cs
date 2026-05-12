using Backend.Events.Data;
using Backend.Events.Hubs;
using Backend.Models;
using Backend.Models.Quests;
using Backend.Models.Digimons;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Events.Services;

public class EventDispatcherService(
    IHubContext<GameHub> hubContext,
    ILogger<EventDispatcherService> logger,
    StateChangeDetector detector) : Interfaces.IEventDispatcherService
{
    private State? previousState;
    private bool? previousConnectionStatus;

    public void DispatchConnectionStatus(bool isConnected)
    {
        if (previousConnectionStatus == isConnected)
        {
            return;
        }

        previousConnectionStatus = isConnected;
        SafeDispatch(new ConnectionStatusChangedEvent(isConnected));

        if (!isConnected)
        {
            // Reset state so that next connection will do a full sync
            previousState = null;
        }
    }

    public void DispatchInitialStateToClient(string connectionId)
    {
        if (previousState != null)
        {
            SafeDispatch(new InitialStateSyncEvent(previousState), hubContext.Clients.Client(connectionId));
        }
    }

    public void ProcessGameState(State newState)
    {
        var events = detector.DetectChanges(previousState, newState);

        foreach (var ev in events)
        {
            SafeDispatch(ev);
        }

        previousState = CloneState(newState);
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
                    EquippedDigievolutions = new Digievolution[3]
                    {
                        d.EquippedDigievolutions[0] != null ? new Digievolution { Id = d.EquippedDigievolutions[0]!.Id, Level = d.EquippedDigievolutions[0]!.Level } : null,
                        d.EquippedDigievolutions[1] != null ? new Digievolution { Id = d.EquippedDigievolutions[1]!.Id, Level = d.EquippedDigievolutions[1]!.Level } : null,
                        d.EquippedDigievolutions[2] != null ? new Digievolution { Id = d.EquippedDigievolutions[2]!.Id, Level = d.EquippedDigievolutions[2]!.Level } : null
                    },
                    ActiveDigievolutionId = d.ActiveDigievolutionId
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
                    Prerequisites = s.Journal.MainQuest.Prerequisites.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone, ItemKey = p.ItemKey, ItemType = p.ItemType }).ToList(),
                    Steps = s.Journal.MainQuest.Steps.Select(step => new QuestStep
                    {
                        Number = step.Number,
                        IsCompleted = step.IsCompleted,
                        Address = step.Address,
                        BitMask = step.BitMask,
                        Prerequisites = step.Prerequisites?.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone, ItemKey = p.ItemKey, ItemType = p.ItemType }).ToList()
                    }).ToList()
                },
                SideQuests = s.Journal.SideQuests.Select(q => new SideQuest
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    Prerequisites = q.Prerequisites.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone, ItemKey = p.ItemKey, ItemType = p.ItemType }).ToList(),
                    Steps = q.Steps.Select(step => new QuestStep
                    {
                        Number = step.Number,
                        IsCompleted = step.IsCompleted,
                        Address = step.Address,
                        BitMask = step.BitMask,
                        Prerequisites = step.Prerequisites?.Select(p => new Requisite { Description = p.Description, IsDone = p.IsDone, ItemKey = p.ItemKey, ItemType = p.ItemType }).ToList()
                    }).ToList()
                }).ToList()
            } : null
        };
    }

    private void SafeDispatch(BaseEvent ev, IClientProxy? target = null)
    {
        target ??= hubContext.Clients.All;
        _ = target.SendAsync(ev.Type.ToString(), ev)
            .ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    logger.LogError(t.Exception, "Error dispatching event {Type}", ev.Type);
                }
            });
    }
}
