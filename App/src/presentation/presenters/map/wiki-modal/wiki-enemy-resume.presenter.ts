import type { Party } from "@/state/models";
import { PartyDvexpConverter } from "@/presentation/presenters/converter/party-dvexp.converter";
import { DigimonRepository } from "@/presentation/repositories";
import { DvexpService } from "@/presentation/services/dvexp.service";
import type { PartyDvexpViewModel } from "@/presentation/viewmodels/dvexp/party-dvexp.viewmodel";
import type { EnemyLocationViewModel } from "@/presentation/viewmodels/enemy/enemy-location.viewmodel";

export class WikiEnemyResumePresenter {
  public static getEnemyLocations(
    locations: EnemyLocationViewModel[] | undefined,
  ): EnemyLocationViewModel[] {
    return [...(locations ?? [])].sort((first, second) => {
      return first.id.localeCompare(second.id);
    });
  }

  public static getPartyDvexp(party: Party, enemyBaseDvexp: number): PartyDvexpViewModel[] {
    return DvexpService.getPartyBattleGains(party, enemyBaseDvexp).map((gain) => {
      return PartyDvexpConverter.convert(gain, DigimonRepository.getNameById(gain.digimonId));
    });
  }
}
