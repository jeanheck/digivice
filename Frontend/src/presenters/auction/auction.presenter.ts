import { AuctionConverter } from "@/presenters/converter/auction.converter";
import { AuctionHelper } from "@/presenters/helper/auction.helper";
import { QuestHelper } from "@/presenters/helper/quest.helper";
import type { Journal } from "@/models";
import { AuctionRepository } from "@/repositories/auction.repository";
import type { AuctionCardViewModel } from "@/viewmodels/auction/auction-card.viewmodel";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionPresenter {
    public static getAuctionCardViewModel(journal: Journal | null): AuctionCardViewModel {
        const activeAuctionListItem = this.getActiveAuctionListItemViewModel(journal);

        return AuctionConverter.convertCard(
            activeAuctionListItem !== null,
            activeAuctionListItem?.equipmentId ?? null,
        );
    }

    public static getAuctionCurrentViewModel(journal: Journal | null): AuctionCurrentViewModel {
        const activeAuctionListItem = this.getActiveAuctionListItemViewModel(journal);

        if (activeAuctionListItem === null) {
            return AuctionConverter.convertCurrent(null);
        }

        const activeAuctionRaw = AuctionRepository.getAuctionById(activeAuctionListItem.id);
        return AuctionConverter.convertCurrent(activeAuctionRaw);
    }

    public static getAuctionHistoryViewModels(journal: Journal | null): AuctionListItemViewModel[] {
        return this.buildAuctionListItemViewModels(journal);
    }

    private static getActiveAuctionListItemViewModel(journal: Journal | null): AuctionListItemViewModel | null {
        return this.buildAuctionListItemViewModels(journal).find((auctionListItemViewModel) => {
            return auctionListItemViewModel.status === "availableNow";
        }) ?? null;
    }

    private static buildAuctionListItemViewModels(journal: Journal | null): AuctionListItemViewModel[] {
        const lastCompletedMainQuestStep = QuestHelper.getLastCompletedMainQuestStep(journal?.mainQuest ?? null);

        return AuctionRepository.getAllAuctionsRaw().map((auctionRaw) => {
            const auctionRuntime = journal?.auctions.find((auction) => {
                return auction.id === auctionRaw.id;
            });
            const hasParticipated = auctionRuntime?.hasParticipated ?? false;
            const status = AuctionHelper.resolveAuctionStatus(
                auctionRaw.steps,
                lastCompletedMainQuestStep,
                hasParticipated,
            );

            return AuctionConverter.convertListItem(auctionRaw, status);
        });
    }
}
