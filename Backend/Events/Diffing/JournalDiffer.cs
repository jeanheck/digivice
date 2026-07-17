using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Journals;
using Backend.Events.DTO;
using Backend.Events.DTO.Auctions;
using Backend.Events.DTO.Journals;

namespace Backend.Events.Diffing;

public static class JournalDiffer
{
    public static JournalDTO Diff(Journal? previousJournal, Journal newJournal)
    {
        if (newJournal.HasNoChanges(previousJournal))
        {
            return new JournalDTO();
        }

        if (previousJournal == null)
        {
            return JournalConverter.ToDTO(newJournal);
        }

        var dto = new JournalDTO();

        var mainQuestDelta = QuestDiffer.Diff(previousJournal.MainQuest, newJournal.MainQuest);
        if (mainQuestDelta != null)
        {
            dto = dto with { MainQuest = mainQuestDelta };
        }

        var sideQuestsDelta = GenerateQuestsDtos(newJournal.SideQuests, previousJournal.SideQuests);
        if (sideQuestsDelta.Count > 0)
        {
            dto = dto with { SideQuests = sideQuestsDelta };
        }

        var legendaryWeaponsDelta = GenerateQuestsDtos(newJournal.LegendaryWeapons, previousJournal.LegendaryWeapons);
        if (legendaryWeaponsDelta.Count > 0)
        {
            dto = dto with { LegendaryWeapons = legendaryWeaponsDelta };
        }

        var driAgentsDelta = GenerateQuestsDtos(newJournal.DriAgents, previousJournal.DriAgents);
        if (driAgentsDelta.Count > 0)
        {
            dto = dto with { DriAgents = driAgentsDelta };
        }

        var auctionsDelta = new List<AuctionDTO>();
        foreach (var newAuction in newJournal.Auctions)
        {
            var previousAuction = previousJournal.Auctions.FirstOrDefault(auction => auction.Id == newAuction.Id);
            var auctionDelta = AuctionDiffer.Diff(previousAuction, newAuction);
            if (auctionDelta != null)
            {
                auctionsDelta.Add(auctionDelta);
            }
        }

        if (auctionsDelta.Count > 0)
        {
            dto = dto with { Auctions = auctionsDelta };
        }

        return dto;
    }

    private static List<QuestDTO> GenerateQuestsDtos(List<Quest> newJournalQuests, List<Quest> previousJournalQuests)
    {
        var deltas = new List<QuestDTO>();
        foreach (var quest in newJournalQuests)
        {
            var sideQuestPreviousState = previousJournalQuests.FirstOrDefault(q => q.Id == quest.Id);
            var sideQuestDelta = QuestDiffer.Diff(sideQuestPreviousState, quest);
            if (sideQuestDelta != null)
            {
                deltas.Add(sideQuestDelta);
            }
        }

        return deltas;
    }
}
