import type { Player } from "@/models";
import type * as Events from "@/events/events.map";

export class PlayerSyncer {
  public static sync(previousPlayer: Player, newPlayerDto: Events.PlayerDTO): void {
    if (newPlayerDto.bits !== undefined) {
      previousPlayer.bits = newPlayerDto.bits;
    }
    if (newPlayerDto.mapId !== undefined) {
      previousPlayer.mapId = newPlayerDto.mapId;
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
