using codeshaper.buildings;
using codeshaper.data;
using codeshaper.entity;
using codeshaper.map;
using codeshaper.nbt;
using codeshaper.util;
using fNbt;
using System;
using UnityEngine;

namespace codeshaper.team {

    // Teams are accessed through static fields for convenience and initialized in Map.Awake()
    public class Team : INbtSerializable {

        public static Team NONE = new TeamNone(0);
        public static Team GREEN = new Team(1, "green", Color.green);
        public static Team PURPLE = new Team(2, "purple", new Color(0.40f, 0.07f, 0.54f));
        public static Team ORANGE = new Team(3, "orange", new Color(1f, 0.522f, 0.106f));
        public static Team BLUE = new Team(4, "blue", Color.blue);
        public static Team[] ALL_TEAMS = new Team[] { GREEN, PURPLE, ORANGE, BLUE };

        public readonly Predicate<MapObject> predicateThisTeam;
        public readonly Predicate<MapObject> predicateOtherTeam;

        private readonly int teamId;
        private readonly string internalName;
        private readonly string teamName;
        private readonly Color color;
        private readonly EnumTeam enumTeam;
        /// <summary> The number of resources the team has. </summary>
        private int resources;
        private Map map;

        private Team(int teamId, string name, Color color) {
            this.teamId = teamId;
            this.internalName = name;
            this.teamName = char.ToUpper(name[0]) + name.Substring(1);
            this.color = color;
            this.enumTeam = (EnumTeam)this.teamId;

            this.predicateThisTeam = (MapObject obj) => { return obj is SidedObjectEntity && ((SidedObjectEntity)obj).getTeam() == this; };
            this.predicateOtherTeam = (MapObject obj) => { return obj is SidedObjectEntity && ((SidedObjectEntity)obj).getTeam() != this; };
        }

        public void prepare(Map map) {
            this.map = map;

            this.setResources(Constants.STARTING_RESOURCES);
        }

        /// <summary>
        /// Returns the name of the team.
        /// </summary>
        public string getName() {
            return this.teamName;
        }

        public Color getColor() {
            return this.color;
        }

        public EnumTeam getEnum() {
            return this.enumTeam;
        }

        public int getTeamId() {
            return this.teamId;
        }

        /// <summary>
        /// Returns the current number of resources that this team has.
        /// </summary>
        public int getResources() {
            return this.resources;
        }

        /// <summary>
        /// Sets the Team's resources, clamping it between 0 and the maximum number the player can have.  Any overflow is discarded.
        /// </summary>
        public void setResources(int amount) {
            this.resources = Mathf.Clamp(amount, 0, this.getMaxResourceCount());
        }

        /// <summary>
        /// Reduces the Team's resources by the passed amount.
        /// </summary>
        public void reduceResources(int amount) {
            this.setResources(this.resources - amount);
        }

        public void increaseResources(int amount) {
            this.setResources(this.resources + amount);
        }

        /// <summary>
        /// Returns the maximum amount of resources that this Team can have.
        /// </summary>
        public int getMaxResourceCount() {
            int maxResources = Constants.STARTING_RESOURCE_CAP;
            foreach (SidedObjectEntity o in this.map.findMapObjects(this.predicateThisTeam)) {
                if (o is BuildingStoreroom) {
                    maxResources += Constants.BUILDING_STOREROOM_RESOURCE_BOOST;
                }
            }
            return maxResources;
        }

        /// <summary>
        /// Returns the total number of troops on this team.
        /// </summary>
        public int getTroopCount() {
            return this.map.findMapObjects(this.predicateThisTeam).Count;
        }

        /// <summary>
        /// Returns the total number of troops this team can have.
        /// </summary>
        public int getMaxTroopCount() {
            int i = Constants.STARTING_TROOP_CAP;
            foreach (SidedObjectEntity o in this.map.findMapObjects(this.predicateThisTeam)) {
                if (o is BuildingCamp) {
                    BuildingCamp camp = (BuildingCamp)o;
                    if (!camp.isConstructing()) {
                        i += Constants.BUILDING_CAMP_TROOP_BOOST;
                    }
                }
            }
            return i;
        }

        public void readFromNbt(NbtCompound tag) {
            NbtCompound tag1 = tag.getCompound(this.internalName);

            this.setResources(tag1.getInt("resources"));
        }

        public void writeToNbt(NbtCompound tag) {
            NbtCompound tag1 = new NbtCompound(this.internalName);
            tag1.setTag("resources", this.resources);

            tag.Add(tag1);
        }

        /// <summary>
        /// Returns the Team with the passed ID, or Team.None if the ID does not point to a team.
        /// </summary>
        public static Team getTeamFromId(int id) {
            foreach(Team team in Team.ALL_TEAMS) {
                if(team.teamId == id) {
                    return team;
                }
            }
            return Team.NONE;
        }

        public static Team getTeamFromEnum(EnumTeam enumTeam) {
            switch (enumTeam) {
                case EnumTeam.GREEN: return Team.GREEN;
                case EnumTeam.PURPLE: return Team.PURPLE;
                case EnumTeam.ORANGE: return Team.ORANGE;
                case EnumTeam.BLUE: return Team.BLUE;
                case EnumTeam.NONE: default: return Team.NONE;
            }
        }

        private class TeamNone : Team {

            public TeamNone(int teamId) : base(teamId, "None", Color.white) { }
        }
    }
}
