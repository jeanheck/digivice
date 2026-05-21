import type { PlayerDTO } from './player.dto';
import type { PartyDTO } from './party.dto';
import type { JournalDTO } from './journal.dto';

export interface StateDTO {
    player: Required<PlayerDTO> | null;
    party: Required<PartyDTO> | null;
    journal: Required<JournalDTO> | null;
}
