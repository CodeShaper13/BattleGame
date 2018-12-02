using codeshaper.entity;
using codeshaper.registry;
using codeshaper.team;
using UnityEngine;

namespace codeshaper.map.generation.structure {

    public abstract class StructureBase {

        protected Map map;
        protected Team team;
        protected Vector3 orgin;
        protected System.Random rnd;

        public StructureBase(Map map, Vector3 pos, Team exclude, long seed) {
            this.map = map;
            this.orgin = pos;
            this.rnd = new System.Random((int)seed & this.orgin.GetHashCode());

            int teamCount = Team.ALL_TEAMS.Length;
            int i = this.rnd.Next(0, teamCount);
            if(Team.ALL_TEAMS[i] == exclude) {
                i++;
                if(i >= teamCount) {
                    i = 0;
                }
            }
            this.team = Team.ALL_TEAMS[i];
        }

        /// <summary>
        /// Generates the structure.
        /// </summary>
        public abstract void generate();

        protected void placeBuilding(RegisteredObject obj, Vector3 pos) {
            SidedObjectEntity entity = (SidedObjectEntity)this.map.spawnEntity(obj, pos, Quaternion.identity);
            entity.setTeam(this.team);
        }
    }
}
