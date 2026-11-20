export class PriceService {
  public static getResaleValue(price: number): number {
    return Math.floor(price / 2);
  }
}
