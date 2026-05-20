import type { Quest } from '../../models/Journal';
import type { QuestDTO } from '../../events/dto/journals/quest.dto';
import { StepSyncer } from './step.syncer';
import { StepConverter } from '../../events/converters/step.converter';
import { RequisiteSyncer } from './requisite.syncer';
import { RequisiteConverter } from '../../events/converters/requisite.converter';

export class QuestSyncer {
    public static sync(previousQuest: Quest, newQuestDto: QuestDTO): void {
        if (newQuestDto.requisites !== undefined) {
            newQuestDto.requisites.forEach((requisiteDto) => {
                const existing = previousQuest.requisites.find((r) => r.id === requisiteDto.id);

                if (existing) {
                    RequisiteSyncer.sync(existing, requisiteDto);
                } else {
                    previousQuest.requisites.push(RequisiteConverter.convert(requisiteDto));
                }
            });
        }

        if (newQuestDto.steps !== undefined) {
            newQuestDto.steps.forEach((stepDto) => {
                const existing = previousQuest.steps.find((s) => s.number === stepDto.number);

                if (existing) {
                    StepSyncer.sync(existing, stepDto);
                } else {
                    previousQuest.steps.push(StepConverter.convert(stepDto));
                }
            });
        }
    }
}
