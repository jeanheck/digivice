import { describe, it, expect, vi } from "vitest";
import type { Journal } from "@/models";
import type { Quest } from "@/models/quest";
import type { Step } from "@/models/step";
import { JournalPresenter } from "@/presenters/journal.presenter";

const minimalStepRaw = {
  location: "0000",
  coordinates: { x: 0, y: 0 },
  zoomedLocations: [],
  requisites: [],
};

vi.mock("@/repositories/quest.repository", () => ({
  QuestRepository: {
    getMainQuestRaw: () => ({
      id: "MainQuest",
      requisites: [],
      steps: {
        "1": minimalStepRaw,
        "2": minimalStepRaw,
      },
    }),
    getSideQuestsRaw: () => [
      {
        id: "FishingPole",
        requisites: [{ id: "FolderBag" }],
        steps: {
          "1": minimalStepRaw,
          "2": minimalStepRaw,
        },
      },
    ],
  },
}));

function buildSteps(stepNumbers: number[], completedStepNumbers: number[] = []): Step[] {
  return stepNumbers.map((stepNumber) => {
    return {
      number: stepNumber,
      isDone: completedStepNumbers.includes(stepNumber),
      requisites: [],
    };
  });
}

function buildFishingPoleQuest(completedStepNumbers: number[] = [], folderBagDone = true): Quest {
  return {
    id: "FishingPole",
    requisites: [{ id: "FolderBag", isDone: folderBagDone }],
    steps: buildSteps([1, 2], completedStepNumbers),
  };
}

function buildMainQuest(completedStepNumbers: number[] = []): Quest {
  return {
    id: "MainQuest",
    requisites: [],
    steps: buildSteps([1, 2], completedStepNumbers),
  };
}

function buildJournal(mainQuest: Quest | null, sideQuests: Quest[]): Journal {
  return {
    mainQuest,
    sideQuests,
  };
}

describe("JournalPresenter", () => {
  it("returns null main quest when journal main quest is null", () => {
    const journalViewModel = JournalPresenter.getJournalViewModel(
      buildJournal(null, [buildFishingPoleQuest()])
    );

    expect(journalViewModel.mainQuest).toBeNull();
  });

  it("enriches main quest with isLocked and isNew always false", () => {
    const journalViewModel = JournalPresenter.getJournalViewModel(
      buildJournal(buildMainQuest(), [buildFishingPoleQuest()])
    );

    expect(journalViewModel.mainQuest).not.toBeNull();
    expect(journalViewModel.mainQuest?.isLocked).toBe(false);
    expect(journalViewModel.mainQuest?.isNew).toBe(false);
    expect(journalViewModel.mainQuest?.currentStep?.number).toBe("1");
    expect(journalViewModel.mainQuest?.cardVariant).toBe("active");
  });

  it("marks main quest as done when every step is completed", () => {
    const journalViewModel = JournalPresenter.getJournalViewModel(
      buildJournal(buildMainQuest([1, 2]), [buildFishingPoleQuest()])
    );

    expect(journalViewModel.mainQuest?.isDone).toBe(true);
    expect(journalViewModel.mainQuest?.currentStep).toBeNull();
    expect(journalViewModel.mainQuest?.cardVariant).toBe("done");
  });

  it("marks side quest as locked when requisites are incomplete", () => {
    const journalViewModel = JournalPresenter.getJournalViewModel(
      buildJournal(buildMainQuest(), [buildFishingPoleQuest([], false)])
    );

    const fishingPole = journalViewModel.sideQuests.find((quest) => quest.id === "fishingPole");

    expect(fishingPole?.isLocked).toBe(true);
    expect(fishingPole?.isNew).toBe(false);
    expect(fishingPole?.cardVariant).toBe("locked");
  });

  it("marks side quest as new when step 1 is not completed", () => {
    const journalViewModel = JournalPresenter.getJournalViewModel(
      buildJournal(buildMainQuest(), [buildFishingPoleQuest()])
    );

    const fishingPole = journalViewModel.sideQuests.find((quest) => quest.id === "fishingPole");

    expect(fishingPole?.isLocked).toBe(false);
    expect(fishingPole?.isNew).toBe(true);
    expect(fishingPole?.cardVariant).toBe("new");
  });

  it("marks side quest as active when step 1 is done and another step remains", () => {
    const journalViewModel = JournalPresenter.getJournalViewModel(
      buildJournal(buildMainQuest(), [buildFishingPoleQuest([1])])
    );

    const fishingPole = journalViewModel.sideQuests.find((quest) => quest.id === "fishingPole");

    expect(fishingPole?.isDone).toBe(false);
    expect(fishingPole?.isNew).toBe(false);
    expect(fishingPole?.currentStep?.number).toBe("2");
    expect(fishingPole?.cardVariant).toBe("active");
  });

  it("marks side quest as done when every step is completed", () => {
    const journalViewModel = JournalPresenter.getJournalViewModel(
      buildJournal(buildMainQuest(), [buildFishingPoleQuest([1, 2])])
    );

    const fishingPole = journalViewModel.sideQuests.find((quest) => quest.id === "fishingPole");

    expect(fishingPole?.isDone).toBe(true);
    expect(fishingPole?.currentStep).toBeNull();
    expect(fishingPole?.cardVariant).toBe("done");
  });
});
