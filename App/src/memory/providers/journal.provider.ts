import type { Journal } from "@/state/models";
import type { JournalAddresses } from "@/memory/addresses/journal.addresses";
import { JournalAssembler } from "@/memory/assemblers/journal.assembler";
import { JournalReader } from "@/memory/readers/journal.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";

export class JournalProvider {
  private static getAddresses(): JournalAddresses {
    return {
      mainQuest: AddressesRepository.getMainQuest(),
      sideQuests: AddressesRepository.getAllSideQuests(),
      legendaryWeapons: AddressesRepository.getAllLegendaryWeapons(),
      driAgents: AddressesRepository.getAllDriAgents(),
      duelIsland: AddressesRepository.getAllDuelIsland(),
    };
  }

  public static get(snapshot: MemorySnapshot): Journal {
    const resource = JournalReader.read(snapshot, JournalProvider.getAddresses());
    return JournalAssembler.assemble(resource);
  }
}
