import type { CardRaw } from "@/repositories/tables/raws/tcg/card.raw";
import type { WikiCardDetailsViewModel } from "@/viewmodels/wiki-modal/wiki-card-details.viewmodel";

export class WikiCardDetailsConverter {
  public static convert(cardId: string, cardRaw: CardRaw): WikiCardDetailsViewModel {
    return {
      imageName: cardRaw.imageName,
      nameKey: `cards.${cardId}.name`,
      noteKey: `cards.${cardId}.note`,
      type: cardRaw.type,
      points: cardRaw.points,
    };
  }
}
