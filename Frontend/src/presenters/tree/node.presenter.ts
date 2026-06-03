import type { Attributes, Digimon } from "@/models";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { NodeViewModel } from "@/viewmodels/digievolution/node.viewmodel";
import type { RequirementViewModel } from "@/viewmodels/digimon/requirement.viewmodel";

export class NodePresenter {
    public static getRequirementText(
        digimonName: string,
        requirement: RequirementViewModel
    ): string {
        switch (requirement.type) {
            case "DigimonLevel":
                return `${digimonName} Lv ${requirement.value}`;
            case "Attribute":
                return `${digimonName} - ${requirement.attribute} >= ${requirement.value}`;
            case "DigievolutionLevel": {
                if (requirement.digievolution === undefined) {
                    return "Unknown Parameter";
                }

                const digievolutionName = DigievolutionRepository.getNameById(requirement.digievolution);
                return `${digievolutionName} Lv ${requirement.value}`;
            }
            default:
                return "Unknown Parameter";
        }
    }

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
            case "DigievolutionLevel": {
                if (requirement.digievolution === undefined) {
                    return false;
                }

                const storedDigievolution = digimon.storedDigievolutions.find(
                    (stored) => stored.digievolutionId === requirement.digievolution
                );

                if (!storedDigievolution) {
                    return false;
                }

                return storedDigievolution.level >= requirement.value;
            }
            default:
                return false;
        }
    }

    public static checkRequirements(
        digimon: Digimon,
        node: NodeViewModel
    ): boolean {
        if(node.id === 260){
            console.log("node do stingmon ", node)
        }

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
