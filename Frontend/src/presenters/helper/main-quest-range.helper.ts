export class MainQuestRangeHelper {
  public static isInMainQuestRange(
    startWhenLastMainQuestStepDone: string | undefined,
    finishWhenLastMainQuestStepDone: string | undefined,
    lastCompletedMainQuestStep: number,
  ): boolean {
    const start = MainQuestRangeHelper.parseStart(startWhenLastMainQuestStepDone);
    const finish = MainQuestRangeHelper.parseFinish(finishWhenLastMainQuestStepDone);

    return lastCompletedMainQuestStep >= start && lastCompletedMainQuestStep <= finish;
  }

  public static parseStart(value: string | undefined): number {
    if (value === undefined || value === "") {
      return 0;
    }

    const parsedValue = Number(value);
    if (Number.isNaN(parsedValue)) {
      return 0;
    }

    return parsedValue;
  }

  public static parseFinish(value: string | undefined): number {
    if (value === undefined || value === "") {
      return Number.POSITIVE_INFINITY;
    }

    const parsedValue = Number(value);
    if (Number.isNaN(parsedValue)) {
      return Number.POSITIVE_INFINITY;
    }

    return parsedValue;
  }
}
