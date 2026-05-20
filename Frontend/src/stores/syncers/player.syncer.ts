import type { Player } from '../../models/Player';
import type * as Events from '../../events/events.map';

export class PlayerSyncer {
    public static sync(player: Player, dto: Events.PlayerDTO): void {
        if (dto.name !== undefined) {
            player.name = dto.name;
        }
        if (dto.bits !== undefined) {
            player.bits = dto.bits;
        }
        if (dto.location !== undefined) {
            player.location = dto.location;
        }
    }
}
