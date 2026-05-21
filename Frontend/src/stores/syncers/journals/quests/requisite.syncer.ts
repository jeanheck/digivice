import type { RequisiteDTO } from "@/events/dto/journals/quests/requisite.dto";
import type { Requisite } from "@/models";

export class RequisiteSyncer {
    public static sync(previousRequisite: Requisite, newRequisiteDto: RequisiteDTO): void {
        if (newRequisiteDto.value !== undefined) {
            previousRequisite.isDone = newRequisiteDto.value !== 0;
        }
    }
}
