import common from "./common.json";
import areaInformation from "./area-information.json";
import enemy from "./enemies/enemy.json";
import species from "./enemies/species.json";
import conditions from "./enemies/conditions.json";
import techniqueTypes from "./technique-types.json";
import digimon from "./digimon.json";
import stats from "./stats.json";
import equipments from "./equipments.json";
import equipmentType from "./equipment-type.json";
import location from "./location.json";
import technique from "./technique.json";
import digivolution from "./digievolution.json";
import journal from "./journal.json";
import mainQuest from "./quest/main-quest.json";
import fishingPole from "./quest/side-quest/fishing-pole.json";
import folderBag from "./quest/side-quest/folder-bag.json";
import treeBoots from "./quest/side-quest/tree-boots.json";

export default {
  ...common,
  ...areaInformation,
  ...enemy,
  ...species,
  ...conditions,
  ...techniqueTypes,
  ...digimon,
  ...stats,
  ...equipments,
  ...equipmentType,
  ...location,
  ...technique,
  ...digivolution,
  ...journal,
  ...mainQuest,
  ...fishingPole,
  ...folderBag,
  ...treeBoots
};


