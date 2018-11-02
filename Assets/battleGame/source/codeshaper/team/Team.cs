using fNbt;
using codeshaper.buildings;
using codeshaper.data;
using codeshaper.entity;
using codeshaper.map;
using codeshaper.registry;
using codeshaper.util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace codeshaper.team {

    // Teams are accessed through static fields for convenience and initialized in Map.Awake()
    public class Team {

        public static Team NONE = new TeamNone(0);
        public static Team GREEN = new Team(1, "Green", Color.green, EnumTeam.GREEN);
        public static Team PURPLE = new Team(2, "Pruple", new Color(0.902f, 0.902f, 0.98f), EnumTeam.PURPLE);
        public static Team ORANGE = new Team(3, "Orange", new Color(1f, 0.522f, 0.106f), EnumTeam.ORANGE);
        public static Team BLUE = new Team(4, "Blue", Color.blue, EnumTeam.BLUE);
        public static Team[] ALL_TEAMS = new Team[] { GREEN, PURPLE, ORANGE, BLUE };

        private readonly int teamId;
        private readonly string teamName;
        private readonly Color color;
        private readonly EnumTeam enumTeam;

        private readonly List<SidedObjectEntity> members;

        /// <summary> The number of resources the team has. </summary>
        private int resources;

        private Team(int teamId, string name, Color color, EnumTeam team) {
            this.teamId = teamId;
            this.teamName = name;
            this.color = color;
            this.enumTeam = team;
            this.members = new List<SidedObjectEntity>();

            this.setResources(Constants.STARTING_RESOURCES);
        }

        /// <summary>
        /// Resets the teams for a new map by clearing their member list.  Called from Map.Awake().
        /// </summary>
        public static void resetTeams() {
            foreach(Team t in Team.ALL_TEAMS) {
                t.getMembers().Clear();
            }
        }

        /// <summary>
        /// Joins the passed object to the team, adding them to the list of members.
        /// </summary>
        public virtual void join(SidedObjectEntity obj) {
            if(this.members.Contains(obj)) {
                throw new Exception("Tried to add " + obj.name + "  to team " + this.teamName + " but it was already on the steam.");
            }
            this.members.Add(obj);
        }

        /// <summary>
        /// Removes the passed member from the team, throwing an exception if they were not on the team.
        /// </summary>
        public virtual void leave(SidedObjectEntity obj) {
            if(!this.members.Remove(obj)) {
                throw new Exception("Tried to remove " + obj.name + " from team " + this.teamName + " but it wasn't a memeber of the team!");
            }
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

        /// <summary>
        /// Returns all members on the team.
        /// </summary>
        public List<SidedObjectEntity> getMembers() {
            return this.members;
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
            foreach (SidedObjectEntity o in this.members) {
                if (o is BuildingStoreroom) {
                    maxResources += Constants.BUILDING_STOREROOM_RESOURCE_BOOST;
                }
            }
            return maxResources;
        }

        /// <summary>
        /// Returns the total number of troops on this team.
        /// </summary>
        /// <returns></returns>
        public int getTroopCount() {
            return this.members.Count;
        }

        /// <summary>
        /// Returns the total number of troops this team can have.
        /// </summary>
        public int getMaxTroopCount() {
            int i = Constants.STARTING_TROOP_CAP;
            foreach (SidedObjectEntity o in this.members) {
                if (o is BuildingCamp) {
                    BuildingCamp camp = (BuildingCamp)o;
                    if (!camp.isConstructing()) {
                        i += Constants.BUILDING_CAMP_TROOP_BOOST;
                    }
                }
            }
            return i;
        }

        public void read(Map map, NbtCompound tag) {
            NbtList list = tag.getList("members");
            foreach(NbtCompound compound in list) {
                int id = compound.getInt("id");
                RegisteredObject registeredObject = Registry.getObjectfromRegistry(id);
                map.spawnEntity(registeredObject, compound);
            }

            this.setResources(tag.getInt("resources"));
        }

        public NbtCompound write() {
            NbtCompound tag = new NbtCompound(this.teamName);

            NbtList list = new NbtList("members", NbtTagType.Compound);
            foreach (SidedObjectEntity obj in this.members) {
                NbtCompound t = new NbtCompound();
                obj.writeToNbt(t);
                list.Add(t);
            }

            tag.Add(list);
            tag.setTag("resources", this.resources);

            return tag;
        }

        public static Team teamFromEnum(EnumTeam enumTeam) {
            switch (enumTeam) {
                case EnumTeam.GREEN: return Team.GREEN;
                case EnumTeam.PURPLE: return Team.PURPLE;
                case EnumTeam.ORANGE: return Team.ORANGE;
                case EnumTeam.BLUE: return Team.BLUE;
                default: return Team.NONE;
            }
        }

        private class TeamNone : Team {

            public TeamNone(int teamId) : base(teamId, "None", Color.white, EnumTeam.NONE) { }

            public override void join(SidedObjectEntity obj) {
                // Don't keep track of members.
            }

            public override void leave(SidedObjectEntity obj) {
                // Don't keep track of members.
            }
        }
    }
}
