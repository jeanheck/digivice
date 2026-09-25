import type { InnerLocationRaw } from "@/repositories/tables/raws/location/inner-location.raw";
import type { CoordinatesRaw } from "./coordinates.raw";
import type { RequisiteRaw } from "./requisite.raw";

export interface StepRaw {
  requisites: RequisiteRaw[];
  location: string;
  coordinates: CoordinatesRaw;
  innerLocation?: InnerLocationRaw[];
}
