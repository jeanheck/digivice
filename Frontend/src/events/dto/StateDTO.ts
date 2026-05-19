import type { PlayerDTO } from './PlayerDTO';
import type { PartyDTO } from './PartyDTO';
import type { JournalDTO } from './JournalDTO';

export interface StateDTO {
    player: PlayerDTO | null;
    party: PartyDTO | null;
    journal: JournalDTO | null;
}
