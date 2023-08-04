using System;
using System.Collections.Generic;
using Data;

namespace Model
{
    public interface IUnit
    {
        void AddModuleEffect(int slotIndex, List<Tuple<BuffEffect, float>> effects);
        void RemoveModuleEffect(int slotIndex);
        float ShieldEnergy { get; }
        float Health { get; }
        float ShieldRecovery { get;}
        Dictionary<int, IWeapon> Weapons { get;}
        Dictionary<int, IModule> Modules { get; }
        event Action<IUnit> OnStatsChange;
    }
}