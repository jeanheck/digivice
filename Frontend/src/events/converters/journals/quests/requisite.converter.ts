import type { DeepRequired } from "@/events/dto/deep-required";
import type { RequisiteDTO } from "@/events/dto/journals/quests/requisite.dto";
import type { Requisite } from "@/models";

export class RequisiteConverter {
  public static convert(requisiteDto: DeepRequired<RequisiteDTO>): Requisite {
    return {
      id: requisiteDto.id,
      isDone: requisiteDto.isDone,
    };
  }
}
