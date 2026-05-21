import type { Digimon } from '../../models';
import type { DigimonDTO } from '../../events/dto/parties/digimon.dto';
import { EquipmentsConverter } from '../../events/converters/equipments.converter';
import { AttributesStateManager } from '../../stateManagers/AttributesStateManager';
import { ResistancesStateManager } from '../../stateManagers/ResistancesStateManager';
import { DigievolutionSlotSyncer } from './digievolution-slot.syncer';

export class DigimonSyncer {
    public static sync(previousDigimon: Digimon, digimonDto: DigimonDTO): void {
        if (digimonDto.level !== undefined) {
            previousDigimon.level = digimonDto.level;
        }
        if (digimonDto.experience !== undefined) {
            previousDigimon.experience = digimonDto.experience;
        }

        if (digimonDto.vitals) {
            const vitalsDto = digimonDto.vitals;
            if (vitalsDto.currentHP !== undefined) {
                previousDigimon.vitals.currentHP = vitalsDto.currentHP;
            }
            if (vitalsDto.maxHP !== undefined) {
                previousDigimon.vitals.maxHP = vitalsDto.maxHP;
            }
            if (vitalsDto.currentMP !== undefined) {
                previousDigimon.vitals.currentMP = vitalsDto.currentMP;
            }
            if (vitalsDto.maxMP !== undefined) {
                previousDigimon.vitals.maxMP = vitalsDto.maxMP;
            }
        }

        if (digimonDto.equipments) {
            previousDigimon.equipments = EquipmentsConverter.convert(digimonDto.equipments)!;
        }

        if (digimonDto.activeDigievolutionId !== undefined) {
            previousDigimon.activeDigievolutionId = digimonDto.activeDigievolutionId;
        }

        if (digimonDto.digievolutions && digimonDto.digievolutions.length > 0) {
            digimonDto.digievolutions.forEach((slotDto) => {
                if (slotDto) {
                    const index = slotDto.index;
                    if (index >= 0 && index < previousDigimon.digievolutions.length) {
                        DigievolutionSlotSyncer.sync(previousDigimon.digievolutions, index, slotDto);
                    }
                }
            });
        }

        if (digimonDto.attributes) {
            AttributesStateManager.refresh(previousDigimon, digimonDto.attributes);
        } else {
            if (digimonDto.equipments || digimonDto.activeDigievolutionId !== undefined) {
                AttributesStateManager.refresh(previousDigimon);
            }
        }

        if (digimonDto.resistances) {
            ResistancesStateManager.refresh(previousDigimon, digimonDto.resistances);
        } else {
            if (digimonDto.equipments || digimonDto.activeDigievolutionId !== undefined) {
                ResistancesStateManager.refresh(previousDigimon);
            }
        }
    }
}
