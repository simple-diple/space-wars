using JetBrains.Annotations;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Weapon", fileName = "Weapon", order = 0)]
    [CanBeNull]
    public class WeaponData : ScriptableObject
    {
        public string id;
        public string bulletId;
        public float force;
        public string weaponName;
        public int damage;
        public float reloadTime;

        public override string ToString()
        {
            return weaponName + "(D:" + damage + " R:" + reloadTime + ")";
        }
    }
}