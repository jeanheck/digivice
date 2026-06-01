import type { StatKey } from "@/constants/stat/stat-key";

export interface EquipmentsAttributes {
    attribute: StatKey;
    type: string;
    value: number;
}
