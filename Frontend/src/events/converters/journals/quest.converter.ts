import type { QuestDTO } from "@/events/dto/journals/quest.dto";
import type { Quest } from "@/models";
import { RequisiteConverter } from "./quests/requisite.converter";
import { StepConverter } from "./quests/step.converter";

export class QuestConverter {
    public static convert(questDto: Required<QuestDTO>): Quest {
        return {
            id: questDto.id,
            requisites: questDto.requisites ? questDto.requisites.map(r => RequisiteConverter.convert(r)) : [],
            steps: questDto.steps ? questDto.steps.map(s => StepConverter.convert(s)) : []
        };
    }
}
