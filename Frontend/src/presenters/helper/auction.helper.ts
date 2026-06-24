import type { AuctionStepsRaw } from "@/repositories/tables/raws/auction/auction-steps.raw";
import type { AuctionStatus } from "@/viewmodels/auction/auction-status";

export class AuctionHelper {
    public static resolveAuctionStatus(
        steps: AuctionStepsRaw,
        lastCompletedStep: number,
        hasParticipated: boolean,
    ): AuctionStatus {
        if (hasParticipated) {
            return "participated";
        }

        if (lastCompletedStep < steps.startsWhenComplete) {
            return "notYetOccurred";
        }

        if (lastCompletedStep >= steps.startsWhenComplete && lastCompletedStep < steps.endsWhenComplete) {
            return "availableNow";
        }

        return "missed";
    }
}
