import type { Digimon } from '../../../models';
import type { DigimonDTO } from '../../../events/dto/parties/digimon.dto';
import { VitalsSyncer } from './vitals.syncer';

import { AttributesSyncer } from './attributes.syncer';
import { ResistancesSyncer } from './resistances.syncer';
import { DigievolutionSlotSyncer } from './digievolution-slot.syncer';
import { EquipmentsSyncer } from './equipments.syncer';

export class DigimonSyncer {
    public static sync(previousDigimon: Digimon, newDigimonDto: DigimonDTO): void {
        if (newDigimonDto.level !== undefined) {
            previousDigimon.level = newDigimonDto.level;
        }
        if (newDigimonDto.experience !== undefined) {
            previousDigimon.experience = newDigimonDto.experience;
        }
        if (newDigimonDto.activeDigievolutionId !== undefined) {
            previousDigimon.activeDigievolutionId = newDigimonDto.activeDigievolutionId;
        }
        if (newDigimonDto.vitals) {
            VitalsSyncer.sync(previousDigimon.vitals, newDigimonDto.vitals);
        }
        if (newDigimonDto.equipments) {
            EquipmentsSyncer.sync(previousDigimon.equipments, newDigimonDto.equipments);
        }
        if (newDigimonDto.attributes) {
            AttributesSyncer.sync(previousDigimon.attributes, newDigimonDto.attributes);
        }
        if (newDigimonDto.resistances) {
            ResistancesSyncer.sync(previousDigimon.resistances, newDigimonDto.resistances);
        }
        if (newDigimonDto.digievolutions && newDigimonDto.digievolutions.length > 0) {
            newDigimonDto.digievolutions.forEach((newDigievolutionSlotDto) => {
                if (newDigievolutionSlotDto) {
                    const previousDigievolutionSlot = previousDigimon.digievolutions.find((s) => s.index === newDigievolutionSlotDto.index);
                    if (previousDigievolutionSlot) {
                        DigievolutionSlotSyncer.sync(previousDigievolutionSlot, newDigievolutionSlotDto);
                    }
                }
            });
        }
    }
}
