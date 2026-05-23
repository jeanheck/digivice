import common from "./common.json";
import areaInformation from "./area-information.json";
import enemy from "./enemies/enemy.json";
import species from "./enemies/species.json";
import conditions from "./enemies/conditions.json";
import techniqueTypes from "./technique-types.json";
import digimon from "./digimon.json";
import stats from "./stats.json";
import equipments from "./equipments.json";
import digivolution from "./digievolution.json";
import journal from "./journal.json";

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
  ...digivolution,
  ...journal
};


