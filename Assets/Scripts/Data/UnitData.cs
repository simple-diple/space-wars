using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Unit", fileName = "Unit", order = 0)]
    public class UnitData : ScriptableObject
    {
        public string id;
        public string unitName;
        public int health;
        public ShieldData shield;
        public WeaponData[] weapons;
        public ModuleData[] modules;
    }
}
