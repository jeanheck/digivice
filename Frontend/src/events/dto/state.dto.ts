import type { PlayerDTO } from './player.dto';
import type { PartyDTO } from './party.dto';
import type { JournalDTO } from './journal.dto';

export interface StateDTO {
    player: PlayerDTO | null;
    party: PartyDTO | null;
    journal: JournalDTO | null;
}
