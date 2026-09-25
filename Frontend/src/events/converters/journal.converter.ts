import type { DeepRequired } from "@/events/dto/deep-required";
import type { JournalDTO } from "@/events/dto/journal.dto";
import type { Journal } from "@/models";
import { QuestConverter } from "./journals/quest.converter";

export class JournalConverter {
  public static convert(journalDto: DeepRequired<JournalDTO>): Journal {
    return {
      mainQuest: QuestConverter.convert(journalDto.mainQuest),
      sideQuests: journalDto.sideQuests.map((questDto) => QuestConverter.convert(questDto)),
      legendaryWeapons: journalDto.legendaryWeapons.map((questDto) => QuestConverter.convert(questDto)),
      driAgents: journalDto.driAgents.map((questDto) => QuestConverter.convert(questDto)),
      duelIsland: journalDto.duelIsland.map((questDto) => QuestConverter.convert(questDto)),
    };
  }
}
