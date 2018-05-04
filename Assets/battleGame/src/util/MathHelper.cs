using UnityEngine;

namespace src.util {

    public static class MathHelper {

        public static bool inRange(Vector3 v1, Vector3 v2, float maxDistance) {
            return Vector3.Distance(v1, v2) <= maxDistance;

        }

        public static bool inRange(MonoBehaviour obj1, MonoBehaviour obj2, float maxDistance) {
            return MathHelper.inRange(obj1.transform.position, obj2.transform.position, maxDistance);
        }
    }
}
