import common from "./common.json";
import gameplay from "./gameplay.json";
import battle from "./battle.json";
import digimon from "./digimon.json";
import stats from "./stats.json";
import equipments from "./equipments.json";

export default {
  ...common,
  ...gameplay,
  ...battle,
  ...digimon,
  ...stats,
  ...equipments
};

