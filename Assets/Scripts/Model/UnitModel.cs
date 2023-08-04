using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using View;
using Object = UnityEngine.Object;

namespace Model
{
    public class UnitModel : IUnit
    {
        public int Player { get; }

        public float ShieldEnergy => _shieldModel.Energy;
        public float ShieldRecovery => _shieldModel.Recovery;
        public float Health
        {
            get => _health + GetEffect(BuffEffect.Health);
            private set => SetHealth(value);
        }
        public bool IsAlive => Health > 0;
        public Dictionary<int, IWeapon> Weapons => _weaponModels;
        public Dictionary<int, IModule> Modules => _moduleModels;

        private readonly UnitView _view;
        private float _health;
        private readonly Dictionary<int, IWeapon> _weaponModels;
        private readonly Dictionary<int, IModule> _moduleModels;
        private readonly ShieldModel _shieldModel;
        private readonly float _maxHealth;
        private readonly GameData _gameData;
        private readonly Dictionary<int, List<Tuple<BuffEffect, float>>> _modulesEffects;
        private Dictionary<BuffEffect, float> _allEffects;

        public event Action<UnitModel> OnDie;
        public event Action<IUnit> OnStatsChange;

        public UnitModel(UnitData unitData, Transform spawnPoint, int player, GameData gameData)
        {
            _gameData = gameData;
            
            var (success, reason) = Validate(unitData);
            if (success == false)
            {
                throw new Exception($"Can not spawn unit {unitData.id}! {reason}");
            }

            var prefab = gameData.GetPrefab<UnitView>(unitData.id);
            _view = Object.Instantiate(prefab, spawnPoint, true);
            Transform transform = _view.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            _maxHealth = unitData.health;
            _health = unitData.health;
            Player = player;
            _shieldModel = new ShieldModel(this, _view.shieldView, unitData.shield);
            _shieldModel.OnEnergyChanged += OnShieldEnergyChanged;
            _weaponModels = new Dictionary<int, IWeapon>(unitData.weapons.Length);
            _moduleModels = new Dictionary<int, IModule>(unitData.modules.Length);
            _modulesEffects = new Dictionary<int, List<Tuple<BuffEffect, float>>>(unitData.modules.Length);
            _allEffects = CreateEmptyEffects();

            for (var i = 0; i < unitData.weapons.Length; i++)
            {
                ConnectSlot(unitData.weapons[i], i);
            }
            
            for (var i = 0; i < unitData.modules.Length; i++)
            {
                ConnectSlot(unitData.modules[i], i);
            }

            _view.Connect(this);
            
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
            OnStatsChange?.Invoke(this);
        }
        
        public void Fire()
        {
            foreach (var weaponModel in _weaponModels.Values)
            {
                weaponModel.Fire(true);
            }
        }
        
        public void StopFire()
        {
            foreach (var weapon in _weaponModels.Values)
            {
                weapon.Fire(false);
            }
        }
        
        public float GetEffect(BuffEffect effect)
        {
            return _allEffects[effect];
        }

        private Dictionary<BuffEffect, float> CreateEmptyEffects()
        {
            var effects = Enum.GetValues(typeof(BuffEffect));
            var result  = new Dictionary<BuffEffect, float>(effects.Length);
            foreach (var effect in effects)
            {
                result.Add((BuffEffect)effect, 0);
            }

            return result;
        }

        private void OnShieldEnergyChanged()
        {
            OnStatsChange?.Invoke(this);
        }

        private void SetHealth(float value)
        {
            float effect = GetEffect(BuffEffect.Health);
            _health = Mathf.Clamp(value - effect, -effect, _maxHealth);
            
            if (Health == 0)
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

        public void SetShieldCollider(bool value)
        {
            _shieldModel.SetShieldCollider(value);
        }
        
        public void ConnectSlot(ModuleData moduleData, int index)
        {
            bool moduleExist = _moduleModels.ContainsKey(index);

            if (moduleExist)
            {
                _moduleModels[index].Disconnect();
                _moduleModels.Remove(index);
                _view.moduleSlots[index].DisposeModule();
            }
            
            if (moduleData == null)
            {
                OnStatsChange?.Invoke(this);
                return;
            }

            var slot = _view.moduleSlots[index];
            var module = _gameData.GetPrefab<ModuleView>(moduleData.id);
            var moduleView = Object.Instantiate(module, slot.spawnModuleAnchor, true);
            var transform = moduleView.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            slot.moduleView = moduleView;

            ModuleModel moduleModel = new ModuleModel(moduleView, moduleData, this, index);
            moduleModel.Connect();
            _moduleModels.Add(index, moduleModel);
            OnStatsChange?.Invoke(this);
        }

        public void ConnectSlot(WeaponData weaponData, int index)
        {
            bool weaponExist = _weaponModels.ContainsKey(index);

            if (weaponExist)
            {
                _view.weaponSlots[index].DisposeWeapon();
                _weaponModels.Remove(index);
            }

            if (weaponData == null)
            {
                OnStatsChange?.Invoke(this);
                return;
            }
            
            WeaponSlotView slot = _view.weaponSlots[index];
            WeaponView weaponView = _gameData.GetPrefab<WeaponView>(weaponData.id);
            var newWeapon = Object.Instantiate(weaponView, slot.spawnWeaponAnchor, true);
            newWeapon.bulletView = _gameData.GetPrefab<BulletView>(weaponData.bulletId);
            var transform = newWeapon.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            slot.weaponView = newWeapon;
            slot.weaponView.gameObject.SetActive(true);
            var newModel = new WeaponModel(newWeapon, weaponData, this);
            _weaponModels.Add(index, newModel);

            OnStatsChange?.Invoke(this);
        }

        public void AddModuleEffect(int slotIndex, List<Tuple<BuffEffect, float>> buffEffect)
        {
            if (_modulesEffects.ContainsKey(slotIndex) == false)
            {
                _modulesEffects.Add(slotIndex, buffEffect);
            }
            else
            {
                _modulesEffects[slotIndex] = buffEffect;
            }

            UpdateEffects();
        }
        
        public void RemoveModuleEffect(int slotIndex)
        {
            if (_modulesEffects.ContainsKey(slotIndex) == false)
            {
                return;
            }

            _modulesEffects.Remove(slotIndex);
            UpdateEffects();
        }

        private void UpdateEffects()
        {
            _allEffects = CreateEmptyEffects();
            foreach (List<Tuple<BuffEffect, float>> effects in _modulesEffects.Values)
            {
                foreach (Tuple<BuffEffect, float> effect in effects)
                {
                    _allEffects[effect.Item1] += effect.Item2;
                }
            }
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