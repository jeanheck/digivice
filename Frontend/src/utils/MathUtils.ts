export class MathUtils {
    public static calculatePercentage(current: number, max: number): number {
        if (max <= 0) {
            return 0;
        }

        const percentage = (current / max) * 100;
        return Math.min(Math.max(percentage, 0), 100);
    }

    public static Sum(values: number[]): number {
        return values.reduce((total, current) => total + current, 0);
    }
}
