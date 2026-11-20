import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import type { Journal, Party } from "@/state/models";
import { QuestConverter } from "@/presentation/presenters/converter/quest.converter";
import { MapFrameSlideConverter } from "@/presentation/presenters/converter/map-frame-slide.converter";
import { MapRepository } from "@/presentation/repositories/map.repository";
import { QuestRepository } from "@/presentation/repositories/quest.repository";
import { MapService } from "@/presentation/services/map.service";
import { PartyService } from "@/presentation/services/party.service";
import type { MapFrameSlideViewModel } from "@/presentation/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { QuestViewModel } from "@/presentation/viewmodels/quest/quest.viewmodel";
import type { StepViewModel } from "@/presentation/viewmodels/quest/step.viewmodel";

export class QuestModalPresenter {
  public static getQuestViewModel(
    journal: Journal,
    questId: string,
    party: Party,
  ): QuestViewModel | null {
    const partyLevel = PartyService.getLevel(party);
    const mainQuestRaw = QuestRepository.getMainQuestRaw();
    if (mainQuestRaw.id === questId) {
      return QuestConverter.convert(mainQuestRaw, journal.mainQuest, {
        calculateNewStatus: false,
        partyLevel,
      });
    }

    const sideQuestRaw = QuestRepository.getSideQuestsRaw().find((raw) => raw.id === questId);
    if (sideQuestRaw !== undefined) {
      const sideQuest = journal.sideQuests.find((quest) => quest.id === questId);
      if (sideQuest === undefined) {
        return null;
      }

      return QuestConverter.convert(sideQuestRaw, sideQuest, {
        calculateNewStatus: true,
        partyLevel,
      });
    }

    const legendaryWeaponRaw = QuestRepository.getLegendaryWeaponsRaw().find(
      (raw) => raw.id === questId,
    );
    if (legendaryWeaponRaw !== undefined) {
      const legendaryWeapon = journal.legendaryWeapons.find((quest) => quest.id === questId);
      if (legendaryWeapon === undefined) {
        return null;
      }

      return QuestConverter.convert(legendaryWeaponRaw, legendaryWeapon, {
        calculateNewStatus: true,
        partyLevel,
      });
    }

    const driAgentRaw = QuestRepository.getDriAgentsRaw().find((raw) => raw.id === questId);
    if (driAgentRaw !== undefined) {
      const driAgent = journal.driAgents.find((quest) => quest.id === questId);
      if (driAgent === undefined) {
        return null;
      }

      return QuestConverter.convert(driAgentRaw, driAgent, {
        calculateNewStatus: true,
        partyLevel,
      });
    }

    const duelIslandQuestRaw = QuestRepository.getDuelIslandRaw().find((raw) => raw.id === questId);
    if (duelIslandQuestRaw !== undefined) {
      const duelIslandQuest = journal.duelIsland.find((quest) => quest.id === questId);
      if (duelIslandQuest === undefined) {
        return null;
      }

      return QuestConverter.convert(duelIslandQuestRaw, duelIslandQuest, {
        calculateNewStatus: true,
        partyLevel,
      });
    }

    return null;
  }

  public static getWorldMapLocations(selectedStep: StepViewModel | null): MapFrameSlideViewModel[] {
    if (!selectedStep?.location) {
      return [];
    }

    const worldLocation = MapService.getWorldLocation(selectedStep.location);
    if (worldLocation === undefined) {
      return [];
    }

    const asukaMapUrl = ImageCatalog.getLocationImageUrl("Asuka");
    if (asukaMapUrl === null) {
      return [];
    }

    return [
      MapFrameSlideConverter.convert(asukaMapUrl, worldLocation, `map.${selectedStep.location}`),
    ];
  }

  public static getLocalMapLocations(
    selectedStep: StepViewModel | null,
    questId: string | null,
  ): MapFrameSlideViewModel[] {
    if (!selectedStep?.location || !selectedStep.coordinates || !questId) {
      return [];
    }

    const composedLocations = [
      ...selectedStep.innerLocation,
      {
        location: selectedStep.location,
        coordinates: selectedStep.coordinates,
      },
    ];

    return composedLocations.map((composedLocation, locationIndex) => {
      const nextLocation = composedLocations[locationIndex + 1];
      let labelKey = `${questId}.steps.${selectedStep.number}.locationTarget`;
      if (nextLocation !== undefined) {
        labelKey = `map.${nextLocation.location}`;
      }

      return MapFrameSlideConverter.convert(
        QuestModalPresenter.getLocalMapUrl(composedLocation.location),
        composedLocation.coordinates,
        labelKey,
      );
    });
  }

  private static getLocalMapUrl(locationId: string | undefined): string | null {
    if (!locationId) {
      return null;
    }

    const locationRaw = MapRepository.getMapById(locationId);
    return ImageCatalog.getLocationImageUrl(locationRaw.imageName);
  }
}
