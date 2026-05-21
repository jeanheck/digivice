import type { AttributesDTO } from "@/events/dto/parties/digimons/attributes.dto";
import type { Attributes } from "@/models/attributes";

export class AttributesConverter {
    public static convert(newAttributesDto: AttributesDTO | null): Attributes {
        return {
            strength: newAttributesDto?.strength ?? 0,
            defense: newAttributesDto?.defense ?? 0,
            spirit: newAttributesDto?.spirit ?? 0,
            wisdom: newAttributesDto?.wisdom ?? 0,
            speed: newAttributesDto?.speed ?? 0,
            charisma: newAttributesDto?.charisma ?? 0
        };
    }
}
