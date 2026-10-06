import { ImageCatalog } from "@/catalogs/image.catalog";
import type { PartyDvexpGain } from "@/services/dvexp.service";
import type { PartyDvexpViewModel } from "@/viewmodels/dvexp/party-dvexp.viewmodel";

export class PartyDvexpConverter {
  public static convert(gain: PartyDvexpGain, digimonName: string): PartyDvexpViewModel {
    return {
      digimonId: gain.digimonId,
      digimonName,
      imageUrl: ImageCatalog.getDigimonImageUrl(`${digimonName}-healthy`),
      dvexp: gain.dvexp,
    };
  }
}
