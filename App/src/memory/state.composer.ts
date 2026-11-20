import type { State } from "@/state/models";
import { AuctionsProvider } from "@/memory/providers/auctions.provider";
import { CardBattleProvider } from "@/memory/providers/card-battle.provider";
import { DigimonBattleProvider } from "@/memory/providers/digimon-battle.provider";
import { ImportantItemsProvider } from "@/memory/providers/important-items.provider";
import { JournalProvider } from "@/memory/providers/journal.provider";
import { NpcsProvider } from "@/memory/providers/npcs.provider";
import { PartyProvider } from "@/memory/providers/party.provider";
import { PlayerProvider } from "@/memory/providers/player.provider";
import type { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { SaveLoadedValidator } from "@/memory/validators/save-loaded.validator";
import { SaveNotLoadedError } from "@/memory/validators/save-not-loaded.error";

export class StateComposer {
  public static compose(snapshot: MemorySnapshot): State {
    const state: State = {
      player: PlayerProvider.get(snapshot),
      importantItems: ImportantItemsProvider.get(snapshot),
      party: PartyProvider.get(snapshot),
      digimonBattle: DigimonBattleProvider.get(snapshot),
      cardBattle: CardBattleProvider.get(snapshot),
      auctions: AuctionsProvider.get(snapshot),
      npcs: NpcsProvider.get(snapshot),
      journal: JournalProvider.get(snapshot),
    };

    const violation = SaveLoadedValidator.findViolation(state.party);
    if (violation !== null) {
      throw new SaveNotLoadedError(violation);
    }

    return state;
  }
}
