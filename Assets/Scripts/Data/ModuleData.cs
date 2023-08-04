using JetBrains.Annotations;
using Model;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Module", fileName = "Module", order = 0)]
    [CanBeNull]
    public class ModuleData : ScriptableObject
    {
        public string id;
        public string moduleName;
        public BuffData[] buffs;

        public override string ToString()
        {
            string effects = string.Empty;
            foreach (BuffData buffData in buffs)
            {
                string value = buffData.value > 0 ? "+" : "";
                value += buffData.value % 1 == 0 ? buffData.value.ToString() : (buffData.value * 100).ToString() + "%";
                effects += string.Format(buffData.buffName, value + " ");
            }

            return $"{moduleName} ({effects})";
        }
    }
}