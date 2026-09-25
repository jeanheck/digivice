import type { DeepRequired } from "@/events/dto/deep-required";
import type { PlayerDTO } from "@/events/dto/player.dto";
import type { Player } from "@/models";

export class PlayerConverter {
  public static convert(playerDto: DeepRequired<PlayerDTO>): Player {
    return {
      bits: playerDto.bits,
      mapId: playerDto.mapId,
      previousMapId: playerDto.previousMapId,
      seabedRoute: playerDto.seabedRoute,
      mapVariant: playerDto.mapVariant,
    };
  }
}
