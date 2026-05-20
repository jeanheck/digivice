import type { Step } from '../../models/Journal';
import type { StepDTO } from '../../events/dto/journals/quests/step.dto';
import { RequisiteSyncer } from './requisite.syncer';

export class StepSyncer {
    public static sync(previousStep: Step, newStepDto: StepDTO): void {
        if (newStepDto.value !== undefined) {
            previousStep.isDone = newStepDto.value !== 0;
        }
        
        if (newStepDto.requisites && newStepDto.requisites.length > 0) {
            newStepDto.requisites.forEach((newRequisiteDto) => {
                const previousRequisite = previousStep.requisites!.find((r) => r.id === newRequisiteDto.id);

                if (previousRequisite) {
                    RequisiteSyncer.sync(previousRequisite, newRequisiteDto);
                }
            });
        }
    }
}
