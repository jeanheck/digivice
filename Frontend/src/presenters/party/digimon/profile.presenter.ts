import { DigimonRepository } from "@/repositories/digimon.repository";

export class ProfilePresenter {
    public static getNameById(id: number): string {
        return DigimonRepository.getNameById(id);
    }
}
