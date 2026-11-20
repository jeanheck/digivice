import { describe, expect, it } from "vitest";
import type { JournalAddresses } from "@/memory/addresses/journal.addresses";
import type { QuestAddresses } from "@/memory/addresses/journals/quest.addresses";
import { JournalReader } from "@/memory/readers/journal.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

function singleStepQuest(id: string, address: number): QuestAddresses {
  return {
    id,
    requisites: [],
    steps: [{ number: 1, address, bitMasks: [], requisites: [] }],
  };
}

describe("JournalReader", () => {
  describe("read", () => {
    it("should read every category when the addresses are synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x1000, 1)
          .writeByte(0x2000, 2)
          .writeByte(0x3000, 3)
          .writeByte(0x4000, 4)
          .writeByte(0x5000, 5)
          .build(),
      );
      const addresses: JournalAddresses = {
        mainQuest: singleStepQuest("mainQuest", 0x1000),
        sideQuests: [singleStepQuest("sideQuest", 0x2000)],
        legendaryWeapons: [singleStepQuest("legendaryWeapon", 0x3000)],
        driAgents: [singleStepQuest("driAgent", 0x4000)],
        duelIsland: [singleStepQuest("duelIsland", 0x5000)],
      };

      const resource = JournalReader.read(snapshot, addresses);

      const step = (value: number) => [{ number: 1, value, requisites: [] }];
      expect(resource).toEqual({
        mainQuest: { id: "mainQuest", requisites: [], steps: step(1) },
        sideQuests: [{ id: "sideQuest", requisites: [], steps: step(2) }],
        legendaryWeapons: [{ id: "legendaryWeapon", requisites: [], steps: step(3) }],
        driAgents: [{ id: "driAgent", requisites: [], steps: step(4) }],
        duelIsland: [{ id: "duelIsland", requisites: [], steps: step(5) }],
      });
    });

    it("should keep the repository order in every category when the addresses are real", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());
      const addresses: JournalAddresses = {
        mainQuest: AddressesRepository.getMainQuest(),
        sideQuests: AddressesRepository.getAllSideQuests(),
        legendaryWeapons: AddressesRepository.getAllLegendaryWeapons(),
        driAgents: AddressesRepository.getAllDriAgents(),
        duelIsland: AddressesRepository.getAllDuelIsland(),
      };
      const ids = (quests: { id: string }[]) => quests.map((quest) => quest.id);

      const resource = JournalReader.read(snapshot, addresses);

      expect(resource.mainQuest.id).toBe("mainQuest");
      expect(ids(resource.sideQuests)).toEqual(ids(addresses.sideQuests));
      expect(ids(resource.legendaryWeapons)).toEqual(ids(addresses.legendaryWeapons));
      expect(ids(resource.driAgents)).toEqual(ids(addresses.driAgents));
      expect(ids(resource.duelIsland)).toEqual(ids(addresses.duelIsland));
    });
  });
});
