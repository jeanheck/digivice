import type { Vitals } from '../../models';
import type { VitalsDTO } from '../../events/dto/parties/digimons/vitals.dto';

export class VitalsSyncer {
    public static sync(previousVitals: Vitals, newVitalsDto: VitalsDTO): void {
        if (newVitalsDto.currentHP !== undefined) {
            previousVitals.currentHP = newVitalsDto.currentHP;
        }
        if (newVitalsDto.maxHP !== undefined) {
            previousVitals.maxHP = newVitalsDto.maxHP;
        }
        if (newVitalsDto.currentMP !== undefined) {
            previousVitals.currentMP = newVitalsDto.currentMP;
        }
        if (newVitalsDto.maxMP !== undefined) {
            previousVitals.maxMP = newVitalsDto.maxMP;
        }
    }
}
