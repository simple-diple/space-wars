using System.Collections.Generic;
using Controller;
using Data;
using TMPro;
using UnityEngine;

namespace View.UI
{
    public class UISetSlot : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private TMP_Dropdown weaponsDropDown;
        [SerializeField] private TMP_Dropdown modulesDropDown;
        [SerializeField] private GameController gameController;

        private static UISetSlot instance;
        private List<ModuleData> _modules;
        private List<WeaponData> _weapons;
        private GameObjectView _currentSlot;

        private void Awake()
        {
            instance = this;

            _modules = gameController.GameData.GetModules();
            _weapons = gameController.GameData.GetWeapons();
            
            modulesDropDown.options.Add(new TMP_Dropdown.OptionData() { text = "Chose module:" });
            weaponsDropDown.options.Add(new TMP_Dropdown.OptionData() { text = "Chose weapon:" });
            modulesDropDown.options.Add(new TMP_Dropdown.OptionData() { text = "Empty" });
            weaponsDropDown.options.Add(new TMP_Dropdown.OptionData() { text = "Empty" });
            
            foreach (ModuleData moduleData in _modules)
            {
                var optionData = new TMP_Dropdown.OptionData();
                optionData.text = moduleData.ToString();
                modulesDropDown.options.Add(optionData);
            }
            
            foreach (WeaponData moduleData in _weapons)
            {
                var optionData = new TMP_Dropdown.OptionData();
                optionData.text = moduleData.ToString();
                weaponsDropDown.options.Add(optionData);
            }
            
            weaponsDropDown.onValueChanged.AddListener(OnWeaponSelected);
            modulesDropDown.onValueChanged.AddListener(OnModuleSelected);
        }

        private void OnModuleSelected(int index)
        {
            if (!_currentSlot)
            {
                return;
            }
            
            ModuleData moduleData = index > 1 ? _modules[index - 2] : null;
            var slot = (ModuleSlotView)_currentSlot;
            slot.unitModel.ConnectSlot(moduleData, slot.slotIndex);
            modulesDropDown.gameObject.SetActive(false);
            _currentSlot = null;
            modulesDropDown.value = 0;
        }

        private void OnWeaponSelected(int index)
        {
            if (!_currentSlot)
            {
                return;
            }
            
            WeaponData moduleData = index > 1 ? _weapons[index - 2] : null;
            var slot = (WeaponSlotView)_currentSlot;
            slot.unitModel.ConnectSlot(moduleData, slot.slotIndex);
            weaponsDropDown.gameObject.SetActive(false);
            _currentSlot = null;
            weaponsDropDown.value = 0;
        }

        public static void WeaponSlotClick(WeaponSlotView view)
        {
            instance.WeaponSlotClickMethod(view);
        }

        public static void ModuleSlotClick(ModuleSlotView view)
        {
            instance.ModuleSlotClickMethod(view);
        }

        private void WeaponSlotClickMethod(WeaponSlotView view)
        {
            if (weaponsDropDown.IsActive() || modulesDropDown.IsActive())
            {
                return;
            }
            
            _currentSlot = view;
            modulesDropDown.gameObject.SetActive(false);
            Vector2 screenPoint = cam.WorldToScreenPoint(view.transform.position);
            weaponsDropDown.GetComponent<RectTransform>().position = screenPoint;
            weaponsDropDown.gameObject.SetActive(true);
        }
        
        private void ModuleSlotClickMethod(ModuleSlotView view)
        {
            if (weaponsDropDown.IsActive() || modulesDropDown.IsActive())
            {
                return;
            }
            
            _currentSlot = view;
            weaponsDropDown.gameObject.SetActive(false);
            Vector2 screenPoint = cam.WorldToScreenPoint(view.transform.position);
            modulesDropDown.GetComponent<RectTransform>().position = screenPoint;
            modulesDropDown.gameObject.SetActive(true);
        }
    }
}