import type { QuestDTO } from '../dto/journals/quest.dto';
import type { Quest } from '../../models/Journal';
import { RequisiteConverter } from './requisite.converter';
import { StepConverter } from './step.converter';

export class QuestConverter {
    public static convert(dto: Required<QuestDTO>): Quest {
        return {
            id: dto.id,
            title: '', // Preenchido via getLocalizedQuest no composable de localização
            description: '',
            prerequisites: dto.requisites ? dto.requisites.map(r => RequisiteConverter.convert(r)) : [],
            steps: dto.steps ? dto.steps.map(s => StepConverter.convert(s)) : []
        };
    }
}
