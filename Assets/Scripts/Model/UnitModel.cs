using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using View;
using Object = UnityEngine.Object;

namespace Model
{
    public class UnitModel
    {
        public int Player => _player;
        public float ShieldEnergy => _shieldModel.Energy;
        public float ShieldRecovery => _shieldModel.Recovery;
        public float Health
        {
            get => _health;
            private set => SetHealth(value);
        }

        public bool IsAlive => _health > 0;
        public List<WeaponModel> Weapons => _weaponModels.Values.ToList();
        public List<ModuleModel> Modules => _moduleModels.Values.ToList();

        private readonly UnitView _view;
        private float _health;
        
        private readonly Dictionary<int, WeaponModel> _weaponModels;
        private readonly Dictionary<int, ModuleModel> _moduleModels;
        private readonly ShieldModel _shieldModel;
        private readonly int _player;
        private float _maxHealth;
        private readonly GameData _gameData;
        
        public event Action<UnitModel> OnDie;
        public event Action<UnitModel> OnStatsChange;
        public UnitModel(UnitView view, UnitData unitData, int player, GameData gameData)
        {
            _view = view;
            _maxHealth = unitData.health;
            _health = unitData.health;
            _player = player;
            _gameData = gameData;

            _shieldModel = new ShieldModel(this, view.shieldView, unitData.shield);
            _shieldModel.OnEnergyChanged += OnShieldEnergyChanged;

            _weaponModels = new Dictionary<int, WeaponModel>(unitData.weapons.Length);
            for (var i = 0; i < unitData.weapons.Length; i++)
            {
                WeaponData weaponData = unitData.weapons[i];
                WeaponView weaponView = view.weaponSlots[i].weaponView;
                if (weaponView)
                {
                    _weaponModels.Add(i, new WeaponModel(weaponView, weaponData, _player));
                }
            }

            _moduleModels = new Dictionary<int, ModuleModel>(unitData.modules.Length);
            for (var i = 0; i < unitData.modules.Length; i++)
            {
                ModuleData moduleData = unitData.modules[i];
                ModuleView moduleView = view.moduleSlots[i].moduleView;
                
                if (moduleData != null)
                {
                    ModuleModel moduleModel = new ModuleModel(moduleView, moduleData);
                    moduleModel.Connect(this);
                    _moduleModels.Add(i, moduleModel);
                }
            }

            _view.Connect(this);
            
        }

        private void OnShieldEnergyChanged()
        {
            OnStatsChange?.Invoke(this);
        }

        public void GetDamage(int value)
        {
            Debug.Log("Get damage " + value);
            
            if (_shieldModel.Energy - value >= 0)
            {
                _shieldModel.Energy -= value;
                OnStatsChange?.Invoke(this);
                return;
            }

            Health -= value - _shieldModel.Energy;
            _shieldModel.Energy = 0;
            _view.ShowDamage();
        }

        public void GiveHealth(float value)
        {
            _maxHealth += (int)value;
            _health += (int)value;
        }

        public void UpgradeReload(float value)
        {
            foreach (WeaponModel weaponModel in _weaponModels.Values)
            {
                weaponModel.UpgradeReload(value);
            }
        }

        public void AddShieldEnergy(float value)
        {
            _shieldModel.AddMaxEnergy(value);
        }

        public void RecoverShield()
        {
            _shieldModel.Recover();
        }

        public void UpgradeShieldRecoverySpeed(float modifier)
        {
            _shieldModel.AddRecoverySpeed(modifier);
        }

        public void Fire()
        {
            foreach (WeaponModel weaponModel in _weaponModels.Values)
            {
                weaponModel.Fire();
            }
        }
        
        private void SetHealth(float value)
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
            
            if (_health == 0)
            {
                Die();
            }
            
            OnStatsChange?.Invoke(this);
        }

        private void Die()
        {
            _view.Dispose();
            _weaponModels.Clear();
            _moduleModels.Clear();
            OnDie?.Invoke(this);
        }

        public void StopFire()
        {
            foreach (var weapon in _weaponModels.Values)
            {
                weapon.StopFire();
            }
        }

        public void SetShieldCollider(bool value)
        {
            _shieldModel.SetShieldCollider(value);
        }

        public void ConnectSlot(WeaponData weaponData, int index)
        {
            if (weaponData == null && _weaponModels.ContainsKey(index) == false)
            {
                return;
            }
            
            if (weaponData == null && _weaponModels.ContainsKey(index))
            {
                _weaponModels.Remove(index);
                Object.Destroy(_view.weaponSlots[index].weaponView.gameObject);
                OnStatsChange?.Invoke(this);
                return;
            }

            if (_weaponModels.ContainsKey(index))
            {
                Object.Destroy(_view.weaponSlots[index].weaponView.gameObject);
            }
            
            WeaponSlotView slot = _view.weaponSlots[index];
            WeaponView weaponView = _gameData.GetPrefab<WeaponView>(weaponData.id);
            var newWeapon = Object.Instantiate(weaponView, slot.spawnWeaponAnchor, true);
            newWeapon.bulletView = _gameData.GetPrefab<BulletView>(weaponData.bulletId);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;
            slot.weaponView = newWeapon;
            slot.weaponView.gameObject.SetActive(true);

            var newModel = new WeaponModel(newWeapon, weaponData, _player);

            if (_weaponModels.ContainsKey(index) == false)
            {
                _weaponModels.Add(index, newModel);
            }

            _weaponModels[index] = newModel;
            OnStatsChange?.Invoke(this);
        }

        public void ConnectSlot(ModuleData moduleData, int index)
        {
            if (moduleData == null && _moduleModels.ContainsKey(index) == false)
            {
                return;
            }
            
            if (moduleData == null && _moduleModels.ContainsKey(index))
            {
                _moduleModels[index].Disconnect(this);
                _moduleModels.Remove(index);
                Object.Destroy(_view.moduleSlots[index].moduleView.gameObject);
                OnStatsChange?.Invoke(this);
                return;
            }
            
            if (_moduleModels.ContainsKey(index))
            {
                Object.Destroy(_view.moduleSlots[index].moduleView.gameObject);
            }
            
            var slot = _view.moduleSlots[index];
            var module = _gameData.GetPrefab<ModuleView>(moduleData.id);
            var moduleView = Object.Instantiate(module, slot.spawnModuleAnchor, true);
            moduleView.transform.localPosition = Vector3.zero;
            moduleView.transform.localRotation = Quaternion.identity;
            slot.moduleView = moduleView;

            if (_moduleModels.ContainsKey(index))
            {
                _moduleModels[index].Disconnect(this);
                _moduleModels.Remove(index);
            }
            
            ModuleModel moduleModel = new ModuleModel(moduleView, moduleData);
            moduleModel.Connect(this);
            _moduleModels.Add(index, moduleModel);
            OnStatsChange?.Invoke(this);
        }
    }
}