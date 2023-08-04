using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Buff", fileName = "Buff", order = 0)]
    public class BuffData : ScriptableObject
    {
        public string buffName;
        public BuffEffect buffEffect;
        public float value;
    }
}