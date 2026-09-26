import type { Player } from "@/models";
import type { PlayerDTO } from "@/events/dto/player.dto";

export class PlayerSyncer {
  public static sync(previousPlayer: Player, newPlayerDto: PlayerDTO): void {
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
