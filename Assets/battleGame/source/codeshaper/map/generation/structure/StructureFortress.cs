using codeshaper.registry;
using codeshaper.team;
using codeshaper.util;
using System;
using UnityEngine;

namespace codeshaper.map.generation.structure {

    public class StructureFortress : StructureBase {

        public StructureFortress(Map map, Vector3 pos, Team exclude, long seed) : base(map, pos, exclude, seed) {
        }

        public override void generate() {
            bool isBig = this.rnd.Next() % 2 == 0;

            if(isBig) {
                this.placeBuilding(Registry.buildingStoreroom, this.orgin);
                int baseSizeX = this.rnd.Next(3) + 1;
                int baseSizeZ = this.rnd.Next(3) + 1;
                for (int x = -baseSizeX; x <= baseSizeX; x++) {
                    for (int z = -baseSizeZ; z <= baseSizeZ; z++) {
                        if(!(x == 0 && z == 0)) {
                            RegisteredObject obj = this.getBigBaseBuilding(x, z);
                            if (obj != null) {
                                this.placeBuilding(obj, (new Vector3(x, 0, z) * 4) + this.orgin);
                            }
                        }
                    }
                }
            } else {
                // Small base.
                this.placeBuilding(Registry.buildingTower, this.orgin);
                foreach(Vector3Int vec in Direction.CARDINAL) {
                    Vector3 v = (vec * 3) + this.orgin;
                    RegisteredObject registeredObj = this.getSmallBaseBuilding();
                    if(registeredObj != null) {
                        this.placeBuilding(registeredObj, v);
                    }
                }
            }
        }

        private RegisteredObject getBigBaseBuilding(int x, int z) {
            int i = this.rnd.Next(0, 15);
            int layer = Math.Max(Math.Abs(x), Math.Abs(z));
            if(i == 1) {
                if( layer <= 1) {
                    return Registry.buildingStoreroom;
                } else {
                    return Registry.buildingTower;
                }
            } else if(i <= 3) { // 2, 3
                return Registry.buildingTower;
            } else if(i <= 5) { // 4, 5
                return Registry.buildingTrainingHouse;
            } else if(i <= 6) { // 6
                return Registry.buildingWorkshop;
            } else if(i <= 9) { // 9
                return Registry.buildingCamp;
            } else if(i <= 10) { // 10
                return Registry.buildingProducer;
            } else { // 11, 12, 13, 14
                if(layer <= 1) {
                    return Registry.buildingProducer;
                } else {
                    return null;
                }
            }
        }

        private RegisteredObject getSmallBaseBuilding() {
            int i = this.rnd.Next(0, 10);
            if(i <= 1) { // 0, 1
                return Registry.buildingProducer;
            } else if(i <= 3) { // 2, 3
                return Registry.buildingTrainingHouse;
            } else if(i <= 6) {
                return Registry.buildingCamp; // 4, 5, 6
            } else if(i <= 8) { // 7, 8
                return Registry.buildingTower;
            } else { // 9
                return null;
            }
        }
    }
}
