using System.Collections.Generic;
using UnityEngine;

namespace codeshaper.util {

    public static class FileUtils {

        /// <summary>
        /// Reads a Text Asset and returns the contents.  Empty
        /// line and lines starting with "#" are ignored.
        /// </summary>
        public static List<string> readTextAsset(TextAsset textAsset) {
            string[] strings = textAsset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>(strings);
            list.RemoveAll(i => i.StartsWith("#"));
            return list;
        }
    }
}
