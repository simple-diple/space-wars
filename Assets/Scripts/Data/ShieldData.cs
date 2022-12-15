using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Shield", fileName = "Shield", order = 0)]
    public class ShieldData : ScriptableObject
    {
        public int energy;
        public int recoveryPerSecond;
        public int recoveryAmount = 1;
    }
}