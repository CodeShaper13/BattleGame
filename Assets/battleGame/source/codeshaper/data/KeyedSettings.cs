using codeshaper.util;
using System.Collections.Generic;
using UnityEngine;

namespace codeshaper.data {

    public class KeyedSettings {

        private readonly Dictionary<string, object> dict;

        public KeyedSettings(TextAsset textAsset) {
            List<string> list = FileUtils.readTextAsset(textAsset);
            this.dict = new Dictionary<string, object>();

            string[] s1;
            string key, value;
            foreach (string s in list) {
                s1 = s.Split('=');
                key = s1[0];
                value = s1[1];

                float f;
                if (float.TryParse(value, out f)) {
                    if ((int)f == f) {
                        this.dict.Add(key, (int)f);
                    }
                    else {
                        this.dict.Add(key, f);
                    }
                    continue;
                }
                bool flag;
                if (bool.TryParse(value, out flag)) {
                    this.dict.Add(key, flag);
                    continue;
                }
                this.dict.Add(key, value);
            }
        }

        public float getFloat(string key) {
            return (float)this.dict[key];
        }

        public int getInt(string key) {
            return (int)this.dict[key];
        }

        public bool getBool(string key) {
            return (bool)this.dict[key];
        }

        public string getString(string key) {
            return (string)this.dict[key];
        }
    }
}
