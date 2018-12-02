using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using codeshaper.entity.unit;

namespace codeshaper.buildings.harvestable {

    public class HarvestableRock : HarvestableObject {

        public override bool harvest(UnitBuilder builder) {
            bool flag = base.harvest(builder);

            return flag;
        }
    }
}
