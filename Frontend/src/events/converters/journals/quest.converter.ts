import type { DeepRequired } from "@/events/dto/deep-required";
import type { QuestDTO } from "@/events/dto/journals/quest.dto";
import type { Quest } from "@/models";
import { RequisiteConverter } from "./quests/requisite.converter";
import { StepConverter } from "./quests/step.converter";

export class QuestConverter {
  public static convert(questDto: DeepRequired<QuestDTO>): Quest {
    return {
      id: questDto.id,
      requisites: questDto.requisites.map((requisiteDto) => RequisiteConverter.convert(requisiteDto)),
      steps: questDto.steps.map((stepDto) => StepConverter.convert(stepDto)),
    };
  }
}
