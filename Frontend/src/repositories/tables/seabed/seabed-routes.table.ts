import type { SeabedRouteLocationRaw } from "../raws/seabed/seabed-route-location.raw";

export type SeabedRoutesTable = Record<string, Record<string, SeabedRouteLocationRaw>>;
