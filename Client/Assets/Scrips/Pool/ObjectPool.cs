using System.Collections.Generic;
using UnityEngine;

namespace ZCGame {
    public class ObjectPool<T> {
        protected virtual int MAX_COUNT { get; private set; }
        protected List<T> cacaheLists = new List<T>();
        public virtual void Init() {
            MAX_COUNT = 100;
        }

        public virtual void AddPool(T obj) {

        }

        public virtual void Update() {

        }

        public virtual void Destory() {
            for (int i = 0;i < cacaheLists.Count;i++) {
                Object.Destroy(cacaheLists[i] as Object);
            }
            cacaheLists.Clear();
        }
    }
}
    
