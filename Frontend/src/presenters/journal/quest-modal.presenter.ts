import asukaMapUrl from "@/assets/AsukaMap.webp";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { Journal } from "@/models";
import { QuestConverter } from "@/presenters/converter/quest.converter";
import { ZoomedLocationMapConverter } from "@/presenters/converter/zoomed-location-map.converter";
import { LocationRepository } from "@/repositories/location.repository";
import { QuestRepository } from "@/repositories/quest.repository";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";
import type { ZoomedLocationMapViewModel } from "@/viewmodels/quest/zoomed-location-map.viewmodel";

export class QuestModalPresenter {
    public static getQuestViewModel(journal: Journal, questId: string): QuestViewModel | null {
        const mainQuestRaw = QuestRepository.getMainQuestRaw();
        if (mainQuestRaw.id === questId) {
            if (journal.mainQuest === null) {
                return null;
            }

            return QuestConverter.convert(mainQuestRaw, journal.mainQuest, { calculateNewStatus: false });
        }

        const sideQuestRaw = QuestRepository.getSideQuestsRaw().find((raw) => raw.id === questId);
        if (sideQuestRaw !== undefined) {
            const sideQuest = journal.sideQuests.find((quest) => quest.id === questId);
            if (sideQuest === undefined) {
                return null;
            }

            return QuestConverter.convert(sideQuestRaw, sideQuest, { calculateNewStatus: true });
        }

        const legendaryWeaponRaw = QuestRepository.getLegendaryWeaponsRaw().find((raw) => raw.id === questId);
        if (legendaryWeaponRaw !== undefined) {
            const legendaryWeapon = journal.legendaryWeapons.find((quest) => quest.id === questId);
            if (legendaryWeapon === undefined) {
                return null;
            }

            return QuestConverter.convert(legendaryWeaponRaw, legendaryWeapon, { calculateNewStatus: true });
        }

        return null;
    }

    public static getWorldMapLocations(selectedStep: StepViewModel | null): ZoomedLocationMapViewModel[] {
        if (!selectedStep?.location || !selectedStep.coordinates) {
            return [];
        }

        return [
            ZoomedLocationMapConverter.convert(
                asukaMapUrl,
                selectedStep.coordinates,
                `location.${selectedStep.location}`
            ),
        ];
    }

    public static getLocalMapLocations(
        selectedStep: StepViewModel | null,
        questId: string | null
    ): ZoomedLocationMapViewModel[] {
        if (!selectedStep?.zoomedLocations?.length || !questId) {
            return [];
        }

        return selectedStep.zoomedLocations.map((zoomedLocation, locationIndex) => {
            return ZoomedLocationMapConverter.convert(
                QuestModalPresenter.getLocalMapUrl(zoomedLocation.location),
                zoomedLocation.coordinates,
                `${questId}.steps.${selectedStep.number}.locations.${locationIndex}.locationTarget`
            );
        });
    }

    private static getLocalMapUrl(locationId: string | undefined): string | null {
        if (!locationId) {
            return null;
        }

        const locationRaw = LocationRepository.getLocationById(locationId);
        return ImageCatalog.getMapImageUrl(locationRaw.image);
    }
}
