using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Data
{
    [CreateAssetMenu(menuName = "Create GameData", fileName = "GameData", order = 0)]
    public class GameData : ScriptableObject
    {
        [Header("Data")]
        [SerializeField ]private List<UnitData> units;
        [SerializeField ]private List<WeaponData> weapons;
        [SerializeField ]private List<ModuleData> modules;

        [Space] 
        [Header("Prefabs")] 
        [SerializeField ]private List<UnitPrefab> unitPrefabs;
        [SerializeField ]private List<WeaponPrefab> weaponPrefabs;
        [SerializeField ]private List<ModulePrefab> modulePrefabs;
        [SerializeField ]private List<BulletPrefab> bulletPrefabs;

        private Dictionary<string, UnitView> _unitPrefabs;
        private Dictionary<string, WeaponView> _weaponPrefabs;
        private Dictionary<string, ModuleView> _modulePrefabs;
        private Dictionary<string, BulletView> _bulletPrefabs;

        private Dictionary<Type, ICollection> _allPrefabs;

        public void Init()
        {
            _unitPrefabs = CreatePrefabDictionary<UnitView, UnitPrefab>(unitPrefabs);
            _weaponPrefabs = CreatePrefabDictionary<WeaponView, WeaponPrefab>(weaponPrefabs);
            _modulePrefabs = CreatePrefabDictionary<ModuleView, ModulePrefab>(modulePrefabs);
            _bulletPrefabs  = CreatePrefabDictionary<BulletView, BulletPrefab>(bulletPrefabs);

            _allPrefabs = new Dictionary<Type, ICollection>(4)
            {
                { typeof(UnitView), _unitPrefabs },
                { typeof(WeaponView), _weaponPrefabs },
                { typeof(ModuleView), _modulePrefabs },
                { typeof(BulletView), _bulletPrefabs },
            };
            
        }

        public T GetPrefab<T>(string id) where T : GameObjectView
        {
            Dictionary<string, T> dictionary = (Dictionary<string, T>)_allPrefabs[typeof(T)];
            return dictionary[id];
        }

        public bool HasPrefab<T>(string id) where T : GameObjectView
        {
            Dictionary<string, T> dictionary = (Dictionary<string, T>)_allPrefabs[typeof(T)];
            return dictionary.ContainsKey(id) && dictionary[id] != null;
        }

        private Dictionary<string, T1> CreatePrefabDictionary<T1, T2>(List<T2> prefabsList) 
            where T1 : GameObjectView 
            where T2 : PrefabData
        {
            Dictionary<string, T1> result = new Dictionary<string, T1>(prefabsList.Count);
            
            foreach (var prefab in prefabsList)
            {
                if (result.ContainsKey(prefab.id) == false)
                {
                    result.Add(prefab.id, (T1)prefab.Prefab);
                }
                else
                {
                    throw new Exception($"Duplicate id = {prefab.id}");
                }
            }

            return result;
        }

        public List<WeaponData> GetWeapons() => weapons;
        public List<ModuleData> GetModules() => modules;


    }
}