import type { DeepRequired } from "@/events/dto/deep-required";
import type { StepDTO } from "@/events/dto/journals/quests/step.dto";
import { RequisiteConverter } from "./requisite.converter";
import type { Step } from "@/models";

export class StepConverter {
  public static convert(stepDto: DeepRequired<StepDTO>): Step {
    return {
      number: stepDto.number,
      isDone: stepDto.isDone,
      requisites: stepDto.requisites.map((requisiteDto) => RequisiteConverter.convert(requisiteDto)),
    };
  }
}
