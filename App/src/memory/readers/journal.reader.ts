import type { JournalAddresses } from "@/memory/addresses/journal.addresses";
import type { QuestAddresses } from "@/memory/addresses/journals/quest.addresses";
import { QuestReader } from "@/memory/readers/quest.reader";
import type { JournalResource } from "@/memory/resources/journal.resource";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class JournalReader {
  private static readQuests(
    snapshot: MemorySnapshot,
    addresses: QuestAddresses[],
  ): QuestResource[] {
    return addresses.map((questAddresses) => QuestReader.read(snapshot, questAddresses));
  }

  public static read(snapshot: MemorySnapshot, addresses: JournalAddresses): JournalResource {
    return {
      mainQuest: QuestReader.read(snapshot, addresses.mainQuest),
      sideQuests: JournalReader.readQuests(snapshot, addresses.sideQuests),
      legendaryWeapons: JournalReader.readQuests(snapshot, addresses.legendaryWeapons),
      driAgents: JournalReader.readQuests(snapshot, addresses.driAgents),
      duelIsland: JournalReader.readQuests(snapshot, addresses.duelIsland),
    };
  }
}
