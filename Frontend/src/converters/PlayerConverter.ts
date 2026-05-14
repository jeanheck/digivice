import type * as DTO from '../dtos/events.dto';
import type { Player } from '../models/Player';

export class PlayerConverter {
    public static convert(player: DTO.PlayerDTO | null): Player | null {
        if (!player) return null;
        return { ...player };
    }
}
