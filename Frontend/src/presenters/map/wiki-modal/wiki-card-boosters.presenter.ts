import { BoosterConverter } from "@/presenters/converter/booster.converter";
import { BoosterRepository } from "@/repositories/booster.repository";
import { CardRepository } from "@/repositories/card.repository";
import type { BoosterViewModel } from "@/viewmodels/card/booster.viewmodel";

export class WikiCardBoostersPresenter {
  public static getViewModel(cardId: string): BoosterViewModel[] {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return [];
    }

    const boosters: BoosterViewModel[] = [];

    for (const boosterId of cardRaw.boosters) {
      if (BoosterRepository.getById(boosterId) === undefined) {
        continue;
      }

      boosters.push(
        BoosterConverter.convert({
          dropKey: String(boosterId),
          boosterId,
        }),
      );
    }

    return boosters;
  }
}
