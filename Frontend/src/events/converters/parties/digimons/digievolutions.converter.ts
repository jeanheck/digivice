
export class DigievolutionsConverter {
    /*public static convert(digievolutions: DigievolutionSlotDTO[] | null): DigievolutionSlot[] {
        const result: DigievolutionSlot[] = Array.from({ length: 8 }, (_, i) => ({
            index: i,
            digievolutionId: 0,
            digievolution: null
        }));

        if (!digievolutions) return result;

        digievolutions.forEach(slot => {
            if (slot && slot.index >= 0 && slot.index < result.length) {
                const converted = DigievolutionSlotConverter.convert(slot);
                if (converted) {
                    result[slot.index] = converted;
                }
            }
        });

        return result;
    }*/
}
