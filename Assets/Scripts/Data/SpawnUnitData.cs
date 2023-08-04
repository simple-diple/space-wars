using System;
using UnityEngine;
using View.UI;

namespace Data
{
    [Serializable]
    public class SpawnUnitData
    {
        public UnitData unitData;
        public Transform spawnPoint;
        public UISheepInfo ui;
    }
}