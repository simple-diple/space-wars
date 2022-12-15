using System.Collections.Generic;
using Model;
using Unity.VisualScripting;
using UnityEngine;

namespace View
{
    public class UnitView : GameObjectView
    {
        public UnitModel UnitModel => _unitModel;
        
        [SerializeField] public List<WeaponSlotView> weaponSlots;
        [SerializeField] public List<ModuleSlotView> moduleSlots;
        [SerializeField] public ShieldView shieldView;

        private UnitModel _unitModel;

        public void Connect(UnitModel unitModel)
        {
            _unitModel = unitModel;
            var colliders = gameObject.GetComponentsInChildren<Collider>();

            foreach (Collider c in colliders)
            {
                c.AddComponent<UnitCollider>().unit = unitModel;
                c.gameObject.layer = LayerMask.NameToLayer("Player" + unitModel.Player);
            }

            for (var i = 0; i < weaponSlots.Count; i++)
            {
                var weaponSlotView = weaponSlots[i];
                weaponSlotView.unitModel = unitModel;
                weaponSlotView.slotIndex = i;
            }

            for (var i = 0; i < moduleSlots.Count; i++)
            {
                var moduleSlotView = moduleSlots[i];
                moduleSlotView.unitModel = unitModel;
                moduleSlotView.slotIndex = i;
            }
        }

        public void ShowDamage()
        {
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}