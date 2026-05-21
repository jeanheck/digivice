import type { StepDTO } from '@/events/dto/journals/quests/step.dto';
import { RequisiteConverter } from './requisite.converter';
import type { Step } from '@/models';

export class StepConverter {
    public static convert(stepDto: StepDTO): Step {
        return {
            number: stepDto.number,
            isDone: stepDto.value !== undefined && stepDto.value !== 0,
            requisites: stepDto.requisites ? stepDto.requisites.map(r => RequisiteConverter.convert(r)) : []
        };
    }
}
