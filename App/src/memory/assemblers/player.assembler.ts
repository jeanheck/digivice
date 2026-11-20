import type { Player } from "@/state/models";
import type { PlayerResource } from "@/memory/resources/player.resource";

export class PlayerAssembler {
  private static toMapId(value: number): string {
    return (value & 0xffff).toString(16).toUpperCase().padStart(4, "0");
  }

  private static toNullWhenZero(value: number): number | null {
    if (value === 0) {
      return null;
    }

    return value;
  }

  public static assemble(resource: PlayerResource): Player {
    return {
      bits: resource.bits,
      mapId: PlayerAssembler.toMapId(resource.mapId),
      previousMapId: PlayerAssembler.toMapId(resource.previousMapId),
      seabedRoute: PlayerAssembler.toNullWhenZero(resource.seabedRoute),
      mapVariant: PlayerAssembler.toNullWhenZero(resource.mapVariant),
    };
  }
}
