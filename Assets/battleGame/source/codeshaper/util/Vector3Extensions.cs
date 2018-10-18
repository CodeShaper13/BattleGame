using UnityEngine;

namespace codeshaper.util {

    public static class Vector3Extensions {

        public static Vector3 setX(this Vector3 vector, float f) {
            return new Vector3(f, vector.y, vector.z);
        }

        public static Vector3 setY(this Vector3 vector, float f) {
            return new Vector3(vector.x, f, vector.z);
        }

        public static Vector3 setZ(this Vector3 vector, float f) {
            return new Vector3(vector.x, vector.y, f);
        }
    }
}
