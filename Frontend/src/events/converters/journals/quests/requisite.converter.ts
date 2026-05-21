import type { RequisiteDTO } from "@/events/dto/journals/quests/requisite.dto";
import type { Requisite } from "@/models";

export class RequisiteConverter {
    public static convert(requisiteDto: RequisiteDTO): Requisite {
        return {
            id: requisiteDto.id,
            isDone: requisiteDto.value !== undefined && requisiteDto.value !== 0
        };
    }
}
