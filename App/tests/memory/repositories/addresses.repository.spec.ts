import { describe, expect, it } from "vitest";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";

describe("AddressesRepository", () => {
  describe("real definitions", () => {
    it("should load every addresses file", () => {
      expect(AddressesRepository.getPlayerAddresses()).toEqual({
        bits: 0x48da0,
        mapId: 0x4b3f8,
        previousMapId: 0x4b400,
        seabedRoute: 0x48d78,
        mapVariant: 0x48d7a,
      });
      expect(AddressesRepository.getImportantItemsAddresses().treeBoots).not.toBe(0);
      expect(AddressesRepository.getCardBattleAddresses().id).not.toBe(0);
      expect(AddressesRepository.getDigimonBattleAddresses().field).not.toBe(0);
      expect(AddressesRepository.getEnemyAddresses().enemySlotBase).not.toBe(0);
      expect(AddressesRepository.getEnemyAddresses().slotCount).toBe(3);
      expect(AddressesRepository.getInBattleAddresses().allySlotBase).not.toBe(0);
      expect(AddressesRepository.getAuctionsAddresses().divineBarrier.address).not.toBe(0);
    });

    it("should load the party slots in order with the empty slot sentinel", () => {
      const partyAddresses = AddressesRepository.getPartyAddresses();

      expect(partyAddresses.emptySlotId).toBe(0xff);
      expect(partyAddresses.slots.map((slot) => slot.index)).toEqual([1, 2, 3]);
      expect(partyAddresses.slots.every((slot) => slot.address !== 0)).toBe(true);
    });

    it("should load the Digimon status offsets including the negative active digievolution", () => {
      const digimonStatusAddresses = AddressesRepository.getDigimonStatusAddresses();

      expect(digimonStatusAddresses.level).not.toBe(0);
      expect(digimonStatusAddresses.digievolutions.slots).not.toHaveLength(0);
      expect(digimonStatusAddresses.digievolutions.activeDigievolution).toBe(-4);
    });

    it("should load every Digimon address by numeric id", () => {
      const digimonsAddresses = AddressesRepository.getDigimonsAddresses();

      expect([...digimonsAddresses.keys()]).toEqual([0, 1, 2, 3, 4, 5, 6, 7]);
      expect(
        [...digimonsAddresses.values()].every((digimon) => digimon.memoryBlockAddress !== 0),
      ).toBe(true);
      expect(AddressesRepository.getDigimonAddressById(0)).toEqual({
        name: "Kotemon",
        memoryBlockAddress: 0x4949c,
        blastAddress: 0x42b74,
      });
    });

    it("should return null when the Digimon id is unknown", () => {
      expect(AddressesRepository.getDigimonAddressById(99)).toBeNull();
    });

    it("should load every NPC with battles", () => {
      const npcsAddresses = AddressesRepository.getNpcsAddresses();

      expect(Object.keys(npcsAddresses)).toHaveLength(35);
      for (const [name, npcAddresses] of Object.entries(npcsAddresses)) {
        expect(Object.keys(npcAddresses.battles).length, `${name} has no battles`).toBeGreaterThan(
          0,
        );
      }
      expect(npcsAddresses.genji.battles).toEqual({
        first: { address: 0x4b3df, bitMask: 0x20 },
        second: { address: 0x4b39a, bitMask: 0x01 },
      });
      expect(npcsAddresses.meiLin).toBeDefined();
      expect(npcsAddresses.optionalTrooper1AdminCenterB1F).toBeDefined();
    });

    it("should load every quest folder", () => {
      const folderQuests = [
        ...AddressesRepository.getAllSideQuests(),
        ...AddressesRepository.getAllLegendaryWeapons(),
        ...AddressesRepository.getAllDriAgents(),
        ...AddressesRepository.getAllDuelIsland(),
      ];

      expect(AddressesRepository.getMainQuest().steps).not.toHaveLength(0);
      expect(AddressesRepository.getAllSideQuests()).toHaveLength(3);
      expect(AddressesRepository.getAllLegendaryWeapons()).toHaveLength(5);
      expect(AddressesRepository.getAllDriAgents()).toHaveLength(8);
      expect(AddressesRepository.getAllDuelIsland()).toHaveLength(2);
      for (const quest of folderQuests) {
        expect(quest.id).not.toBe("");
        expect(quest.steps).not.toHaveLength(0);
      }
    });

    it("should order the quests of each folder by file name", () => {
      const ids = (quests: { id: string }[]): string[] => quests.map((quest) => quest.id);

      expect(ids(AddressesRepository.getAllSideQuests())).toEqual([
        "fishingPole",
        "folderBag",
        "treeBoots",
      ]);
      expect(ids(AddressesRepository.getAllLegendaryWeapons())).toEqual([
        "eternally",
        "invincible",
        "muramasa",
        "punishment",
        "superNova",
      ]);
      expect(ids(AddressesRepository.getAllDriAgents())).toEqual([
        "driAgentAgumon",
        "driAgentGuilmon",
        "driAgentKotemon",
        "driAgentKumamon",
        "driAgentMonmon",
        "driAgentPatamon",
        "driAgentRenamon",
        "driAgentVeemon",
      ]);
      expect(ids(AddressesRepository.getAllDuelIsland())).toEqual(["asukaRematch", "asukaTrophy"]);
    });

    it("should return the cached instance on subsequent calls", () => {
      expect(AddressesRepository.getPlayerAddresses()).toBe(
        AddressesRepository.getPlayerAddresses(),
      );
      expect(AddressesRepository.getAllDriAgents()).toBe(AddressesRepository.getAllDriAgents());
    });
  });
});
