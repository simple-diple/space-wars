using System;
using UnityEngine;
using View;

namespace Data
{
    [Serializable]
    public class WeaponPrefab : PrefabData
    {
        public WeaponView prefab;
        public override GameObjectView Prefab => prefab;
    }
}