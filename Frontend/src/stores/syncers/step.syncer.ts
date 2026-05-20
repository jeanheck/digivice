import type { Step } from '../../models/Journal';
import type { StepDTO } from '../../events/dto/journals/quests/step.dto';
import { RequisiteSyncer } from './requisite.syncer';
import { RequisiteConverter } from '../../events/converters/requisite.converter';

export class StepSyncer {
    public static sync(previousStep: Step, newStepDto: StepDTO): void {
        if (newStepDto.value !== undefined) {
            previousStep.isDone = newStepDto.value !== 0;
        }

        if (newStepDto.requisites !== undefined) {
            if (!previousStep.requisites) {
                previousStep.requisites = [];
            }

            newStepDto.requisites.forEach((requisiteDto) => {
                const existing = previousStep.requisites!.find((r) => r.id === requisiteDto.id);

                if (existing) {
                    RequisiteSyncer.sync(existing, requisiteDto);
                } else {
                    previousStep.requisites!.push(RequisiteConverter.convert(requisiteDto));
                }
            });
        }
    }
}
