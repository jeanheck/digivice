import type {
  QuestAddresses,
  QuestAddressesRaw,
} from "@/memory/addresses/journals/quest.addresses";
import type {
  RequisiteAddresses,
  RequisiteAddressesRaw,
} from "@/memory/addresses/journals/quests/requisite.addresses";
import type {
  StepAddresses,
  StepAddressesRaw,
} from "@/memory/addresses/journals/quests/step.addresses";
import { AddressValueConverter } from "@/memory/converters/address-value.converter";

export class QuestAddressesConverter {
  private static convertId(id: string | undefined): string {
    if (typeof id !== "string" || id === "") {
      throw new Error(`Invalid id: ${JSON.stringify(id)}.`);
    }

    return id;
  }

  private static convertRequisite(raw: RequisiteAddressesRaw): RequisiteAddresses {
    return {
      id: QuestAddressesConverter.convertId(raw.id),
      address: AddressValueConverter.convert(raw.address),
      bitMasks: AddressValueConverter.convertList(raw.bitMasks),
    };
  }

  private static convertRequisites(
    raws: RequisiteAddressesRaw[] | undefined,
  ): RequisiteAddresses[] {
    return (raws ?? []).map((raw) => QuestAddressesConverter.convertRequisite(raw));
  }

  private static convertStep(raw: StepAddressesRaw): StepAddresses {
    return {
      number: AddressValueConverter.convert(raw.number),
      address: AddressValueConverter.convert(raw.address),
      bitMasks: AddressValueConverter.convertList(raw.bitMasks),
      requisites: QuestAddressesConverter.convertRequisites(raw.requisites),
    };
  }

  public static convert(raw: QuestAddressesRaw): QuestAddresses {
    return {
      id: QuestAddressesConverter.convertId(raw.id),
      requisites: QuestAddressesConverter.convertRequisites(raw.requisites),
      steps: (raw.steps ?? []).map((step) => QuestAddressesConverter.convertStep(step)),
    };
  }
}
