import type { QuestDTO } from '../events/dto/Journals/QuestDTO';
import type { Quest } from '../models/Journal';
import { RequisiteConverter } from './RequisiteConverter';
import { StepConverter } from './StepConverter';

export class QuestConverter {
    public static convert(dto: QuestDTO | null): Quest | null {
        if (!dto || !dto.id) return null;
        return {
            id: dto.id,
            title: '', // Preenchido via getLocalizedQuest no composable de localização
            description: '',
            prerequisites: dto.requisites ? dto.requisites.map(r => RequisiteConverter.convert(r)) : [],
            steps: dto.steps ? dto.steps.map(s => StepConverter.convert(s)) : []
        };
    }
}
