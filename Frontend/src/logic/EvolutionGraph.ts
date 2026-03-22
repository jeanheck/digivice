import DigivolvingRequirementsTable from '../data/static/DigivolvingRequirementsTable.json'

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
    children: GraphNode[]
}

export class EvolutionGraph {
    /**
     * Parses the requirements table and builds a hierarchical tree for a specific rookie.
     * If an evolution has multiple DigievolutionLevel requirements, it will appear as a child
     * in multiple branches.
     * If it only requires DigimonLevel or Attributes, it branches directly from the Rookie.
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
                children: []
            }
        }

        // Pass 1: Extract all evolutions and their logical parents
        const evolutionNames = Object.keys(rookieEvolutions)

        // Map of parentName -> array of childNames
        const childrenMap: Record<string, string[]> = {}

        // Initialize root children
        childrenMap[rookieName] = []

        for (const evoName of evolutionNames) {
            const parts = rookieEvolutions[evoName]!

            // Find all DigievolutionLevel requirements
            const parentReqs = parts.filter(p => p.Type === 'DigievolutionLevel')

            if (parentReqs.length === 0) {
                // No explicit evolution parent, so it branches from Rookie
                childrenMap[rookieName].push(evoName)
            } else {
                // It branches from one or more parent evolutions
                for (const req of parentReqs) {
                    const parentName = req.Digievolution!
                    if (!childrenMap[parentName]) {
                        childrenMap[parentName] = []
                    }
                    childrenMap[parentName].push(evoName)
                }
            }
        }

        // Pass 2: Build the tree recursively
        const buildNode = (nodeName: string, path: string, existingReqs: EvolutionRequirement[] = []): GraphNode => {
            const childrenNames = childrenMap[nodeName] || []

            const node: GraphNode = {
                id: `${path}_${nodeName}`,
                name: nodeName,
                requirements: existingReqs,
                children: []
            }

            for (const childName of childrenNames) {
                // Prevent infinite loops if there are accidental cycles in the data
                if (path.includes(`_${childName}_`)) {
                    console.warn(`Cycle detected in evolution graph: ${path}_${childName}`)
                    continue
                }

                node.children.push(buildNode(childName, node.id, rookieEvolutions[childName]))
            }

            return node
        }

        return buildNode(rookieName, 'root')
    }

    /**
     * Retrieves a flat list of all possible evolutions for a given rookie.
     * This avoids the redundant node duplication necessary in tree representations.
     */
    static getAllEvolutions(rookieName: string): { name: string, requirements: EvolutionRequirement[] }[] {
        const rawData = DigivolvingRequirementsTable as unknown as Record<string, Record<string, EvolutionRequirement[]>>
        const rookieEvolutions = rawData[rookieName]
        if (!rookieEvolutions) {
            return []
        }
        
        return Object.keys(rookieEvolutions).map(evoName => ({
            name: evoName,
            requirements: rookieEvolutions[evoName]!
        }))
    }

    /**
     * Checks if a Digimon has met all the requirements for a given evolution node.
     */
    static checkRequirements(digimon: import('../types/backend').Digimon, node: GraphNode): boolean {
        // If there are no requirements (e.g., base Rookie), it's always met
        if (node.requirements.length === 0) {
            return true
        }

        // Must satisfy ALL requirements in the array
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
                    // TODO: We need a mapping of String Name -> ID to check equipped evolutions correctly
                    // Currently Digimon type only holds {id, level} for equipped evolutions.
                    // For now, this is impossible to verify accurately without a dictionary.
                    // So we will assume false for Digievolutions requirements until the ID dictionary is available.
                    return false
            }
        }

        return true
    }

    /**
     * Finds the path from a given node down to a target node name using Depth-First Search.
     * Returns an array of node names representing the path, or null if not found.
     */
    static findEvolutionPath(targetName: string, currentNode: GraphNode, currentPath: string[] = []): string[] | null {
        // Add current node to path
        const newPath = [...currentPath, currentNode.name]

        // If we found the target, return the path
        if (currentNode.name === targetName) {
            return newPath
        }

        // Recursively search children
        for (const child of currentNode.children) {
            const foundPath = this.findEvolutionPath(targetName, child, newPath)
            if (foundPath) {
                return foundPath
            }
        }

        return null
    }
}
