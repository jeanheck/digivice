import CardShopJson from "@/data/game/shop/card-shop.json";
import type { CardShopTable } from "@/presentation/repositories/tables/tcg/card-shop.table";
import type {
  CardShopCardRaw,
  CardShopCatalogRaw,
} from "@/presentation/repositories/tables/raws/tcg/card-shop.raw";

export class CardShopRepository {
  private static readonly cardShopTable = CardShopJson as CardShopTable;
  private static readonly cardShopsByCardId = this.buildCardShopsByCardId();

  private static buildCardShopsByCardId(): Map<string, CardShopCardRaw[]> {
    const cardShopsByCardId = new Map<string, CardShopCardRaw[]>();
    for (const [cardShopId, cardShopRaw] of Object.entries(this.cardShopTable)) {
      for (const phase of cardShopRaw.phases) {
        for (const cardId of phase.inventory) {
          const cardShops = cardShopsByCardId.get(cardId) ?? [];
          cardShops.push({
            cardShopId,
            mainQuestAvailabilityWindow: phase.mainQuestAvailabilityWindow,
          });
          cardShopsByCardId.set(cardId, cardShops);
        }
      }
    }

    return cardShopsByCardId;
  }

  public static getById(cardShopId: string): CardShopCatalogRaw | undefined {
    return this.cardShopTable[cardShopId];
  }

  public static getTable(): CardShopTable {
    return this.cardShopTable;
  }

  public static getIds(): string[] {
    return Object.keys(this.cardShopTable);
  }

  public static getCardShopsByCardId(cardId: string): CardShopCardRaw[] {
    return this.cardShopsByCardId.get(cardId) ?? [];
  }
}
