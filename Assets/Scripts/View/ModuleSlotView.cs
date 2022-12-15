using Model;
using UnityEngine;
using View.UI;

namespace View
{
    public class ModuleSlotView : GameObjectView
    {
        public int slotIndex;
        public UnitModel unitModel;
        public Transform spawnModuleAnchor;
        public ModuleView moduleView;

        private void OnMouseDown()
        {
            UISetSlot.ModuleSlotClick(this);
        }
    }
}