import type { PlayerDTO } from '../dto/player.dto';
import type { Player } from '../../models/Player';

export class PlayerConverter {
    public static convert(player: PlayerDTO | null): Player | null {
        if (!player) return null;
        return {
            name: player.name ?? '',
            bits: player.bits ?? 0,
            location: player.location ?? ''
        };
    }
}
