﻿using src.data;

namespace src.entity.unit {

    public class UnitSoldier : UnitFighting {

        public override EntityData getData() {
            return Constants.ED_SOLDIER;
        }
    }
}
