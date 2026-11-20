import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";
import type { Digimon, State } from "@/state/models";
import { storeLogger } from "@/core/logger/logger";
import { StatePatcher } from "@/memory/patch/state.patcher";
import { BattleAgainstNatsumiExpected } from "../../fixtures/snapshots/battle-against-natsumi.expected";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";

function centralParkState(): State {
  return structuredClone(FieldCentralParkExpected);
}

function getDigimon(state: State, slotPosition: number): Digimon {
  const digimon = state.party.slots[slotPosition]?.digimon;
  if (!digimon) {
    throw new Error(`Slot ${slotPosition} has no Digimon.`);
  }

  return digimon;
}

describe("StatePatcher", () => {
  let warn: ReturnType<typeof vi.spyOn>;

  beforeEach(() => {
    warn = vi.spyOn(storeLogger, "warn").mockImplementation(() => {});
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  describe("patch", () => {
    it("should keep every object and list reference when the source has the same values", () => {
      const target = centralParkState();
      const { party, journal, npcs } = target;
      const firstSlot = target.party.slots[0];
      const kotemon = getDigimon(target, 0);
      const sideQuests = target.journal.sideQuests;

      new StatePatcher().patch(target, centralParkState());

      expect(target).toEqual(FieldCentralParkExpected);
      expect(target.party).toBe(party);
      expect(target.journal).toBe(journal);
      expect(target.npcs).toBe(npcs);
      expect(target.party.slots[0]).toBe(firstSlot);
      expect(getDigimon(target, 0)).toBe(kotemon);
      expect(target.journal.sideQuests).toBe(sideQuests);
    });

    it("should not write a primitive when the value is the same", () => {
      const target = centralParkState();
      let bits = target.player.bits;
      const setBits = vi.fn((value: number) => {
        bits = value;
      });
      Object.defineProperty(target.player, "bits", {
        get: () => bits,
        set: setBits,
        enumerable: true,
      });

      new StatePatcher().patch(target, centralParkState());

      expect(setBits).not.toHaveBeenCalled();
    });

    it("should update a primitive in place when the value changes", () => {
      const target = centralParkState();
      const player = target.player;
      const source = centralParkState();
      source.player.bits = 999;
      source.player.mapVariant = 2;

      new StatePatcher().patch(target, source);

      expect(target.player).toBe(player);
      expect(target.player.bits).toBe(999);
      expect(target.player.mapVariant).toBe(2);
    });

    it("should assign the source enemy when an enemy appears", () => {
      const target = centralParkState();
      const source = centralParkState();
      source.digimonBattle = structuredClone(BattleAgainstNatsumiExpected.digimonBattle);

      new StatePatcher().patch(target, source);

      expect(target.digimonBattle.enemy).toBe(source.digimonBattle.enemy);
    });

    it("should set the enemy to null when the enemy leaves", () => {
      const target = centralParkState();
      target.digimonBattle = structuredClone(BattleAgainstNatsumiExpected.digimonBattle);

      new StatePatcher().patch(target, centralParkState());

      expect(target.digimonBattle.enemy).toBeNull();
    });

    it("should patch the enemy in place when the enemy stays", () => {
      const target = centralParkState();
      target.digimonBattle = structuredClone(BattleAgainstNatsumiExpected.digimonBattle);
      const enemy = target.digimonBattle.enemy;
      const source = centralParkState();
      source.digimonBattle = structuredClone(BattleAgainstNatsumiExpected.digimonBattle);
      source.digimonBattle.enemy!.hp.current = 50;

      new StatePatcher().patch(target, source);

      expect(target.digimonBattle.enemy).toBe(enemy);
      expect(target.digimonBattle.enemy?.hp.current).toBe(50);
    });

    it("should match list items by key when the source order is different", () => {
      const target = centralParkState();
      const targetIds = target.journal.sideQuests.map((quest) => quest.id);
      const firstQuest = target.journal.sideQuests[0]!;
      const source = centralParkState();
      source.journal.sideQuests.reverse();
      const sourceFirstQuest = source.journal.sideQuests.find(
        (quest) => quest.id === firstQuest.id,
      )!;
      sourceFirstQuest.steps[0]!.isDone = !sourceFirstQuest.steps[0]!.isDone;

      new StatePatcher().patch(target, source);

      expect(target.journal.sideQuests.map((quest) => quest.id)).toEqual(targetIds);
      expect(target.journal.sideQuests[0]).toBe(firstQuest);
      expect(firstQuest.steps[0]?.isDone).toBe(sourceFirstQuest.steps[0]?.isDone);
    });

    it("should push new items and remove missing items in keyed lists", () => {
      const target = centralParkState();
      const source = centralParkState();
      source.npcs.genji.battles = [
        ...source.npcs.genji.battles.slice(1),
        { id: "extraBattle", won: true },
      ];

      new StatePatcher().patch(target, source);

      expect(target.npcs.genji.battles).toEqual(source.npcs.genji.battles);
    });
  });

  describe("patch digimon slots", () => {
    it("should patch the Digimon in place when the digimon id is the same", () => {
      const target = centralParkState();
      const kotemon = getDigimon(target, 0);
      const source = centralParkState();
      getDigimon(source, 0).hp.current = 1;

      new StatePatcher().patch(target, source);

      expect(getDigimon(target, 0)).toBe(kotemon);
      expect(kotemon.hp.current).toBe(1);
    });

    it("should replace the whole Digimon when the digimon id changes", () => {
      const target = centralParkState();
      const source = centralParkState();
      source.party.slots[0] = { ...source.party.slots[1]!, index: 1 };

      new StatePatcher().patch(target, source);

      expect(target.party.slots[0]?.digimonId).toBe(7);
      expect(target.party.slots[0]?.digimon).toBe(source.party.slots[0]?.digimon);
    });

    it("should empty the slot when the Digimon leaves the party", () => {
      const target = centralParkState();
      const source = centralParkState();
      source.party.slots[2] = { index: 3, digimonId: null, digimon: null };

      new StatePatcher().patch(target, source);

      expect(target.party.slots[2]).toEqual({ index: 3, digimonId: null, digimon: null });
    });

    it("should patch the Central Park party into the battle party", () => {
      const target = centralParkState();
      const kotemon = getDigimon(target, 0);
      const source = centralParkState();
      source.party = structuredClone(BattleAgainstNatsumiExpected.party);

      new StatePatcher().patch(target, source);

      expect(target.party).toEqual(BattleAgainstNatsumiExpected.party);
      expect(getDigimon(target, 0)).toBe(kotemon);
    });
  });

  describe("patch digievolution slots", () => {
    function withEmptyThirdDigievolutionSlot(state: State): State {
      getDigimon(state, 0).digievolutions[2] = {
        index: 3,
        digievolutionId: null,
        digievolution: null,
      };
      return state;
    }

    it("should ignore a filled → empty transition and warn once", () => {
      const target = centralParkState();
      const statePatcher = new StatePatcher();

      statePatcher.patch(target, withEmptyThirdDigievolutionSlot(centralParkState()));
      statePatcher.patch(target, withEmptyThirdDigievolutionSlot(centralParkState()));

      expect(getDigimon(target, 0).digievolutions[2]).toEqual(
        getDigimon(FieldCentralParkExpected, 0).digievolutions[2],
      );
      expect(warn).toHaveBeenCalledOnce();
    });

    it("should warn again when the slot is filled and then emptied again", () => {
      const target = centralParkState();
      const statePatcher = new StatePatcher();

      statePatcher.patch(target, withEmptyThirdDigievolutionSlot(centralParkState()));
      statePatcher.patch(target, centralParkState());
      statePatcher.patch(target, withEmptyThirdDigievolutionSlot(centralParkState()));

      expect(warn).toHaveBeenCalledTimes(2);
    });

    it("should warn again when the Digimon was replaced in between", () => {
      const target = centralParkState();
      const statePatcher = new StatePatcher();
      const patamonInFirstSlot = centralParkState();
      patamonInFirstSlot.party.slots[0] = { ...patamonInFirstSlot.party.slots[1]!, index: 1 };

      statePatcher.patch(target, withEmptyThirdDigievolutionSlot(centralParkState()));
      statePatcher.patch(target, patamonInFirstSlot);
      statePatcher.patch(target, centralParkState());
      statePatcher.patch(target, withEmptyThirdDigievolutionSlot(centralParkState()));

      expect(warn).toHaveBeenCalledTimes(2);
    });

    it("should fill an empty slot with the source digievolution", () => {
      const target = withEmptyThirdDigievolutionSlot(centralParkState());

      new StatePatcher().patch(target, centralParkState());

      expect(getDigimon(target, 0).digievolutions[2]).toEqual(
        getDigimon(FieldCentralParkExpected, 0).digievolutions[2],
      );
      expect(warn).not.toHaveBeenCalled();
    });

    it("should patch the level and dvexp when the digievolution id changes", () => {
      const target = centralParkState();
      const source = centralParkState();
      getDigimon(source, 0).digievolutions[1] = {
        index: 2,
        digievolutionId: 20,
        digievolution: { level: 7, dvexp: 60 },
      };

      new StatePatcher().patch(target, source);

      expect(getDigimon(target, 0).digievolutions[1]).toEqual({
        index: 2,
        digievolutionId: 20,
        digievolution: { level: 7, dvexp: 60 },
      });
    });
  });

  describe("patch stored digievolutions", () => {
    it("should keep entries missing from the source", () => {
      const target = centralParkState();
      const source = centralParkState();
      getDigimon(source, 0).storedDigievolutions.splice(1, 1);

      new StatePatcher().patch(target, source);

      expect(getDigimon(target, 0).storedDigievolutions).toEqual(
        getDigimon(FieldCentralParkExpected, 0).storedDigievolutions,
      );
    });

    it("should append new entries and patch the level of existing ones", () => {
      const target = centralParkState();
      const source = centralParkState();
      const sourceStored = getDigimon(source, 0).storedDigievolutions;
      sourceStored[2]!.level = 10;
      sourceStored.unshift({ digievolutionId: 400, level: 1 });

      new StatePatcher().patch(target, source);

      expect(getDigimon(target, 0).storedDigievolutions).toEqual([
        { digievolutionId: 386, level: 99 },
        { digievolutionId: 5, level: 1 },
        { digievolutionId: 389, level: 10 },
        { digievolutionId: 400, level: 1 },
      ]);
    });
  });
});
