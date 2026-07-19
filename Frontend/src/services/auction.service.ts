import { AuctionStatusConstant } from "@/constants/auction-status.constant";
import type { Auction, Journal } from "@/models";
import { AuctionConverter } from "@/presenters/converter/auction.converter";
import { AuctionRepository } from "@/repositories/auction.repository";
import type { AuctionStepsRaw } from "@/repositories/tables/raws/auction/auction-steps.raw";
import { QuestService } from "@/services/quest.service";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionService {
    public static getAuctionAvailableNow(journal: Journal | null): AuctionListItemViewModel | null {
        return this.getAuctions(journal).find((auctionListItemViewModel) => {
            return auctionListItemViewModel.status === AuctionStatusConstant.availableNow;
        }) ?? null;
    }

    public static getAuctions(journal: Journal | null): AuctionListItemViewModel[] {
        const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(journal?.mainQuest ?? null);

        return AuctionRepository.getAuctions().map((auctionRaw) => {
            const auction = journal?.auctions.find((auction) => {
                return auction.id === auctionRaw.id;
            });
            const auctionStatus = this.getCalculatedAuctionStatus(
                auctionRaw.steps,
                lastCompletedMainQuestStep,
                auction,
            );

            return AuctionConverter.convert(auctionRaw, auctionStatus);
        });
    }

    private static getCalculatedAuctionStatus(
        steps: AuctionStepsRaw,
        lastCompletedStep: number,
        auction: Auction | undefined,
    ): AuctionStatusConstant {
        const hasParticipated = auction?.hasParticipated ?? false;
        if (hasParticipated) {
            return AuctionStatusConstant.participated;
        }

        if (lastCompletedStep < steps.startsWhenComplete) {
            return AuctionStatusConstant.notYetOccurred;
        }

        if (lastCompletedStep >= steps.startsWhenComplete && lastCompletedStep < steps.endsWhenComplete) {
            return AuctionStatusConstant.availableNow;
        }

        return AuctionStatusConstant.missed;
    }
}
