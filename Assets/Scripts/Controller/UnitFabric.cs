using System;
using Data;
using Model;
using UnityEngine;
using View;
using Object = UnityEngine.Object;

namespace Controller
{
    public class UnitFabric
    {
        private readonly GameData _gameData;
        
        public UnitFabric(GameData gameData)
        {
            _gameData = gameData;
        }

        public UnitModel SpawnUnit(UnitData unitData, Transform transform, int player)
        {
            var (success, reason) = Validate(unitData);
            if (success == false)
            {
                throw new Exception($"Can not spawn unit {unitData.id}! {reason}");
            }
            
            var prefab = _gameData.GetPrefab<UnitView>(unitData.id);
            UnitView unitView = Object.Instantiate(prefab, transform);

            for (var i = 0; i < unitView.weaponSlots.Count; i++)
            {
                WeaponSlotView slot = unitView.weaponSlots[i];
                WeaponData weaponData = unitData.weapons[i];
                if (weaponData == null)
                {
                    continue;
                }
                WeaponView weapon = _gameData.GetPrefab<WeaponView>(weaponData.id);
                var newWeapon = Object.Instantiate(weapon, slot.spawnWeaponAnchor, true);
                newWeapon.bulletView = _gameData.GetPrefab<BulletView>(weaponData.bulletId);
                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.transform.localRotation = Quaternion.identity;
                slot.weaponView = newWeapon;
            }
            
            for (var i = 0; i < unitView.moduleSlots.Count; i++)
            {
                var slot = unitView.moduleSlots[i];
                var moduleData = unitData.modules[i];
                if (moduleData == null)
                {
                    continue;
                }
                var module = _gameData.GetPrefab<ModuleView>(moduleData.id);
                var newModule = Object.Instantiate(module, slot.spawnModuleAnchor, true);
                newModule.transform.localPosition = Vector3.zero;
                newModule.transform.localRotation = Quaternion.identity;
                slot.moduleView = newModule;
            }

            return new UnitModel(unitView, unitData, player, _gameData);
        }

        private (bool, string) Validate(UnitData unitData)
        {
            if (_gameData.HasPrefab<UnitView>(unitData.id) == false)
            {
                return (false , $"Unit prefab {unitData.id} not found");
            }

            foreach (WeaponData weapon in unitData.weapons)
            {
                if (weapon == null)
                {
                    continue;
                }
                
                if (_gameData.HasPrefab<WeaponView>(weapon.id) == false)
                {
                    return (false , $"Weapon prefab {weapon.id} not found");
                }
                
                if (_gameData.HasPrefab<BulletView>(weapon.bulletId) == false)
                {
                    return (false , $"Weapon {weapon.id} has no bullet {weapon.bulletId} prefab");
                }
            }
            
            foreach (ModuleData module in unitData.modules)
            {
                if (module == null)
                {
                    continue;
                }
                
                if (_gameData.HasPrefab<ModuleView>(module.id) == false)
                {
                    return (false , $"Module prefab {module.id} not found");
                }
            }

            if (_gameData.GetPrefab<UnitView>(unitData.id).weaponSlots.Count < unitData.weapons.Length)
            {
                return (false , $"Unit weapon slots less weapons in unit data");
            }
            
            if (_gameData.GetPrefab<UnitView>(unitData.id).moduleSlots.Count < unitData.modules.Length)
            {
                return (false , $"Unit modules slots less weapons in unit data");
            }

            return (true, string.Empty);
        }
    }
}