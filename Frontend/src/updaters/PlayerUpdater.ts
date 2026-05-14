import type { State } from '../models/State';
import type * as Events from '../dtos/events.dto';

export class PlayerUpdater {
    public static updateBits(state: State | null, event: Events.PlayerBitsChangedDTO) {
        if (state?.player) {
            state.player.bits = event.bits;
        }
    }

    public static updateName(state: State | null, event: Events.PlayerNameChangedDTO) {
        if (state?.player) {
            state.player.name = event.name;
        }
    }

    public static updateLocation(state: State | null, event: Events.PlayerLocationChangedDTO) {
        if (state?.player) {
            state.player.mapId = event.location;
        }
    }
}
