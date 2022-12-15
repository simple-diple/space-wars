using System;
using View;

namespace Data
{
    [Serializable]
    public class ModulePrefab : PrefabData
    {
        public ModuleView prefab;
        public override GameObjectView Prefab => prefab;
    }
}