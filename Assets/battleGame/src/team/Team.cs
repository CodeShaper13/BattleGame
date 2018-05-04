using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.team {

    // Teams are accessed through static fields for convenience and initialized in Map.Awake()
    public class Team {

        public static Team NONE;
        public static Team RED;
        public static Team BLUE;
        public static Team YELLOW;
        public static Team GREEN;
        public static Team[] ALL_TEAMS;

        private readonly int id;
        private readonly string name;
        private readonly Color color;
        private readonly List<SidedObjectBase> members;

        private Team(int id, string name, Color color) {
            this.id = id;
            this.name = name;
            this.color = color;
            this.members = new List<SidedObjectBase>();
        }

        /// <summary>
        /// Instantiates the team objects.
        /// </summary>
        public static void initTeams() {
            Team.NONE = new TeamNone();
            Team.RED = new Team(1, "Red", Color.red);
            Team.BLUE = new Team(2, "Blue", Color.blue);
            Team.YELLOW = new Team(3, "Yellow", Color.yellow);
            Team.GREEN = new Team(4, "Green", Color.green);

            Team.ALL_TEAMS = new Team[] { RED, BLUE, YELLOW, GREEN };
        }

        /// <summary>
        /// Joins the passed object to the team, addign them to the list of members.
        /// </summary>
        public virtual void join(SidedObjectBase obj) {
            this.members.Add(obj);
        }

        /// <summary>
        /// Removes the passed member from the team, throwing an exception if they were not on the team.
        /// </summary>
        public virtual void leave(SidedObjectBase obj) {
            if(!this.members.Remove(obj)) {
                throw new Exception("Tried to remove " + obj.name + " from team " + this.name + " but it wasn't a memeber of the team!");
            }
        }

        /// <summary>
        /// Returns the name of the team.
        /// </summary>
        public string getName() {
            return this.name;
        }

        public Color getColor() {
            return this.color;
        }

        /// <summary>
        /// Returns all members on the team.
        /// </summary>
        public List<SidedObjectBase> getMembers() {
            return this.members;
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
