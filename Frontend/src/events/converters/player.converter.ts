import type { PlayerDTO } from "../dto/player.dto";
import type { Player } from "../../models";

export class PlayerConverter {
    public static convert(playerDto: Required<PlayerDTO>): Player {
        return {
            bits: playerDto.bits,
            location: playerDto.location,
            previousMapId: playerDto.previousMapId,
            seabedRoute: playerDto.seabedRoute,
            mapVariant: playerDto.mapVariant
        };
    }
}
