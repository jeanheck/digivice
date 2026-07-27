export class DigievolutionDvxpPresenter {
    public static readonly MAX_DVXP_BY_LEVEL = 10;

    public static getCalculatedDvxp(dvxp: number): number {
        if (dvxp < 0) {
            return 0;
        }

        return Math.floor(dvxp) % 10;
    }
}
