import type { Digimon } from '../../../models';
import type { DigimonDTO } from '../../../events/dto/parties/digimon.dto';
import { VitalsSyncer } from './digimons/vitals.syncer';

import { AttributesSyncer } from './digimons/attributes.syncer';
import { ResistancesSyncer } from './digimons/resistances.syncer';
import { DigievolutionSlotSyncer } from './digimons/digievolution-slot.syncer';
import { StoredDigievolutionSyncer } from './digimons/stored-digievolution.syncer';
import { EquipmentsSyncer } from './digimons/equipments.syncer';
import { StoredDigievolutionConverter } from '@/events/converters/parties/digimons/stored-digievolution.converter';

export class DigimonSyncer {
    public static sync(previousDigimon: Digimon, newDigimonDto: DigimonDTO): void {
        if (newDigimonDto.level !== undefined) {
            previousDigimon.level = newDigimonDto.level;
        }
        if (newDigimonDto.tp !== undefined) {
            previousDigimon.tp = newDigimonDto.tp;
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
        if (newDigimonDto.storedDigievolutions && newDigimonDto.storedDigievolutions.length > 0) {
            newDigimonDto.storedDigievolutions.forEach((newStoredDigievolutionDto) => {
                if (!newStoredDigievolutionDto || newStoredDigievolutionDto.digievolutionId === undefined) {
                    return;
                }

                const previousStoredDigievolution = previousDigimon.storedDigievolutions.find(
                    (stored) => stored.digievolutionId === newStoredDigievolutionDto.digievolutionId
                );

                if (previousStoredDigievolution) {
                    StoredDigievolutionSyncer.sync(previousStoredDigievolution, newStoredDigievolutionDto);
                    return;
                }

                if (newStoredDigievolutionDto.level !== undefined) {
                    previousDigimon.storedDigievolutions.push(
                        StoredDigievolutionConverter.convert(newStoredDigievolutionDto)
                    );
                }
            });
        }
    }
}
