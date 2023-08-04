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
            get => _energy + _unitModel.GetEffect(BuffEffect.ShieldEnergy);
            set => SetEnergy(value);
        }

        public float Recovery => _recoveryAmount * (1 + _unitModel.GetEffect(BuffEffect.ShieldRecovery));
        private float MaxEnergy => _maxEnergy + _unitModel.GetEffect(BuffEffect.ShieldEnergy);

        public event Action OnEnergyChanged;

        private readonly ShieldView _view;
        private readonly float _maxEnergy;
        private float _energy;
        private readonly float _recoverySpeed;
        private readonly float _recoveryAmount;
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

        private void SetEnergy(float value)
        {
            if (value >= MaxEnergy)
            {
                _energy = _maxEnergy;
                OnEnergyChanged?.Invoke();
                return;
            }
            
            if (Energy <= value)
            {
                _view.ShowRecoveryEffect();
            }
            else
            {
                _view.ShowDamageEffect();
            }

            float effect = _unitModel.GetEffect(BuffEffect.ShieldEnergy);
            _energy = Mathf.Clamp(value - effect, -effect, _maxEnergy);
            _view.SetActive(Energy > 0);
            OnEnergyChanged?.Invoke();
        }

        private IEnumerator RecoveryShield()
        {
            yield return null;
            
            while (_unitModel.IsAlive)
            {
                yield return new WaitForSeconds(_recoverySpeed);
                Energy += Recovery;
            }
        }
        
        public void SetShieldCollider(bool value)
        {
            _view.SetCollider(value);
        }
    }
}