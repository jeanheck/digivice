import { CardRepository } from "@/repositories/card.repository";
import { DropRepository } from "@/repositories/drop.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { LocationRepository } from "@/repositories/location.repository";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import { SearchItemConverter } from "@/presenters/converter/search-item.converter";
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

  public static getLocationSearchItems(
    translateLocationName: (locationId: string) => string,
  ): SearchItemViewModel[] {
    return LocationRepository.getLocationIdsWithWorldLocation().map((locationId) => {
      return SearchItemConverter.convertLocation(locationId, translateLocationName(locationId));
    });
  }

  public static getAllSearchItems(
    translateDropName: (dropKey: string) => string,
    translateCardName: (cardId: string) => string,
    translateLocationName: (locationId: string) => string,
  ): SearchItemViewModel[] {
    return [
      ...this.getEnemySearchItems(),
      ...this.getDropSearchItems(translateDropName),
      ...this.getCardSearchItems(translateCardName),
      ...this.getLocationSearchItems(translateLocationName),
    ];
  }
}
