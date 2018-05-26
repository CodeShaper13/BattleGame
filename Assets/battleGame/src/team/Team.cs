using fNbt;
using src.buildings;
using src.data;
using src.entity;
using src.util;
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
        private readonly string teamName;
        private readonly Color color;
        private readonly List<SidedObjectBase> members;
        private readonly EnumTeam enumTeam;

        /// <summary> The number of resources the team has. </summary>
        private int resources;

        private Team(int id, string name, Color color, EnumTeam team) {
            this.id = id;
            this.teamName = name;
            this.color = color;
            this.enumTeam = team;
            this.members = new List<SidedObjectBase>();

            this.setResources(Constants.STARTING_RESOURCES);
        }

        /// <summary>
        /// Instantiates the team objects.  Called from Map.Awake().
        /// </summary>
        public static void initTeams() {
            Team.NONE = new TeamNone();
            Team.RED = new Team(1, "Red", Color.red, EnumTeam.RED);
            Team.BLUE = new Team(2, "Blue", Color.blue, EnumTeam.BLUE);
            Team.YELLOW = new Team(3, "Yellow", Color.yellow, EnumTeam.YELLOW);
            Team.GREEN = new Team(4, "Green", Color.green, EnumTeam.GREEN);

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
        public List<SidedObjectBase> getMembers() {
            return this.members;
        }

        public EnumTeam getEnum() {
            return this.enumTeam;
        }

        public int getTeamId() {
            return this.id;
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
            int maxResources = Constants.DEFAUT_RESOURCE_CAP;
            foreach (SidedObjectBase o in this.members) {
                if (o is BuildingStoreroom) {
                    maxResources += Constants.STOREROOM_RESOURCE_BOOST;
                }
            }
            return maxResources;
        }

        public NbtCompound write() {
            NbtCompound tag = new NbtCompound(this.teamName);

            NbtList list = new NbtList("members", NbtTagType.Compound);
            foreach (SidedObjectBase obj in this.members) {
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
                case EnumTeam.RED: return Team.RED;
                case EnumTeam.BLUE: return Team.BLUE;
                case EnumTeam.YELLOW: return Team.YELLOW;
                case EnumTeam.GREEN: return Team.GREEN;
                default: return Team.NONE;
            }
        }

        public class TeamNone : Team {
            public TeamNone() : base(0, "None", Color.white, EnumTeam.NONE) { }

            public override void join(SidedObjectBase obj) {
                // Don't keep track of members.
            }

            public override void leave(SidedObjectBase obj) {
                // Don't keep track of members.
            }
        }
    }
}
