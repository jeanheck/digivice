import type { ComposerTranslation } from "vue-i18n";
import type { Attributes, Digimon, Resistances } from "@/models";
import { DigievolutionRequirementConstant } from "@/constants/digievolution-requirement.constant";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { NodeViewModel } from "@/viewmodels/digievolution/node.viewmodel";
import type { RequirementViewModel } from "@/viewmodels/digimon/requirement.viewmodel";

export class NodePresenter {
    public static getRequirementText(
        digimonName: string,
        requirement: RequirementViewModel,
        translate: ComposerTranslation
    ): string {
        const levelLabel = translate("digievolution.lv");

        switch (requirement.type) {
            case DigievolutionRequirementConstant.DigimonLevel:
                return `${digimonName} ${levelLabel} ${requirement.value}`;
            case DigievolutionRequirementConstant.Attribute:
            case DigievolutionRequirementConstant.Resistance:
                return `${digimonName}: ${this.capitalize(requirement.stat!)} >= ${requirement.value}`;
            case DigievolutionRequirementConstant.DigievolutionLevel: {
                if (requirement.digievolution === undefined) {
                    return translate("digievolution.unknownParam");
                }

                const digievolutionName = DigievolutionRepository.getNameById(requirement.digievolution);
                return `${digievolutionName} ${levelLabel}.${requirement.value}`;
            }
            default:
                return translate("digievolution.unknownParam");
        }
    }

    public static isRequirementMet(
        digimon: Digimon,
        requirement: RequirementViewModel
    ): boolean {
        switch (requirement.type) {
            case DigievolutionRequirementConstant.DigimonLevel:
                return digimon.level >= requirement.value;
            case DigievolutionRequirementConstant.Attribute: {
                const attribute = digimon.attributes[requirement.stat?.toLowerCase() as keyof Attributes];
                return attribute >= requirement.value;
            }
            case DigievolutionRequirementConstant.Resistance: {
                const resistance = digimon.resistances[requirement.stat?.toLowerCase() as keyof Resistances];
                return resistance >= requirement.value;
            }
            case DigievolutionRequirementConstant.DigievolutionLevel: {
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

    private static capitalize(value: string): string {
        return value.charAt(0).toUpperCase() + value.slice(1);
    }
}
