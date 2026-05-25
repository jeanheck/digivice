import { DigievolutionRepository } from "@/repositories/digievolution-repository";
import type { DigievolutionRaw } from "@/repositories/tables/raws/digievolution/digievolution-raw";

export class DigimonAttributesResistancesPresenter {
    public static getDigievolutionById(digievolutionId: number): DigievolutionRaw {
        return DigievolutionRepository.getRawDigievolutionById(digievolutionId);
    }
}
