import type { AttributesDTO } from "@/events/dto/parties/digimons/attributes.dto";
import type { Attributes } from "@/models";

export class AttributesSyncer {
    public static sync(previousAttributes: Attributes, newAttributesDto: AttributesDTO): void {
        if (newAttributesDto.strength !== undefined) {
            previousAttributes.strength = newAttributesDto.strength;
        }
        if (newAttributesDto.defense !== undefined) {
            previousAttributes.defense = newAttributesDto.defense;
        }
        if (newAttributesDto.spirit !== undefined) {
            previousAttributes.spirit = newAttributesDto.spirit;
        }
        if (newAttributesDto.wisdom !== undefined) {
            previousAttributes.wisdom = newAttributesDto.wisdom;
        }
        if (newAttributesDto.speed !== undefined) {
            previousAttributes.speed = newAttributesDto.speed;
        }
        if (newAttributesDto.charisma !== undefined) {
            previousAttributes.charisma = newAttributesDto.charisma;
        }
    }
}
