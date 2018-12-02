using codeshaper.map.chunk;
using codeshaper.map.chunkLoader;
using codeshaper.util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace codeshaper.map {

    public class ChunkLoader {

        protected Map map;
        protected Queue<NewChunkInstructions> buildQueue;
        protected int loadRadius;

        private int maxBuildPerLoop = 4;
        private CameraMover cameraMover;
        private ChunkPos previousOccupiedChunkPos;
        private Queue<Chunk> cachedUnusedChunks;

        public ChunkLoader(Map map, CameraMover player, int loadRadius) {
            this.map = map;
            this.cameraMover = player;
            this.loadRadius = loadRadius;
            this.buildQueue = new Queue<NewChunkInstructions>();
            this.cachedUnusedChunks = new Queue<Chunk>();

            this.loadChunks(this.getOccupiedChunkPos(this.cameraMover.getPosition()));

            // Generate all of the chunks around the player.
            this.generateChunksFromInstructions(-1);

            // Render all of the chunks right away.
            //foreach (Chunk c in this.map.loadedChunks.Values) {
            //    if (!c.isReadOnly) {
            //        c.renderChunk();
            //    }
            //}
        }

        /// <summary>
        /// Called by the player to update the chunk loader.
        /// </summary>
        public void updateChunkLoader() {
            ChunkPos playerPos = this.getOccupiedChunkPos(this.cameraMover.getPosition());
            if (!(playerPos.Equals(this.previousOccupiedChunkPos))) {
                this.loadChunks(playerPos);
            }
            this.previousOccupiedChunkPos = playerPos;

            this.unloadChunks(playerPos);
            this.generateChunksFromInstructions(this.maxBuildPerLoop);
        }

        /// <summary>
        /// Unloads all chunks that are out of bounds.
        /// </summary>
        protected void unloadChunks(ChunkPos occupiedChunkPos) {
            Queue<Chunk> removals = new Queue<Chunk>();
            foreach (Chunk chunk in this.map.loadedChunks.Values) {
                if (this.isOutOfBounds(occupiedChunkPos, chunk)) {
                    removals.Enqueue(chunk);
                }
            }
            foreach (Chunk chunk in removals) {
                this.map.unloadChunk(chunk);
                chunk.gameObject.SetActive(false);
                chunk.gameObject.name = "WAITING...";
                chunk.resetChunk();
                this.cachedUnusedChunks.Enqueue(chunk);
            }
        }

        /// <summary>
        /// Creates new chunk instructions for all the chunks around the player and adds them to the list.
        /// </summary>
        protected virtual void loadChunks(ChunkPos occupiedChunkPos) {
            int x, z;
            bool flagX, flagZ;
            for (x = -this.loadRadius; x <= this.loadRadius; x++) {
                for (z = -this.loadRadius; z <= this.loadRadius; z++) {
                    flagX = (x == loadRadius || x == -loadRadius);
                    flagZ = (z == loadRadius || z == -loadRadius);
                    if (!(flagX && flagZ)) {
                        this.loadNewChunk(occupiedChunkPos, x, z, flagX || flagZ);
                    }
                }
            }
        }

        protected bool toFar(float occupiedChunkPos, float questionableChunkPos) {
            return (Math.Abs(occupiedChunkPos - questionableChunkPos) > this.loadRadius);
        }

        protected virtual bool isOutOfBounds(ChunkPos occupiedChunkPos, Chunk chunk) {
            if (this.toFar(occupiedChunkPos.x, chunk.chunkPos.x) || this.toFar(occupiedChunkPos.z, chunk.chunkPos.z)) {
                return true;
            }
            return false;
        }

        protected void loadNewChunk(ChunkPos occupiedChunkPos, int x, int z, bool isReadOnly) {
            NewChunkInstructions instructions = new NewChunkInstructions(x + occupiedChunkPos.x, z + occupiedChunkPos.z, isReadOnly);
            Chunk chunk = this.map.getChunk(instructions.chunkPos);

            if (chunk == null) {
                if (!this.buildQueue.Contains(instructions)) {
                    this.buildQueue.Enqueue(instructions);
                }
            }
            else {
                //chunk.isReadOnly = isReadOnly;
            }
        }
        /// <summary>
        /// Returns the position of the chunk the player is in.
        /// </summary>
        private ChunkPos getOccupiedChunkPos(Vector3 playerPos) {
            return new ChunkPos(
                MathHelper.floor(playerPos.x / Chunk.SIZE),
                MathHelper.floor(playerPos.z / Chunk.SIZE));
        }

        /// <summary>
        /// Generates up to the passed number of chunks, or infinite if -1.  Then returns the total number generated.
        /// </summary>
        private int generateChunksFromInstructions(int max) {
            int builtChunks = 0;
            while (this.buildQueue.Count > 0 && (builtChunks < max || max == -1)) {
                NewChunkInstructions instructions = this.buildQueue.Dequeue();
                Chunk chunk;
                if (this.cachedUnusedChunks.Count > 0) {
                    // Pull out and reset an old chunk.
                    chunk = this.cachedUnusedChunks.Dequeue();
                    chunk.gameObject.SetActive(true);
                    //chunk.filter.mesh.Clear(); // TODO Large performance hit?
                }
                else {
                    // The cache queue is empty, create a new chunk from scratch.
                    chunk = GameObject.Instantiate(References.list.chunkPrefab, this.map.holderChunk).GetComponent<Chunk>();
                }

                chunk.transform.position = instructions.chunkPos.toWorldSpaceVector();

                this.map.loadChunk(chunk, instructions);

                builtChunks++;
            }
            return builtChunks;
        }
    }
}
