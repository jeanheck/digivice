import type { SeabedDirectionRaw } from "@/repositories/tables/raws/seabed/seabed-direction.raw";
import type { SeabedDirectionDockViewModel } from "@/viewmodels/seabed-modal/seabed-route-dock.viewmodel";
import type { SeabedDirectionRouteSegmentViewModel } from "@/viewmodels/seabed-modal/seabed-direction-route-segment.viewmodel";
import type { SeabedDirectionViewModel } from "@/viewmodels/seabed-modal/seabed-direction.viewmodel";

export class SeabedDirectionConverter {
  public static convert(routeId: string, seabedDirectionRaw: SeabedDirectionRaw): SeabedDirectionViewModel {
    const docks: SeabedDirectionDockViewModel[] = seabedDirectionRaw.docks.map((dockRaw) => {
      return {
        location: dockRaw.location,
        x: dockRaw.x,
        y: dockRaw.y,
        type: dockRaw.type,
        labelPlacement: dockRaw.labelPlacement,
      };
    });

    return {
      id: routeId,
      docks,
      segments: SeabedDirectionConverter.buildSegments(docks),
    };
  }

  private static buildSegments(
    docks: SeabedDirectionDockViewModel[]
  ): SeabedDirectionRouteSegmentViewModel[] {
    const segments: SeabedDirectionRouteSegmentViewModel[] = [];

    for (let dockIndex = 0; dockIndex < docks.length - 1; dockIndex++) {
      const fromDock = docks[dockIndex];
      const toDock = docks[dockIndex + 1];

      if (fromDock === undefined || toDock === undefined) {
        continue;
      }

      segments.push({
        from: fromDock,
        to: toDock,
      });
    }

    return segments;
  }
}
