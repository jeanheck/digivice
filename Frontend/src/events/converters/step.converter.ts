import type { StepDTO } from '../dto/journals/quests/step.dto';
import type { QuestStep } from '../../models/Journal';
import { RequisiteConverter } from './requisite.converter';

export class StepConverter {
    public static convert(dto: StepDTO): QuestStep {
        return {
            number: dto.number,
            isCompleted: dto.value !== undefined && dto.value !== 0,
            prerequisites: dto.requisites ? dto.requisites.map(r => RequisiteConverter.convert(r)) : []
        };
    }
}
