export class PartyLevelHelper {
    public static isInRange(partyLevel: number, min: number, max: number): boolean {
        return partyLevel >= min && partyLevel <= max;
    }
}
