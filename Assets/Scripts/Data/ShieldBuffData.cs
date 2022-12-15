using Model;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Buff/Shield", fileName = "ShieldBuff", order = 0)]
    public class ShieldBuffData : BuffData
    {
        public override void Connect(UnitModel unitModel)
        {
            unitModel.AddShieldEnergy(value);
        }

        public override void Disconnect(UnitModel unitModel)
        {
            unitModel.AddShieldEnergy(-value);
        }
    }
}