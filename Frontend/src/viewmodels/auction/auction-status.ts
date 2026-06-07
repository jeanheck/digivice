export type AuctionModalStatus =
    | "notYetOccurred"
    | "bought"
    | "participatedWithoutPurchase"
    | "missed";

export type AuctionRuntimeStatus = AuctionModalStatus | "availableNow";
