import DigivolvingRequirementsTable from '../data/static/DigivolvingRequirementsTable.json'
import DigievolutionTreeData from '../data/static/DigievolutionTree.json'

// --- Types for family chain system ---
interface TreeEntry {
    name: string
    family: string
    before: string | null
    next: string | string[] | null
}

export interface FamilyBranch {
    names: string[]  // ordered list of evolution names in this branch (suffix only for branching families)
}

export interface FamilyChain {
    family: string
    sharedPrefix: string[]      // nodes shared before the branch point (empty for non-branching)
    branches: FamilyBranch[]    // most families have 1 branch with all nodes; branching ones have diverging suffixes
}

export type RequirementType = 'DigimonLevel' | 'DigievolutionLevel' | 'Attribute'

export interface EvolutionRequirement {
    Type: RequirementType
    Value: number
    Digievolution?: string
    Digimon?: string
    Attribute?: string
}

export interface GraphNode {
    id: string
    name: string
    requirements: EvolutionRequirement[]
    level: number
    x: number
    y: number
}

export interface GraphLink {
    source: string
    target: string
}

export interface DigievolutionGraph {
    nodes: GraphNode[]
    links: GraphLink[]
}

export class EvolutionGraph {
    /**
     * Builds ordered family chains from DigievolutionTree.json.
     * Each family becomes a FamilyChain with one or more branches.
     * The rookieFamily is placed first, then others alphabetically.
     */
    static buildFamilyChains(rookieFamily: string): FamilyChain[] {
        const entries = (DigievolutionTreeData as { digievolutions: TreeEntry[] }).digievolutions
        
        // Group entries by family
        const familyMap: Record<string, TreeEntry[]> = {}
        for (const entry of entries) {
            if (!familyMap[entry.family]) familyMap[entry.family] = []
            familyMap[entry.family]!.push(entry)
        }

        // Build chains for each family
        const chains: FamilyChain[] = []
        for (const family in familyMap) {
            const members = familyMap[family] || []
            const byName: Record<string, TreeEntry> = {}
            for (const m of members) byName[m.name] = m

            const heads = members.filter(m => m.before === null)

            const buildBranch = (startName: string): string[] => {
                const branch: string[] = []
                let current: string | null = startName
                while (current) {
                    const e: TreeEntry | undefined = byName[current]
                    if (!e) break
                    branch.push(current)
                    if (typeof e.next === 'string') {
                        current = e.next
                    } else {
                        current = null
                    }
                }
                return branch
            }

            const branches: FamilyBranch[] = []

            for (const head of heads) {
                // Build the trunk from head
                const trunk = buildBranch(head.name)
                
                // Check if the last node in trunk has array next (branching)
                // Actually, check EVERY node for array next
                let branchPoint: TreeEntry | null = null
                let branchIndex = -1
                for (let i = 0; i < trunk.length; i++) {
                    const trunkName = trunk[i]
                    if (!trunkName) continue
                    const e = byName[trunkName]
                    if (e && Array.isArray(e.next)) {
                        branchPoint = e
                        branchIndex = i
                        break
                    }
                }

                if (branchPoint && Array.isArray(branchPoint.next)) {
                    // Separate shared prefix from branch-specific suffixes
                    const prefix = trunk.slice(0, branchIndex + 1)
                    for (const branchStart of branchPoint.next) {
                        const suffix = buildBranch(branchStart)
                        branches.push({ names: suffix })
                    }
                    chains.push({ family, sharedPrefix: prefix, branches })
                } else {
                    branches.push({ names: trunk })
                    chains.push({ family, sharedPrefix: [], branches })
                }
            }

            // If no branches were created (shouldn't happen), push empty
            if (branches.length === 0 && heads.length === 0) {
                chains.push({ family, sharedPrefix: [], branches: [] })
            }
        }

        // Sort: rookieFamily first, then alphabetical
        chains.sort((a, b) => {
            if (a.family === rookieFamily) return -1
            if (b.family === rookieFamily) return 1
            return a.family.localeCompare(b.family)
        })

        return chains
    }

    static getAllEvolutions(rookieName: string): { name: string, requirements: EvolutionRequirement[] }[] {
        const rawData = DigivolvingRequirementsTable as unknown as Record<string, Record<string, EvolutionRequirement[]>>
        const rookieEvolutions = rawData[rookieName]
        if (!rookieEvolutions) return []
        
        return Object.keys(rookieEvolutions).map(evoName => ({
            name: evoName,
            requirements: rookieEvolutions[evoName]!
        }))
    }

    static checkRequirements(digimon: import('../models').Digimon, node: { requirements: EvolutionRequirement[] }): boolean {
        if (node.requirements.length === 0) return true

        for (const req of node.requirements) {
            switch (req.Type) {
                case 'DigimonLevel':
                    if (digimon.basicInfo.level < req.Value) return false
                    break
                case 'Attribute':
                    const attrValue = digimon.attributes[req.Attribute?.toLowerCase() as keyof import('../models').Attributes] || 0
                    if (attrValue < req.Value) return false
                    break
                case 'DigievolutionLevel':
                    return false // Verification needs mapping
            }
        }
        return true
    }

}
