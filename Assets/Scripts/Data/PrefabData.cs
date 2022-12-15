using System;
using UnityEngine;
using View;

namespace Data
{
    [Serializable]
    public abstract class PrefabData
    {
        public string id;
        public abstract GameObjectView Prefab { get; }
    }
}