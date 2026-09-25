import CardShopJson from "@/database/tcg/card-shops.json";
import type { CardShopTable } from "@/repositories/tables/tcg/card-shop.table";
import type { CardShopCatalogRaw } from "@/repositories/tables/raws/tcg/card-shop.raw";

export class CardShopRepository {
  private static readonly cardShopTable = CardShopJson as CardShopTable;

  public static getById(cardShopId: string): CardShopCatalogRaw | undefined {
    return this.cardShopTable[cardShopId];
  }

  public static getTable(): CardShopTable {
    return this.cardShopTable;
  }

  public static getIds(): string[] {
    return Object.keys(this.cardShopTable);
  }
}
