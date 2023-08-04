using System.Collections;
using Data;
using UnityEngine;
using View;

namespace Model
{
    public class WeaponModel : IWeapon
    {
        private float ReloadTime => _reloadTime * (1 + _unitModel.GetEffect(BuffEffect.ReloadTime));
        private readonly WeaponView _view;
        private readonly float _reloadTime;
        private bool _isFire;
        private readonly float _force;
        private readonly string _name;
        private readonly int _damage;
        private readonly UnitModel _unitModel;
        private Coroutine _coroutine;
        
        public WeaponModel(WeaponView view, WeaponData data, UnitModel unitModel)
        {
            _view = view;
            _reloadTime = data.reloadTime;
            _force = data.force;
            _unitModel = unitModel;
            _name = data.weaponName;
            _damage = data.damage;
        }

        public void Fire(bool value)
        {
            if (value)
            {
                Fire();
            }
            else
            {
                StopFire();
            }
        }

        public void Fire()
        {
            _coroutine = _view.StartCoroutine(StartFire());
        }

        private IEnumerator StartFire()
        {
            if (_isFire)
            {
                yield break;
            }

            _isFire = true;

            while (_isFire)
            {
                yield return new WaitForSeconds(ReloadTime);
                _view.Fire(_force, _unitModel.Player, _damage);
            }
            
        }

        public void StopFire()
        {
            _isFire = false;

            if (_coroutine != null)
            {
                _view.StopCoroutine(_coroutine);
            }
        }

        public override string ToString()
        {
            return _name + "(D:" + _damage + " R:" + ReloadTime + ")";
        }
    }
}