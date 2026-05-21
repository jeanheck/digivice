import type { Step } from '@/models';
import type { StepDTO } from '@/events/dto/journals/quests/step.dto';
import { RequisiteSyncer } from './requisite.syncer';

export class StepSyncer {
    public static sync(previousStep: Step, newStepDto: StepDTO): void {
        if (newStepDto.value !== undefined) {
            previousStep.isDone = newStepDto.value !== 0;
        }
        
        const previousRequisites = previousStep.requisites;
        if (newStepDto.requisites && newStepDto.requisites.length > 0 && previousRequisites) {
            newStepDto.requisites.forEach((newRequisiteDto) => {
                const previousRequisite = previousRequisites.find((r) => r.id === newRequisiteDto.id);

                if (previousRequisite) {
                    RequisiteSyncer.sync(previousRequisite, newRequisiteDto);
                }
            });
        }
    }
}
