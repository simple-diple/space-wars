using Model;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Buff/ShieldRecovery", fileName = "ShieldRecoverBuff", order = 0)]
    public class ShieldRecoverBuffData : BuffData
    {
        public override void Connect(UnitModel unitModel)
        {
            unitModel.UpgradeShieldRecoverySpeed(value);
        }

        public override void Disconnect(UnitModel unitModel)
        {
            unitModel.UpgradeShieldRecoverySpeed(-value);
        }
    }
}