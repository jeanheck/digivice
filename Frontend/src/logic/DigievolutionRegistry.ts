import digievolutionData from '../database/Digievolution.json';

export class DigievolutionRegistry {
    private static digievolutionNameById: Map<number, string> = new Map();
    private static isInitialized = false;

    private static initialize() {
        if (this.isInitialized) {
            return;
        }

        digievolutionData.digievolutions.forEach(digievolution => {
            if (digievolution.id !== null && digievolution.id !== undefined) {
                this.digievolutionNameById.set(digievolution.id, digievolution.name);
            }
        });

        this.isInitialized = true;
    }

    public static getDigievolutionNameById(id: number): string {
        this.initialize();
        return this.digievolutionNameById.get(id) ?? `Unknown (${id})`;
    }
}
