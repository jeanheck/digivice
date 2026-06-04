export class DigievolutionDvxpSegmentsPresenter {
    public static getFilledSegmentCount(dvxp: number): number {
        if (dvxp < 0) {
            return 0;
        }

        return Math.floor(dvxp) % 10;
    }
}
