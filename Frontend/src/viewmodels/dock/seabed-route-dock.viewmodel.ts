import type {
  SeabedRouteDockLabelPlacement,
  SeabedRouteDockType,
} from "@/repositories/tables/raws/dock/seabed-route-dock.raw";

export interface SeabedRouteDockViewModel {
  location: string;
  x: number;
  y: number;
  type: SeabedRouteDockType;
  labelPlacement: SeabedRouteDockLabelPlacement;
}
