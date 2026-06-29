import type { PlayerDTO } from '../dto/player.dto';
import type { Player } from '../../models';

export class PlayerConverter {
    public static convert(playerDto: Required<PlayerDTO>): Player {
        return {
            name: playerDto.name,
            bits: playerDto.bits,
            location: playerDto.location
        };
    }
}
