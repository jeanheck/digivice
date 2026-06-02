import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { DigimonRepository } from "@/repositories/digimon.repository";
import type { DigievolutionTreeRaw } from "@/repositories/tables/raws/digievolution/digievolution-tree.raw";
import type { Attributes, Digimon } from "@/models";
import type { RequirementViewModel } from "@/viewmodels/digimon/requirement.viewmodel";
import type { DigievolutionTreeFamilyViewModel } from "@/viewmodels/digievolution/digievolution-tree-family.viewmodel";
import type { NodeViewModel } from "@/viewmodels/digievolution/node.viewmodel";
import type { DigievolutionsTreeViewModel } from "@/viewmodels/digievolution/digievolution-tree.viewmodel";

export class DigievolutionsModalTreePresenter {
    public static getDigievolutionsTree(digimonId: number): DigievolutionsTreeViewModel {
        const digievolutionTreeTable = DigievolutionRepository.getDigievolutionTree();
        const sortedFamilyKeys = this.sortFamilyKeys(
            Object.keys(digievolutionTreeTable),
            digimonId
        );

        const digievolutionTreeFamiliesViewModel = sortedFamilyKeys.map((key) => {
            const digievolutionTreeRaws = digievolutionTreeTable[key]!;
            const digievolutionTreeFamilyNodes = digievolutionTreeRaws.map((node) => {
                return this.buildDigievolutionTreeNode(digimonId, node);
            });

            return this.buildDigievolutionTreeFamily(key, digievolutionTreeFamilyNodes);
        });

        return { families: digievolutionTreeFamiliesViewModel };
    }

    private static sortFamilyKeys(familyKeys: string[], digimonId: number): string[] {
        const currentFamilyKey = String(digimonId);

        const otherFamilyKeys = familyKeys.filter((familyKey) => {
            return familyKey !== currentFamilyKey;
        });

        otherFamilyKeys.sort((firstFamilyKey, secondFamilyKey) => {
            return Number(firstFamilyKey) - Number(secondFamilyKey);
        });

        return [currentFamilyKey, ...otherFamilyKeys];
    }

    private static buildDigievolutionTreeFamily(
        familyKey: string,
        familyNodes: NodeViewModel[]
    ): DigievolutionTreeFamilyViewModel {
        if (!this.familyHasFork(familyNodes)) {
            return {
                key: familyKey,
                nodesBeforeFork: [],
                branchs: [familyNodes],
            };
        }

        const forkNode = this.findForkNode(familyNodes);
        const forkNodeIndex = familyNodes.indexOf(forkNode);
        const branchStartNodeIds = forkNode.next as number[];
        const branchs = branchStartNodeIds.map((branchStartNodeId) => {
            return this.buildBranch(branchStartNodeId, familyNodes);
        });

        return {
            key: familyKey,
            nodesBeforeFork: familyNodes.slice(0, forkNodeIndex + 1),
            branchs,
        };
    }

    private static familyHasFork(familyNodes: NodeViewModel[]): boolean {
        return familyNodes.some((node) => Array.isArray(node.next));
    }

    private static findForkNode(familyNodes: NodeViewModel[]): NodeViewModel {
        return familyNodes.find((node) => Array.isArray(node.next))!;
    }

    private static buildBranch(
        startNodeId: number,
        familyNodes: NodeViewModel[]
    ): NodeViewModel[] {
        const branch: NodeViewModel[] = [];
        let nextNodeId: number | null = startNodeId;

        do {
            const currentNode: NodeViewModel = familyNodes.find((node) => nextNodeId === node.id)!;
            branch.push(currentNode);
            nextNodeId = currentNode.next as number | null;
        } while (nextNodeId);

        return branch;
    }

    private static buildDigievolutionTreeNode(
        digimonId: number,
        digievolutionTreeRaw: DigievolutionTreeRaw
    ): NodeViewModel {
        const digievolutionId = Number(digievolutionTreeRaw.id);

        return {
            id: digievolutionId,
            name: DigievolutionRepository.getNameById(digievolutionId),
            next: this.parseNext(digievolutionTreeRaw.next),
            requirements: DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionId),
        };
    }

    private static parseNext(next: string | string[] | null): number | number[] | null {
        if (next === null) {
            return null;
        }

        if (Array.isArray(next)) {
            return next.map((digievolutionId) => Number(digievolutionId));
        }

        return Number(next);
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
