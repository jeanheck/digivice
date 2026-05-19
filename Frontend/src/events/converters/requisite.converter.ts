import type { RequisiteDTO } from '../dto/journals/quests/requisite.dto';
import type { Requisite } from '../../models/Journal';

export class RequisiteConverter {
    public static convert(dto: RequisiteDTO): Requisite & { itemKey?: string; id?: string } {
        return {
            id: dto.id,
            itemKey: dto.id, // Mapeia id para itemKey permitindo tradução local automática
            description: '', // Preenchido pelo localizador
            isDone: dto.value !== undefined && dto.value !== 0
        };
    }
}
