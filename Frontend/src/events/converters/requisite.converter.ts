import type { RequisiteDTO } from '../dto/journals/quests/requisite.dto';
import type { Requisite } from '../../models/Journal';

export class RequisiteConverter {
    public static convert(requisiteDto: RequisiteDTO): Requisite {
        return {
            id: requisiteDto.id,
            isDone: requisiteDto.value !== undefined && requisiteDto.value !== 0
        };
    }
}
