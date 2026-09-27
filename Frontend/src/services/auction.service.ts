import type { Auctions, Quest } from "@/models";
import { AuctionConverter } from "@/presenters/converter/auction.converter";
import { AuctionRepository } from "@/repositories/auction.repository";
import type { MainQuestAvailabilityWindowRaw } from "@/repositories/tables/raws/auction/main-quest-availability-window.raw";
import { QuestService } from "@/services/quest.service";
import type { AuctionStatus } from "@/types/auction-status.type";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class AuctionService {
  public static getAvailableAuction(auctions: Auctions, mainQuest: Quest): AuctionViewModel | null {
    return (
      this.getAuctions(auctions, mainQuest).find((auctionListItemViewModel) => {
        return auctionListItemViewModel.status === "available";
      }) ?? null
    );
  }

  public static getAuctions(auctions: Auctions, mainQuest: Quest): AuctionViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);

    return AuctionRepository.getAuctions().map((auctionRaw) => {
      const hasParticipated = auctions[auctionRaw.id as keyof Auctions] ?? false;
      const auctionStatus = this.getCalculatedAuctionStatus(
        auctionRaw.mainQuestAvailabilityWindow,
        lastCompletedMainQuestStep,
        hasParticipated,
      );

      return AuctionConverter.convert(auctionRaw, auctionStatus);
    });
  }

  private static getCalculatedAuctionStatus(
    mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw,
    lastCompletedStep: number,
    hasParticipated: boolean,
  ): AuctionStatus {
    if (hasParticipated) {
      return "participated";
    }
    if (lastCompletedStep < mainQuestAvailabilityWindow.starts) {
      return "notYetOccurred";
    }
    if (
      lastCompletedStep >= mainQuestAvailabilityWindow.starts &&
      lastCompletedStep < mainQuestAvailabilityWindow.ends
    ) {
      return "available";
    }

    return "missed";
  }
}
