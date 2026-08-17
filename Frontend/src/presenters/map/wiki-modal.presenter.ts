import { CardRepository } from "@/repositories/card.repository";
import { DropRepository } from "@/repositories/drop.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import { SearchItemConverter } from "@/presenters/converter/search-item.converter";
import type { CardBoosterSourceViewModel } from "@/viewmodels/card/card-booster-source.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export class WikiModalPresenter {
  public static getEnemyById(enemyId: string): EnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    return EnemyConverter.convert(enemyRaw);
  }

  public static getEnemySearchItems(): SearchItemViewModel[] {
    return Object.entries(EnemyRepository.getEnemyTable()).map(([enemyId, enemyRaw]) => {
      return SearchItemConverter.convertEnemy(enemyId, enemyRaw);
    });
  }

  public static getDropSearchItems(translateDropName: (dropKey: string) => string): SearchItemViewModel[] {
    return DropRepository.getDropKeys().map((dropKey) => {
      return SearchItemConverter.convertDrop(dropKey, translateDropName(dropKey));
    });
  }

  public static getCardSearchItems(translateCardName: (cardId: string) => string): SearchItemViewModel[] {
    return CardRepository.getCardIds().map((cardId) => {
      return SearchItemConverter.convertCard(cardId, translateCardName(cardId));
    });
  }

  public static getAllSearchItems(
    translateDropName: (dropKey: string) => string,
    translateCardName: (cardId: string) => string,
  ): SearchItemViewModel[] {
    return [
      ...this.getEnemySearchItems(),
      ...this.getDropSearchItems(translateDropName),
      ...this.getCardSearchItems(translateCardName),
    ];
  }

  public static getCardBoosterSources(cardId: string): CardBoosterSourceViewModel[] {
    const cardRaw = CardRepository.getCardById(cardId);
    if (cardRaw === undefined) {
      return [];
    }

    const sources: CardBoosterSourceViewModel[] = [];

    for (const boosterId of cardRaw.boosters) {
      const dropKey = DropRepository.getDropKeyByNumericId(boosterId);
      if (dropKey === undefined) {
        continue;
      }

      sources.push({
        dropKey,
        boosterId,
      });
    }

    return sources;
  }
}
