import { BoosterConverter } from "@/presenters/converter/booster.converter";
import { BoosterRepository } from "@/repositories/booster.repository";
import type { BoosterViewModel } from "@/viewmodels/card/booster.viewmodel";

export class WikiCardBoostersPresenter {
  public static getViewModel(boosterIds: number[]): BoosterViewModel[] {
    const boosters: BoosterViewModel[] = [];

    for (const boosterId of boosterIds) {
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
