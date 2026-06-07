import type { Journal } from "../../models";
import type * as Events from "../../events/events.map";
import { AuctionSyncer } from "./journals/auction.syncer";
import { QuestSyncer } from "./journals/quest.syncer";

export class JournalSyncer {
    public static sync(previousJournal: Journal, newJournalDto: Events.JournalDTO): void {
        if (newJournalDto.mainQuest && previousJournal.mainQuest) {
            QuestSyncer.sync(previousJournal.mainQuest, newJournalDto.mainQuest);
        }

        if (newJournalDto.sideQuests && newJournalDto.sideQuests.length > 0) {
            newJournalDto.sideQuests.forEach((newSideQuestDto) => {
                const previousSideQuest = previousJournal.sideQuests.find((q) => q.id === newSideQuestDto.id);

                if (previousSideQuest) {
                    QuestSyncer.sync(previousSideQuest, newSideQuestDto);
                }
            });
        }

        if (newJournalDto.legendaryWeapons && newJournalDto.legendaryWeapons.length > 0) {
            newJournalDto.legendaryWeapons.forEach((newLegendaryWeaponDto) => {
                const previousLegendaryWeapon = previousJournal.legendaryWeapons.find((quest) => {
                    return quest.id === newLegendaryWeaponDto.id;
                });

                if (previousLegendaryWeapon) {
                    QuestSyncer.sync(previousLegendaryWeapon, newLegendaryWeaponDto);
                }
            });
        }

        if (newJournalDto.driAgents && newJournalDto.driAgents.length > 0) {
            newJournalDto.driAgents.forEach((newDriAgentDto) => {
                const previousDriAgent = previousJournal.driAgents.find((quest) => {
                    return quest.id === newDriAgentDto.id;
                });

                if (previousDriAgent) {
                    QuestSyncer.sync(previousDriAgent, newDriAgentDto);
                }
            });
        }

        if (newJournalDto.auctions && newJournalDto.auctions.length > 0) {
            newJournalDto.auctions.forEach((newAuctionDto) => {
                const previousAuction = previousJournal.auctions.find((auction) => {
                    return auction.id === newAuctionDto.id;
                });

                if (previousAuction) {
                    AuctionSyncer.sync(previousAuction, newAuctionDto);
                }
            });
        }
    }
}

