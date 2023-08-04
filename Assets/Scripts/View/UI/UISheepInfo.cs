using Model;
using TMPro;
using UnityEngine;

namespace View.UI
{
    public class UISheepInfo : MonoBehaviour
    {
        public TMP_Text hpText;
        public TMP_Text statsText;

        public void Connect(UnitModel unitModel)
        {
            unitModel.OnStatsChange += OnStatsChanged;
            OnStatsChanged(unitModel);
        }

        private void OnStatsChanged(UnitModel unitModel)
        {
            hpText.text =
                $"Shield: {unitModel.ShieldEnergy.ToString()}\n" +
                $"Health: {unitModel.Health.ToString()}";
            
            string weaponStr = unitModel.Weapons.Count > 0 ? "" : "none";
            string moduleStr = unitModel.Modules.Count > 0 ? "" : "none";

            foreach (var weapon in unitModel.Weapons)
            {
                weaponStr += $"{weapon.Value}\n";
            }
            
            foreach (var module in unitModel.Modules)
            {
                moduleStr += $"{module.Value}\n";
            }

            statsText.text = $"Shield Recovery: {unitModel.ShieldRecovery.ToString()}\n" +
                             $"Weapons:\n" + weaponStr + "\n" +
                             "Modules:\n" + moduleStr;
        }
    }
}