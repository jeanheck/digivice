import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { DigimonRepository } from "@/repositories/digimon.repository";
import type { DigievolutionTreeRaw } from "@/repositories/tables/raws/digievolution/digievolution-tree.raw";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type {
    DigievolutionsTreeFamilyNodeViewModel,
    DigievolutionsTreeFamilyViewModel,
    DigievolutionsTreeViewModel,
} from "@/viewmodels/digievolution/digievolution-tree.viewmodel";

export class DigievolutionsModalTreePresenter {
    public static getTreeViewModel(digimonId: number, rookieFamilyKey: string): DigievolutionsTreeViewModel {
        const digievolutionTreeTable = DigievolutionRepository.getDigievolutionTree();
        const familyKeys = Object.keys(digievolutionTreeTable);

        const families = familyKeys.map((familyKey) => {
            return DigievolutionsModalTreePresenter.buildFamilyViewModel(
                digimonId,
                familyKey,
                digievolutionTreeTable[familyKey]!
            );
        });

        families.sort((firstFamily, secondFamily) => {
            if (firstFamily.familyKey === rookieFamilyKey) {
                return -1;
            }

            if (secondFamily.familyKey === rookieFamilyKey) {
                return 1;
            }

            return firstFamily.familyKey.localeCompare(secondFamily.familyKey);
        });

        return { families };
    }

    private static buildFamilyViewModel(
        digimonId: number,
        familyKey: string,
        familyNodes: DigievolutionTreeRaw[]
    ): DigievolutionsTreeFamilyViewModel {
        const nodesByName = DigievolutionsModalTreePresenter.indexNodesByName(familyNodes);
        const headNodes = familyNodes.filter((node) => {
            return node.before === null;
        });

        if (headNodes.length === 0) {
            return {
                familyKey,
                sharedPrefixNodes: [],
                branchPaths: [],
            };
        }

        const headNode = headNodes[0]!;
        const trunkNodeNames = DigievolutionsModalTreePresenter.buildLinearNodeNames(headNode.name, nodesByName);
        const forkPoint = DigievolutionsModalTreePresenter.findForkPoint(trunkNodeNames, nodesByName);

        if (forkPoint) {
            const sharedPrefixNodeNames = trunkNodeNames.slice(0, forkPoint.branchIndex + 1);
            const branchPaths = forkPoint.branchStartNames.map((branchStartName) => {
                const branchNodeNames = DigievolutionsModalTreePresenter.buildLinearNodeNames(branchStartName, nodesByName);
                return DigievolutionsModalTreePresenter.buildNodeViewModels(digimonId, branchNodeNames);
            });

            return {
                familyKey,
                sharedPrefixNodes: DigievolutionsModalTreePresenter.buildNodeViewModels(digimonId, sharedPrefixNodeNames),
                branchPaths,
            };
        }

        return {
            familyKey,
            sharedPrefixNodes: [],
            branchPaths: [
                DigievolutionsModalTreePresenter.buildNodeViewModels(digimonId, trunkNodeNames),
            ],
        };
    }

    private static indexNodesByName(familyNodes: DigievolutionTreeRaw[]): Record<string, DigievolutionTreeRaw> {
        const nodesByName: Record<string, DigievolutionTreeRaw> = {};

        for (const node of familyNodes) {
            nodesByName[node.name] = node;
        }

        return nodesByName;
    }

    private static buildLinearNodeNames(
        startNodeName: string,
        nodesByName: Record<string, DigievolutionTreeRaw>
    ): string[] {
        const nodeNames: string[] = [];
        let currentNodeName: string | null = startNodeName;

        while (currentNodeName) {
            const currentNode = nodesByName[currentNodeName];

            if (!currentNode) {
                break;
            }

            nodeNames.push(currentNodeName);

            if (typeof currentNode.next === "string") {
                currentNodeName = currentNode.next;
            } else {
                currentNodeName = null;
            }
        }

        return nodeNames;
    }

    private static findForkPoint(
        trunkNodeNames: string[],
        nodesByName: Record<string, DigievolutionTreeRaw>
    ): { branchIndex: number; branchStartNames: string[] } | null {
        for (let nodeIndex = 0; nodeIndex < trunkNodeNames.length; nodeIndex++) {
            const nodeName = trunkNodeNames[nodeIndex];

            if (!nodeName) {
                continue;
            }

            const node = nodesByName[nodeName];

            if (node && Array.isArray(node.next)) {
                return {
                    branchIndex: nodeIndex,
                    branchStartNames: node.next,
                };
            }
        }

        return null;
    }

    private static buildNodeViewModels(
        digimonId: number,
        nodeNames: string[]
    ): DigievolutionsTreeFamilyNodeViewModel[] {
        return nodeNames.map((nodeName) => {
            return DigievolutionsModalTreePresenter.buildNodeViewModel(digimonId, nodeName);
        });
    }

    private static buildNodeViewModel(
        digimonId: number,
        nodeName: string
    ): DigievolutionsTreeFamilyNodeViewModel {
        return {
            name: nodeName,
            requirements: DigievolutionsModalTreePresenter.getNodeRequirements(digimonId, nodeName),
        };
    }

    private static getNodeRequirements(
        digimonId: number,
        digievolutionName: string
    ): DigimonDigievolutionRequirementViewModel[] {
        const digimonDigievolutions = DigimonRepository.getDigievolutionsById(digimonId);

        if (!digimonDigievolutions[digievolutionName]) {
            return [];
        }

        return DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionName);
    }
}
