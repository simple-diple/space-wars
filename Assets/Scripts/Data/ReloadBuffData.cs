using Model;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Buff/Reload", fileName = "ReloadBuff", order = 0)]
    public class ReloadBuffData : BuffData
    {
        public override void Connect(UnitModel unitModel)
        {
            unitModel.UpgradeReload(value);
        }

        public override void Disconnect(UnitModel unitModel)
        {
            unitModel.UpgradeReload(-value);
        }
    }
}