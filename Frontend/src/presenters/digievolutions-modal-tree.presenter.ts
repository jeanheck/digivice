import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { DigimonRepository } from "@/repositories/digimon.repository";
import type { DigievolutionTreeRaw } from "@/repositories/tables/raws/digievolution/digievolution-tree.raw";
import type { Attributes, Digimon } from "@/models";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type { DigievolutionTreeFamilyViewModel } from "@/viewmodels/digievolution/digievolution-tree-family.viewmodel";
import type { DigievolutionTreeNodeViewModel } from "@/viewmodels/digievolution/digievolution-tree-node.viewmodel";
import type { DigievolutionsTreeViewModel } from "@/viewmodels/digievolution/digievolution-tree.viewmodel";

export class DigievolutionsModalTreePresenter {
    public static getDigievolutionsTreeViewModel(digimonId: number, digimonName: string): DigievolutionsTreeViewModel {
        const digievolutionTreeTable = DigievolutionRepository.getDigievolutionTree();
        const sortedFamilyKeys = DigievolutionsModalTreePresenter.sortFamilyKeys(
            Object.keys(digievolutionTreeTable),
            digimonName
        );

        const families = sortedFamilyKeys.map((familyKey) => {
            const digievolutionTreeRaw = digievolutionTreeTable[familyKey]!;
            const familyNodes = digievolutionTreeRaw.map((node) => {
                return DigievolutionsModalTreePresenter.buildDigievolutionTreeNodeViewModel(digimonId, node);
            });

            return DigievolutionsModalTreePresenter.buildFamilyViewModel(familyKey, familyNodes);
        });

        return { families };
    }

    public static checkRequirements(
        digimon: Digimon,
        node: { requirements: DigimonDigievolutionRequirementViewModel[] }
    ): boolean {
        if (node.requirements.length === 0) {
            return true;
        }

        for (const requirement of node.requirements) {
            switch (requirement.type) {
                case "DigimonLevel":
                    if (digimon.level < requirement.value) {
                        return false;
                    }
                    break;
                case "Attribute": {
                    const attribute = digimon.attributes[requirement.attribute?.toLowerCase() as keyof Attributes];
                    if (attribute < requirement.value) {
                        return false;
                    }
                    break;
                }
                case "DigievolutionLevel":
                    return false;
            }
        }

        return true;
    }

    private static sortFamilyKeys(familyKeys: string[], digimonName: string): string[] {
        const otherFamilyKeys = familyKeys.filter((familyKey) => {
            return familyKey !== digimonName;
        });

        otherFamilyKeys.sort((firstFamilyKey, secondFamilyKey) => {
            return firstFamilyKey.localeCompare(secondFamilyKey);
        });

        return [digimonName, ...otherFamilyKeys];
    }

    private static buildFamilyViewModel(
        familyKey: string,
        familyNodes: DigievolutionTreeNodeViewModel[]
    ): DigievolutionTreeFamilyViewModel {
        const nodesByName = DigievolutionsModalTreePresenter.indexNodesByName(familyNodes);
        const headNodes = familyNodes.filter((node) => {
            return node.before === null;
        });

        if (headNodes.length === 0) {
            return {
                familyKey,
                nodesBeforeFork: [],
                branchs: [],
            };
        }

        const headNode = headNodes[0]!;
        const trunkNodes = DigievolutionsModalTreePresenter.buildLinearNodes(headNode.name, nodesByName);
        const forkPoint = DigievolutionsModalTreePresenter.findForkPoint(trunkNodes);

        if (forkPoint) {
            const nodesBeforeFork = trunkNodes.slice(0, forkPoint.branchIndex + 1);
            const branchs = forkPoint.branchStartNames.map((branchStartName) => {
                return DigievolutionsModalTreePresenter.buildLinearNodes(branchStartName, nodesByName);
            });

            return {
                familyKey,
                nodesBeforeFork,
                branchs,
            };
        }

        return {
            familyKey,
            nodesBeforeFork: [],
            branchs: [trunkNodes],
        };
    }

    private static indexNodesByName(
        familyNodes: DigievolutionTreeNodeViewModel[]
    ): Record<string, DigievolutionTreeNodeViewModel> {
        const nodesByName: Record<string, DigievolutionTreeNodeViewModel> = {};

        for (const node of familyNodes) {
            nodesByName[node.name] = node;
        }

        return nodesByName;
    }

    private static buildLinearNodes(
        startNodeName: string,
        nodesByName: Record<string, DigievolutionTreeNodeViewModel>
    ): DigievolutionTreeNodeViewModel[] {
        const linearNodes: DigievolutionTreeNodeViewModel[] = [];
        let currentNodeName: string | null = startNodeName;

        while (currentNodeName) {
            const currentNode = nodesByName[currentNodeName];

            if (!currentNode) {
                break;
            }

            linearNodes.push(currentNode);

            if (typeof currentNode.next === "string") {
                currentNodeName = currentNode.next;
            } else {
                currentNodeName = null;
            }
        }

        return linearNodes;
    }

    private static findForkPoint(
        trunkNodes: DigievolutionTreeNodeViewModel[]
    ): { branchIndex: number; branchStartNames: string[] } | null {
        for (let nodeIndex = 0; nodeIndex < trunkNodes.length; nodeIndex++) {
            const node = trunkNodes[nodeIndex];

            if (node && Array.isArray(node.next)) {
                return {
                    branchIndex: nodeIndex,
                    branchStartNames: node.next,
                };
            }
        }

        return null;
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
