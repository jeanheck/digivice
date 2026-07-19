import { AuctionConverter } from "@/presenters/converter/auction.converter";
import type { Auction, Journal } from "@/models";
import { AuctionRepository } from "@/repositories/auction.repository";
import { QuestService } from "@/services/quest.service";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";
import type { AuctionListItemViewModel as AuctionViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";
import type { AuctionStepsRaw } from "@/repositories/tables/raws/auction/auction-steps.raw";
import { AuctionStatusConstant } from "@/constants/auction-status.constant";

export class AuctionPresenter {
	public static getAuctionCurrentViewModel(journal: Journal | null): AuctionCurrentViewModel {
		const activeAuctionListItem = this.getAuctionAvailableNow(journal);

		if (activeAuctionListItem === null) {
			return AuctionConverter.convertCurrent(null);
		}

		const activeAuctionRaw = AuctionRepository.getAuctionById(activeAuctionListItem.id);
		return AuctionConverter.convertCurrent(activeAuctionRaw);
	}

	public static getAuctionAvailableNow(journal: Journal | null): AuctionViewModel | null {
		return this.getAuctions(journal).find((auctionListItemViewModel) => {
			return auctionListItemViewModel.status === AuctionStatusConstant.availableNow;
		}) ?? null;
	}

	public static getAuctions(journal: Journal | null): AuctionViewModel[] {
		const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(journal?.mainQuest ?? null);

		return AuctionRepository.getAuctions().map((auctionRaw) => {
			const auction = journal?.auctions.find((auction) => {
				return auction.id === auctionRaw.id;
			});
			const auctionStatus = AuctionPresenter.getCalculatedAuctionStatus(
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
