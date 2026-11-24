import type { SeabedModalPinViewModel } from "./seabed-modal-pin.viewmodel";

export interface SeabedModalViewModel {
  imageUrl: string | null;
  pins: SeabedModalPinViewModel[];
}
