/**
 * Centralized Mathematical Utilities
 */
export class MathUtils {
    /**
     * Calculates the percentage between a current value and a maximum value.
     * Returns a number between 0 and 100.
     */
    public static calculatePercentage(current: number, max: number): number {
        if (max <= 0) {
            return 0;
        }

        const percentage = (current / max) * 100;
        
        // Clamp the result between 0 and 100
        return Math.min(Math.max(percentage, 0), 100);
    }
}
