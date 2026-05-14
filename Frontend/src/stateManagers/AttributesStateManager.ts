import type { Digimon } from '../models/Digimon';
import type { AttributesDTO } from '../dtos/events.dto';
import { AttributesConverter } from '../converters/AttributesConverter';
import { AttributesUpdater } from '../updaters/AttributesUpdater';

export class AttributesStateManager {
    public static refresh(digimon: Digimon, newBaseAttributes?: AttributesDTO): void {
        const baseAttributes: AttributesDTO = newBaseAttributes || {
            strength: digimon.attributes.strength.fromDigimon,
            defense: digimon.attributes.defense.fromDigimon,
            spirit: digimon.attributes.spirit.fromDigimon,
            wisdom: digimon.attributes.wisdom.fromDigimon,
            speed: digimon.attributes.speed.fromDigimon,
            charisma: digimon.attributes.charisma.fromDigimon
        };

        const newAttributes = AttributesConverter.convert(
            baseAttributes, 
            digimon.equipments, 
            digimon.activeDigievolutionId
        );
        
        AttributesUpdater.update(digimon, newAttributes);
    }
}
