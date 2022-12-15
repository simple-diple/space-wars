using System;
using System.Collections;
using Data;
using UnityEngine;
using View;

namespace Model
{
    public class ShieldModel
    {
        public float Energy
        {
            get => _energy;
            set => SetEnergy(value);
        }

        public float Recovery => _recoveryAmount;

        public event Action OnEnergyChanged;

        private readonly ShieldView _view;
        private float _maxEnergy;
        private float _energy;
        private float _recoverySpeed;
        private float _recoveryAmount;
        private readonly UnitModel _unitModel;
        
        public ShieldModel(UnitModel unitModel, ShieldView view, ShieldData data)
        {
            _unitModel = unitModel;
            _view = view;
            _maxEnergy = data.energy;
            _energy = _maxEnergy;
            _recoverySpeed = data.recoveryPerSecond;
            _recoveryAmount = data.recoveryAmount;

            _view.StartCoroutine(RecoveryShield());
        }

        public void AddMaxEnergy(float value)
        {
            _maxEnergy += (int)value;
            Energy += (int)value;
        }

        public void AddRecoverySpeed(float modifier)
        {
            _recoveryAmount *= 1 + modifier;
        }
        
        private void SetEnergy(float value)
        {
            if (value  >= _maxEnergy)
            {
                _energy = _maxEnergy;
                OnEnergyChanged?.Invoke();
                return;
            }
            
            if (_energy <= value)
            {
                _view.ShowRecoveryEffect();
            }
            else
            {
                _view.ShowDamageEffect();
            }

            _energy = Mathf.Clamp(value, 0, _maxEnergy);
            _view.SetActive(_energy > 0);
            OnEnergyChanged?.Invoke();
        }

        private IEnumerator RecoveryShield()
        {
            yield return null;
            
            while (_unitModel.IsAlive)
            {
                yield return new WaitForSeconds(_recoverySpeed);
                Energy += _recoveryAmount;
            }
        }

        public void Recover()
        {
            Energy = _maxEnergy;
        }

        public void SetShieldCollider(bool value)
        {
            _view.SetCollider(value);
        }
    }
}