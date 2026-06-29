import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { DigimonRepository } from "@/repositories/digimon.repository";
import type { DigievolutionTreeRaw } from "@/repositories/tables/raws/digievolution/digievolution-tree.raw";
import type { FamilyViewModel } from "@/viewmodels/digievolution/family.viewmodel";
import type { NodeViewModel } from "@/viewmodels/digievolution/node.viewmodel";
import type { DigievolutionsTreeViewModel } from "@/viewmodels/digievolution/digievolution-tree.viewmodel";

export class TreePresenter {
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
    ): FamilyViewModel {
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
}
