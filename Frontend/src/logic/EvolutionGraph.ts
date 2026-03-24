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
    children?: GraphNode[] // Legacy tree support
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

    /**
     * Builds a Graph (DAG) for the skill tree.
     * Currently only places the Rookie node at center.
     * Further nodes will be added incrementally.
     */
    static buildGraph(rookieName: string): DigievolutionGraph {
        const rawData = DigivolvingRequirementsTable as unknown as Record<string, Record<string, EvolutionRequirement[]>>
        const rookieEvolutions = rawData[rookieName]

        // Rookie node at center (0, 0)
        const rookieNode: GraphNode = {
            id: rookieName, name: rookieName, requirements: [],
            level: 0, x: 0, y: 0
        }

        if (!rookieEvolutions) {
            return { nodes: [rookieNode], links: [] }
        }

        const nodes: GraphNode[] = [rookieNode]
        const links: GraphLink[] = []
        const placed = new Set<string>([rookieName])

        // Spacing: horizontal gap accounts for node width (176px), vertical for height (80px)
        const H_GAP = 220   // horizontal center-to-center distance
        const V_GAP = 120   // vertical center-to-center distance

        // Directions: TOP, RIGHT, BOTTOM, LEFT
        const DIRECTIONS = [
            { x: 0,      y: -V_GAP }, // TOP
            { x: H_GAP,  y: 0      }, // RIGHT
            { x: 0,      y: V_GAP  }, // BOTTOM
            { x: -H_GAP, y: 0      }, // LEFT
        ]

        // Find children of a given parent node:
        // - For the Rookie: evolutions with DigimonLevel only (no DigievolutionLevel)
        // - For others: evolutions that require this node via DigievolutionLevel
        const findChildren = (parentName: string): { name: string, value: number, reqs: EvolutionRequirement[] }[] => {
            const children: { name: string, value: number, reqs: EvolutionRequirement[] }[] = []
            
            for (const evoName in rookieEvolutions) {
                if (placed.has(evoName)) continue
                const reqs = rookieEvolutions[evoName]!

                if (parentName === rookieName) {
                    // Root level: only DigimonLevel requirements (no DigievolutionLevel)
                    const hasDigievoReq = reqs.some(r => r.Type === 'DigievolutionLevel')
                    const digimonLevelReq = reqs.find(r => r.Type === 'DigimonLevel')
                    if (!hasDigievoReq && digimonLevelReq) {
                        children.push({ name: evoName, value: digimonLevelReq.Value, reqs })
                    }
                } else {
                    // Child level: requires parentName via DigievolutionLevel
                    const parentReq = reqs.find(r => r.Type === 'DigievolutionLevel' && r.Digievolution === parentName)
                    if (parentReq) {
                        children.push({ name: evoName, value: parentReq.Value, reqs })
                    }
                }
            }

            // Sort by requirement value ascending
            children.sort((a, b) => a.value - b.value)
            return children
        }

        // Recursive placement: place children around parent, then recurse
        const placeChildren = (parentName: string, parentX: number, parentY: number, level: number) => {
            const children = findChildren(parentName)

            children.forEach((child, i) => {
                if (i >= DIRECTIONS.length) return
                const dir = DIRECTIONS[i]
                if (!dir) return

                const childX = parentX + dir.x
                const childY = parentY + dir.y

                const node: GraphNode = {
                    id: child.name, name: child.name, requirements: child.reqs,
                    level, x: childX, y: childY
                }
                nodes.push(node)
                links.push({ source: parentName, target: child.name })
                placed.add(child.name)

                // Recurse: place this node's children around it
                placeChildren(child.name, childX, childY, level + 1)
            })
        }

        // Start from the Rookie
        placeChildren(rookieName, 0, 0, 1)

        return { nodes, links }
    }

    /**
     * Legacy support for tree-based components.
     */
    static buildTree(rookieName: string): GraphNode {
        const rawData = DigivolvingRequirementsTable as unknown as Record<string, Record<string, EvolutionRequirement[]>>

        // Safely check if rookie exists
        const rookieEvolutions = rawData[rookieName]
        if (!rookieEvolutions) {
            return {
                id: rookieName,
                name: rookieName,
                requirements: [],
                children: [],
                level: 0,
                x: 0,
                y: 0
            }
        }

        // Map of parentName -> array of childNames
        const childrenMap: Record<string, string[]> = {}
        childrenMap[rookieName] = []

        for (const evoName in rookieEvolutions) {
            const parts = rookieEvolutions[evoName]!
            const parentReqs = parts.filter(p => p.Type === 'DigievolutionLevel')

            if (parentReqs.length === 0) {
                childrenMap[rookieName].push(evoName)
            } else {
                for (const req of parentReqs) {
                    const parentName = req.Digievolution!
                    if (!childrenMap[parentName]) childrenMap[parentName] = []
                    childrenMap[parentName].push(evoName)
                }
            }
        }

        const buildNode = (nodeName: string, path: string, level: number, existingReqs: EvolutionRequirement[] = []): GraphNode => {
            const childrenNames = childrenMap[nodeName] || []
            const node: GraphNode = {
                id: `${path}_${nodeName}`,
                name: nodeName,
                requirements: existingReqs,
                children: [],
                level: level,
                x: 0,
                y: 0
            }

            for (const childName of childrenNames) {
                if (path.includes(`_${childName}_`)) continue
                const childNode = buildNode(childName, node.id, level + 1, rookieEvolutions[childName]!)
                node.children?.push(childNode)
            }

            return node
        }

        return buildNode(rookieName, 'root', 0)
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

    static checkRequirements(digimon: import('../types/backend').Digimon, node: { requirements: EvolutionRequirement[] }): boolean {
        if (node.requirements.length === 0) return true

        for (const req of node.requirements) {
            switch (req.Type) {
                case 'DigimonLevel':
                    if (digimon.basicInfo.level < req.Value) return false
                    break
                case 'Attribute':
                    const attrValue = digimon.attributes[req.Attribute?.toLowerCase() as keyof import('../types/backend').Attributes] || 0
                    if (attrValue < req.Value) return false
                    break
                case 'DigievolutionLevel':
                    return false // Verification needs mapping
            }
        }
        return true
    }

    static findEvolutionPath(targetName: string, currentNode: GraphNode, currentPath: string[] = []): string[] | null {
        const newPath = [...currentPath, currentNode.name]
        if (currentNode.name === targetName) return newPath
        if (!currentNode.children) return null

        for (const child of currentNode.children) {
            const foundPath = this.findEvolutionPath(targetName, child, newPath)
            if (foundPath) return foundPath
        }
        return null
    }
}
