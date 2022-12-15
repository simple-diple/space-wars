using System.Collections;
using Data;
using UnityEngine;
using View;

namespace Model
{
    public class WeaponModel
    {

        private readonly WeaponView _view;
        private float _reloadTime;
        private bool _isFire;
        private readonly float _force;
        private readonly int _player;
        private readonly string _name;
        private readonly int _damage;
        
        public WeaponModel(WeaponView view, WeaponData data, int player)
        {
            _view = view;
            _reloadTime = data.reloadTime;
            _force = data.force;
            _player = player;
            _name = data.weaponName;
            _damage = data.damage;
        }

        public void UpgradeReload(float value)
        {
            _reloadTime *= 1 + value;
        }

        public void Fire()
        {
            _view.StartCoroutine(StartFire());
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
                yield return new WaitForSeconds(_reloadTime);
                _view.Fire(_force, _player, _damage);
            }
            
        }

        public void StopFire()
        {
            _isFire = false;
        }

        public override string ToString()
        {
            return _name + "(D:" + _damage + " R:" + _reloadTime + ")";
        }
    }
}