import type { Player } from '../../models/Player';
import type * as Events from '../../events/events.map';

export class PlayerSyncer {
    public static sync(previousPlayer: Player, newPlayerDto: Events.PlayerDTO): void {
        if (newPlayerDto.name !== undefined) {
            previousPlayer.name = newPlayerDto.name;
        }
        if (newPlayerDto.bits !== undefined) {
            previousPlayer.bits = newPlayerDto.bits;
        }
        if (newPlayerDto.location !== undefined) {
            previousPlayer.location = newPlayerDto.location;
        }
    }
}
