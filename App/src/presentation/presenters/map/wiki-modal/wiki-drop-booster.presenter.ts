import { WikiDropBoosterCardConverter } from "@/presentation/presenters/converter/wiki-drop-booster-card.converter";
import { BoosterRepository } from "@/presentation/repositories/booster.repository";
import { CardRepository } from "@/presentation/repositories/card.repository";
import type { WikiDropBoosterCardViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-drop-booster-card.viewmodel";

export class WikiDropBoosterPresenter {
  public static getViewModel(boosterId: number): WikiDropBoosterCardViewModel[] {
    const cards: WikiDropBoosterCardViewModel[] = [];

    for (const cardId of BoosterRepository.getCardIdsByBoosterId(boosterId)) {
      if (CardRepository.getCardById(String(cardId)) === undefined) {
        continue;
      }

      cards.push(WikiDropBoosterCardConverter.convert(cardId));
    }

    return cards;
  }
}
