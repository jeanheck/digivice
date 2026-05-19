import type * as DTO from '../dtos/events.dto';
import type { Player } from '../models/Player';

export class PlayerConverter {
    public static convert(player: DTO.PlayerDTO | null): Player | null {
        if (!player) return null;
        return {
            name: player.name ?? '',
            bits: player.bits ?? 0,
            mapId: player.location ?? player.mapId ?? '' // Converte location ou mapId para mapId
        };
    }
}
