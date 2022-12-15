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

        public void Connect(UnitModel unitModel)
        {
            foreach (BuffData buff in buffs)
            {
                buff.Connect(unitModel);
            }
        }
        
        public void Disconnect(UnitModel unitModel)
        {
            foreach (BuffData buff in buffs)
            {
                buff.Disconnect(unitModel);
            }
        }

        public override string ToString()
        {
            string effects = string.Empty;
            foreach (BuffData buffData in buffs)
            {
                effects += string.Format(buffData.buffName, buffData.value + " ");
            }

            return $"{moduleName} ({effects})";
        }
    }
}