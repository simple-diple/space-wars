using System;
using Model;
using UnityEngine;

namespace Data
{
    [Serializable]
    public abstract class BuffData : ScriptableObject
    {
        public string buffName;
        public float value;
        public abstract void Connect(UnitModel unitModel);
        public abstract void Disconnect(UnitModel unitModel);
    }
}