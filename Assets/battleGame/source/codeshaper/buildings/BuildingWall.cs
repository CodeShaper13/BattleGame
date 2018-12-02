using System;
using fNbt;
using codeshaper.data;
using codeshaper.util;
using UnityEngine;
using cakeslice;
using codeshaper.util.outline;
using codeshaper.nbt;

namespace codeshaper.buildings {

    public class BuildingWall : BuildingBase {

        private WallExtension posXExtension;
        private WallExtension posZExtension;
        private WallExtension negXExtension;
        private WallExtension negZExtension;

        private BoxCollider xCollider;
        private BoxCollider zCollider;

        protected override void onAwake() {
            base.onAwake();

            BoxCollider[] boxes = this.GetComponents<BoxCollider>();
            this.xCollider = boxes[0];
            this.zCollider = boxes[1];

            this.posXExtension = new WallExtension(this, "posXExtension", Vector3.right);
            this.posZExtension = new WallExtension(this, "posZExtension", Vector3.forward);
            this.negXExtension = new WallExtension(this, "negXExtension", Vector3.left);
            this.negZExtension = new WallExtension(this, "negZExtension", Vector3.back);
        }

        protected override void onStart() {
            base.onStart();

            this.updateSelfWalls();
            this.updateNeighborWalls();
        }

        public override void onDeathCallback() {
            base.onDeathCallback();

            this.gameObject.SetActive(false); // Stops rays from hitting this and breaking stuff.
            this.updateNeighborWalls();
        }

        private void updateSelfWalls() {
            this.posXExtension.updateState();
            this.negXExtension.updateState();
            this.posZExtension.updateState();
            this.negZExtension.updateState();
        }

        private void updateNeighborWalls() {
            this.findAndUpdateNeightbor(Vector3.right);
            this.findAndUpdateNeightbor(Vector3.forward);
            this.findAndUpdateNeightbor(Vector3.left);
            this.findAndUpdateNeightbor(Vector3.back);
        }

        private void findAndUpdateNeightbor(Vector3 v) {
            BuildingWall wall = BuildingWall.getWall(this.getOrgin(), v);
            if (wall != null) {
                wall.updateSelfWalls();
            }
        }

        public Vector3 getOrgin() {
            return this.transform.position + (Vector3.up / 2);
        }

        public override float getHealthBarHeight() {
            return 2f;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }

        public override BuildingData getData() {
            return Constants.BD_WALL;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.posXExtension.read(tag);
            this.posZExtension.read(tag);
            this.negXExtension.read(tag);
            this.negZExtension.read(tag);

            this.updateColliders();
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            this.posXExtension.write(tag);
            this.posZExtension.write(tag);
            this.negXExtension.write(tag);
            this.negZExtension.write(tag);
        }

        public override void setOutlineVisibility(bool visible, EnumOutlineParam type) {
            base.setOutlineVisibility(visible, type);
            // If the piece does not exist the method will prevent anything bad from happening.
            this.posXExtension.setOutlineVisibility(visible, type);
            this.posZExtension.setOutlineVisibility(visible, type);
            this.negXExtension.setOutlineVisibility(visible, type);
            this.negZExtension.setOutlineVisibility(visible, type);
        }

        /// <summary>
        /// Updates the colliders and only the colliders for the wall.
        /// </summary>
        private void updateColliders() {
            float f = 0.25f;
            float g = 0.25f;


            float xSize = 0.5f;
            float xPos = 0f;

            if(this.posXExtension.exists()) {
                xSize += f;
                xPos += g;
            }
            if(this.negXExtension.exists()) {
                xSize += f;
                xPos -= g;
            }
            this.xCollider.center = this.xCollider.center.setX(xPos);
            this.xCollider.size = this.xCollider.size.setX(xSize);

            float zSize = 0.5f;
            float zPos = 0f;

            if (this.posZExtension.exists()) {
                zSize += f;
                zPos += g;
            }
            if (this.negZExtension.exists()) {
                zSize += f;
                zPos -= g;
            }
            this.zCollider.center = this.zCollider.center.setZ(zPos);
            this.zCollider.size = this.zCollider.size.setZ(zSize);
        }

        private class WallExtension : IHasOutline {

            private bool isThere;
            private GameObject gameObj;
            private string saveName;
            private Vector3 vector;
            private BuildingWall parentWall;
            private OutlineHelper outlineHelper;

            public WallExtension(BuildingWall parentWall, string saveName, Vector3 vec) {
                this.parentWall = parentWall;
                this.saveName = saveName;
                this.vector = vec;
            }

            public void read(NbtCompound tag) {
                this.isThere = tag.getByte(this.saveName) == 1;
                if(this.isThere) {
                    this.add();
                }
            }

            public void write(NbtCompound tag) {
                tag.setTag(this.saveName, isThere ? 1 : 0);
            }

            public bool exists() {
                return this.isThere;
            }

            public void updateState() {
                if(BuildingWall.getWall(this.parentWall.getOrgin(), this.vector) != null) {
                    this.add();
                } else {
                    this.remove();
                }
            }


            public void setOutlineVisibility(bool visible, EnumOutlineParam type) {
                if(this.isThere) {
                    this.outlineHelper.updateOutline(visible, type);
                }
            }

            /// <summary>
            /// Removes this wall extension, destroying the GameOBject.
            /// </summary>
            private void remove() {
                this.isThere = false;
                GameObject.Destroy(this.gameObj);
            }

            private void add() {
                if(!this.isThere) {
                    this.isThere = true;
                    this.gameObj = GameObject.Instantiate(References.list.wallJoinPiece);
                    this.gameObj.transform.parent = this.parentWall.transform;
                    this.gameObj.transform.position = this.parentWall.transform.position + (this.vector * 0.25f);
                    if(this.vector.x == 0) {
                        this.gameObj.transform.eulerAngles = new Vector3(0, 90, 0);
                    }
                    this.outlineHelper = new OutlineHelper(this.gameObj);
                }
                else {
                    print("add() called on an extension that already exists!");
                }
            }
        }

        private static BuildingWall getWall(Vector3 orgin, Vector3 direction) {
            RaycastHit hit;
            //Debug.DrawRay(orgin + (direction * 0.55f), direction * 0.55f, UnityEngine.Random.ColorHSV(), 100);
            if (Physics.Raycast(new Ray(orgin + (direction * 0.55f), direction), out hit, 0.45f, Layers.WALL)) {
                // There is a wall new to this one, add an extension to this one.
                BuildingWall wall = hit.transform.GetComponent<BuildingWall>();
                if(wall == null) {
                    throw new Exception("GameObject with layer \"Wall\" found without a BuildingWall component!");
                }
                return wall;
            } else {
                return null;
            }
        }
    }
}