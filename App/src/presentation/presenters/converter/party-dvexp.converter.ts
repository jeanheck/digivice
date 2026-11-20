import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import type { PartyDvexpGain } from "@/presentation/services/dvexp.service";
import type { PartyDvexpViewModel } from "@/presentation/viewmodels/dvexp/party-dvexp.viewmodel";

export class PartyDvexpConverter {
  public static convert(gain: PartyDvexpGain, digimonName: string): PartyDvexpViewModel {
    return {
      digimonId: gain.digimonId,
      digimonName,
      imageUrl: ImageCatalog.getDigimonImageUrl(digimonName),
      dvexp: gain.dvexp,
    };
  }
}
