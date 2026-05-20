import type { PlayerDTO } from '../dto/player.dto';
import type { Player } from '../../models/Player';

export class PlayerConverter {
    public static convert(player: Required<PlayerDTO>): Player {
        return {
            name: player.name,
            bits: player.bits,
            location: player.location
        };
    }
}
