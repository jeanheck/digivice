import type { Player } from "../../models";
import type * as Events from "../../events/events.map";

export class PlayerSyncer {
    public static sync(previousPlayer: Player, newPlayerDto: Events.PlayerDTO): void {
        if (newPlayerDto.bits !== undefined) {
            previousPlayer.bits = newPlayerDto.bits;
        }
        if (newPlayerDto.location !== undefined) {
            previousPlayer.location = newPlayerDto.location;
        }
        if (newPlayerDto.previousMapId !== undefined) {
            previousPlayer.previousMapId = newPlayerDto.previousMapId;
        }
        if (newPlayerDto.seabedRoute !== undefined) {
            previousPlayer.seabedRoute = newPlayerDto.seabedRoute;
        }
        if (newPlayerDto.mapVariant !== undefined) {
            previousPlayer.mapVariant = newPlayerDto.mapVariant;
        }
    }
}
