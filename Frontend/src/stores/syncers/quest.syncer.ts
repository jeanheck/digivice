import type { Quest } from '../../models';
import type { QuestDTO } from '../../events/dto/journals/quest.dto';
import { StepSyncer } from './step.syncer';
import { RequisiteSyncer } from './requisite.syncer';

export class QuestSyncer {
    public static sync(previousQuest: Quest, newQuestDto: QuestDTO): void {
        if (newQuestDto.requisites && newQuestDto.requisites.length > 0 && previousQuest.requisites) {
            newQuestDto.requisites.forEach((newRequisiteDto) => {
                const previousRequisite = previousQuest.requisites.find((r) => r.id === newRequisiteDto.id);

                if (previousRequisite) {
                    RequisiteSyncer.sync(previousRequisite, newRequisiteDto);
                }
            });
        }

        if (newQuestDto.steps && newQuestDto.steps.length > 0 && previousQuest.steps) {
            newQuestDto.steps.forEach((stepDto) => {
                const previousStep = previousQuest.steps.find((s) => s.number === stepDto.number);

                if (previousStep) {
                    StepSyncer.sync(previousStep, stepDto);
                }
            });
        }
    }
}
