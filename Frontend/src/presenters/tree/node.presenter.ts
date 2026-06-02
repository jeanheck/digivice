import type { Attributes, Digimon } from "@/models";
import type { RequirementViewModel } from "@/viewmodels/digimon/requirement.viewmodel";

export class NodePresenter {
    public static isRequirementMet(
        digimon: Digimon,
        requirement: RequirementViewModel
    ): boolean {
        switch (requirement.type) {
            case "DigimonLevel":
                return digimon.level >= requirement.value;
            case "Attribute": {
                const attribute = digimon.attributes[requirement.attribute?.toLowerCase() as keyof Attributes];
                return attribute >= requirement.value;
            }
            case "DigievolutionLevel": // TODO - Check how we will read this information on backend first
                return false;
            default:
                return false;
        }
    }

    public static checkRequirements(
        digimon: Digimon,
        node: { requirements: RequirementViewModel[] }
    ): boolean {
        if (node.requirements.length === 0) {
            return true;
        }

        for (const requirement of node.requirements) {
            if (!this.isRequirementMet(digimon, requirement)) {
                return false;
            }
        }

        return true;
    }
}
