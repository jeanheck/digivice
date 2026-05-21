import type { Vitals } from '../../models';
import type { VitalsDTO } from '../../events/dto/parties/digimons/vitals.dto';

export class VitalsSyncer {
    public static sync(previousVitals: Vitals, vitalsDto: VitalsDTO): void {
        if (vitalsDto.currentHP !== undefined) {
            previousVitals.currentHP = vitalsDto.currentHP;
        }
        if (vitalsDto.maxHP !== undefined) {
            previousVitals.maxHP = vitalsDto.maxHP;
        }
        if (vitalsDto.currentMP !== undefined) {
            previousVitals.currentMP = vitalsDto.currentMP;
        }
        if (vitalsDto.maxMP !== undefined) {
            previousVitals.maxMP = vitalsDto.maxMP;
        }
    }
}
