import type { QuestAddresses } from "@/memory/addresses/journals/quest.addresses";
import type { RequisiteAddresses } from "@/memory/addresses/journals/quests/requisite.addresses";
import type { StepAddresses } from "@/memory/addresses/journals/quests/step.addresses";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";
import type { RequisiteResource } from "@/memory/resources/journals/quests/requisite.resource";
import type { StepResource } from "@/memory/resources/journals/quests/step.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class QuestReader {
  private static readValue(snapshot: MemorySnapshot, address: number, bitMasks: number[]): number {
    const [firstBitMask] = bitMasks;
    if (firstBitMask === undefined) {
      return snapshot.readByte(address);
    }

    if (bitMasks.length === 1) {
      return snapshot.readByte(address, firstBitMask);
    }

    // Multiple bit masks require every bit to be set; the value keeps only the first mask.
    const rawValue = snapshot.readByte(address);
    if (bitMasks.some((bitMask) => (rawValue & bitMask) === 0)) {
      return 0;
    }

    return rawValue & firstBitMask;
  }

  private static readRequisite(
    snapshot: MemorySnapshot,
    addresses: RequisiteAddresses,
  ): RequisiteResource {
    return {
      id: addresses.id,
      value: QuestReader.readValue(snapshot, addresses.address, addresses.bitMasks),
    };
  }

  private static readStep(snapshot: MemorySnapshot, addresses: StepAddresses): StepResource {
    return {
      number: addresses.number,
      value: QuestReader.readValue(snapshot, addresses.address, addresses.bitMasks),
      requisites: addresses.requisites.map((requisite) =>
        QuestReader.readRequisite(snapshot, requisite),
      ),
    };
  }

  public static read(snapshot: MemorySnapshot, addresses: QuestAddresses): QuestResource {
    return {
      id: addresses.id,
      requisites: addresses.requisites.map((requisite) =>
        QuestReader.readRequisite(snapshot, requisite),
      ),
      steps: addresses.steps.map((step) => QuestReader.readStep(snapshot, step)),
    };
  }
}
