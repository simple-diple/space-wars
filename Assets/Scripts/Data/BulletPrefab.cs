using System;
using View;

namespace Data
{
    [Serializable]
    public class BulletPrefab : PrefabData
    {
        public BulletView prefab;
        public override GameObjectView Prefab => prefab;
    }
}