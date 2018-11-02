using codeshaper.entity;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace codeshaper.util {

    public static class Util {

        /// <summary>
        /// Returns true if the passed LivingObject is both not null and alive.
        /// </summary>
        public static bool isAlive(LivingObject obj) {
            return obj != null && !obj.isDead();
        }

        /// <summary>
        /// Returns the closest map object to the passed point.
        /// </summary>
        public static T closestToPoint<T>(Vector3 point, IEnumerable<T> list) where T : MapObject {
            return Util.closestToPoint<T>(point, list, null);
        }

        /// <summary>
        /// Returns the closest MapObject in the list.  this may return null if validOptionFunc
        /// returns false for every member of the list, or if the list is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="point">The point to compare the entity's point to.</param>
        /// <param name="list"></param>
        /// <param name="validOptionFunc">If not null, this function is called on every entity
        /// in the list.  If this function returns false the entity is not considered to be in
        /// the running for the closest.</param>
        /// <returns></returns>
        public static T closestToPoint<T>(Vector3 point, IEnumerable<T> list, Func<T, bool> validOptionFunc) where T : MapObject {
            T bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            foreach (T obj in list) {
                Vector3 directionToTarget = obj.getPos() - point;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr) {
                    if(validOptionFunc != null && !validOptionFunc(obj)) {
                        continue;
                    }
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = obj;
                }
            }

            return bestTarget;
        }

        /// <summary>
        /// Returns true if a save game exists.
        /// </summary>
        public static bool doesSaveExists() {
            return Directory.Exists(Main.SAVE_PATH);
        }
    }
}
