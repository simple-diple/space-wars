using System;
using UnityEngine;
using View;

namespace Data
{
    [Serializable]
    public class UnitPrefab : PrefabData
    {
        public UnitView prefab;
        public override GameObjectView Prefab => prefab;
    }
}