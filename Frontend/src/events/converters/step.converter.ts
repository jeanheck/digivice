import type { StepDTO } from '../dto/journals/quests/step.dto';
import type { Step } from '../../models/Journal';
import { RequisiteConverter } from './requisite.converter';

export class StepConverter {
    public static convert(stepDto: StepDTO): Step {
        return {
            number: stepDto.number,
            isDone: stepDto.value !== undefined && stepDto.value !== 0,
            requisites: stepDto.requisites ? stepDto.requisites.map(r => RequisiteConverter.convert(r)) : []
        };
    }
}
