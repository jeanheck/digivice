import DigievolutionJson from "@/database/digievolution/digievolution.json";
import DigievolutionNameJson from "@/database/digievolution/digievolution-name.json";
import DigievolutionTechniquesJson from "@/database/digievolution/digievolution-technique.json";
import type { DigievolutionTable } from "@/repositories/tables/digievolution/digievolution-table";
import type { DigievolutionNameTable } from "@/repositories/tables/digievolution/digievolution-name-table";
import type { DigievolutionTechniqueTable } from "@/repositories/tables/digievolution/digievolution-technique-table";
import type { DigievolutionRaw } from "./tables/raws/digievolution/digievolution-raw";
import type { DigievolutionTechniqueRaw } from "./tables/raws/digievolution/digievolution-technique-raw";

export class DigievolutionRepository {
    private static readonly digievolutionTable = DigievolutionJson as DigievolutionTable;
    private static readonly digievolutionNameTable = DigievolutionNameJson as DigievolutionNameTable;
    private static readonly digievolutionTechniqueTable = DigievolutionTechniquesJson as DigievolutionTechniqueTable;

    public static getNameById(id: number): string {
        return this.digievolutionNameTable[id]!;
    }
    public static getIdByName(name: string): number {
        const entry = Object.entries(this.digievolutionNameTable).find(([, digievolutionName]) => digievolutionName === name);

        return Number(entry![0]);
    }
    public static getRawDigievolutionById(id: number): DigievolutionRaw {
        return this.digievolutionTable[id]!;
    }
    public static getRawDigievolutionTechniquesById(id: number): DigievolutionTechniqueRaw[] {
        return this.digievolutionTechniqueTable[id]!;
    }
    public static getRawDigievolutionTechniquesByName(digievolutionName: string): DigievolutionTechniqueRaw[] {
        const digievolutionId = this.getIdByName(digievolutionName);
        return this.digievolutionTechniqueTable[digievolutionId]!;
    }
}
