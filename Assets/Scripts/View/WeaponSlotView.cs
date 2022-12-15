using Model;
using UnityEngine;
using View.UI;

namespace View
{
    public class WeaponSlotView : GameObjectView
    {
        public int slotIndex;
        public Transform spawnWeaponAnchor;
        public WeaponView weaponView;
        public UnitModel unitModel;
        
        private void OnMouseDown()
        {
            UISetSlot.WeaponSlotClick(this);
        }
    }
}