import type { CardRaw } from "@/repositories/tables/raws/tcg/card.raw";
import type { CardViewModel } from "@/viewmodels/card/card.viewmodel";

export class WikiCardDetailsConverter {
  public static convert(cardId: string, cardRaw: CardRaw): CardViewModel {
    return {
      imageName: cardRaw.imageName,
      nameKey: `cards.${cardId}.name`,
      noteKey: `cards.${cardId}.note`,
      type: cardRaw.type,
      points: cardRaw.points,
    };
  }
}
