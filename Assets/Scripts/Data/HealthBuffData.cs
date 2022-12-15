using Model;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Buff/Health", fileName = "HealthBuff", order = 0)]
    public class HealthBuffData : BuffData
    {
        public override void Connect(UnitModel unitModel)
        {
            unitModel.GiveHealth(value);
        }

        public override void Disconnect(UnitModel unitModel)
        {
            unitModel.GiveHealth(-value);
        }
    }
}