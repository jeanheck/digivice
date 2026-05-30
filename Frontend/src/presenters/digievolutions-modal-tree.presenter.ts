import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { DigimonRepository } from "@/repositories/digimon.repository";
import type { DigievolutionTreeRaw } from "@/repositories/tables/raws/digievolution/digievolution-tree.raw";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type { DigievolutionTreeNodeViewModel } from "@/viewmodels/digievolution/digievolution-tree-node.viewmodel";
import type { DigievolutionsTreeViewModel } from "@/viewmodels/digievolution/digievolution-tree.viewmodel";

export class DigievolutionsModalTreePresenter {
    public static getDigievolutionsTreeViewModel(digimonId: number, rookieFamilyKey: string): DigievolutionsTreeViewModel {
        const digievolutionTreeTable = DigievolutionRepository.getDigievolutionTree();
        const sortedFamilyKeys = DigievolutionsModalTreePresenter.sortFamilyKeys(
            Object.keys(digievolutionTreeTable),
            rookieFamilyKey
        );

        const digievolutionsTreeViewModel: DigievolutionsTreeViewModel = {};

        for (const familyKey of sortedFamilyKeys) {
            const digievolutionsTreeRaw = digievolutionTreeTable[familyKey]!;
            digievolutionsTreeViewModel[familyKey] = digievolutionsTreeRaw.map((node) => {
                return DigievolutionsModalTreePresenter.buildDigievolutionTreeNodeViewModel(digimonId, node);
            });
        }

        return digievolutionsTreeViewModel;
    }

    private static sortFamilyKeys(familyKeys: string[], rookieFamilyKey: string): string[] {
        return [...familyKeys].sort((firstFamilyKey, secondFamilyKey) => {
            if (firstFamilyKey === rookieFamilyKey) {
                return -1;
            }

            if (secondFamilyKey === rookieFamilyKey) {
                return 1;
            }

            return firstFamilyKey.localeCompare(secondFamilyKey);
        });
    }

    private static buildDigievolutionTreeNodeViewModel(
        digimonId: number, 
        digievolutionTreeRaw: DigievolutionTreeRaw 
    ): DigievolutionTreeNodeViewModel {
        return {
            name: digievolutionTreeRaw.name,
            before: digievolutionTreeRaw.before,
            next: digievolutionTreeRaw.next,
            requirements: DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionTreeRaw.name),
        };
    }
}
