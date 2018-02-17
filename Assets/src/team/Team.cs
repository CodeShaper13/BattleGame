using System.Collections.Generic;
using UnityEngine;

namespace src.team {

    public class Team {

        public static Team NONE = new TeamNone();
        public static Team RED = new Team(1, "Red", Color.red);
        public static Team BLUE = new Team(2, "Blue", Color.blue);
        public static Team YELLOW = new Team(3, "Yellow", Color.yellow);
        public static Team GREEN = new Team(4, "Green", Color.green);

        public int id;
        public string name;
        public Color color;
        public List<SidedObjectBase> members;

        private Team(int id, string name, Color color) {
            this.id = id;
            this.name = name;
            this.color = color;
            this.members = new List<SidedObjectBase>();
        }

        public virtual void join(SidedObjectBase obj) {
            this.members.Add(obj);
        }

        public virtual void leave(SidedObjectBase obj) {
            this.members.Remove(obj);
        }

        public static Team teamFromEnum(EnumTeam e) {
            switch (e) {
                case EnumTeam.RED: return Team.RED;
                case EnumTeam.BLUE: return Team.BLUE;
                case EnumTeam.YELLOW: return Team.YELLOW;
                case EnumTeam.GREEN: return Team.GREEN;
                default: return Team.NONE;
            }
        }

        public class TeamNone : Team {
            public TeamNone() : base(0, "None", Color.white) { }

            public override void join(SidedObjectBase obj) {
                // Don't keep track of members.
            }

            public override void leave(SidedObjectBase obj) {
                // Don't keep track of members.
            }
        }
    }
}
