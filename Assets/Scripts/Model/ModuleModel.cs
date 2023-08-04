using System;
using System.Collections.Generic;
using Data;
using View;

namespace Model
{
    public class ModuleModel : IModule
    {
        private ModuleView _view;
        private readonly ModuleData _data;
        private readonly IUnit _unitModel;
        private readonly int _slotIndex;
        private readonly string _toString;

        public ModuleModel(ModuleView view, ModuleData data, IUnit unitModel, int slotIndex)
        {
            _view = view;
            _data = data;
            _unitModel = unitModel;
            _slotIndex = slotIndex;
            _toString = data.ToString();
        }

        public void Connect()
        {
            var effects = new List<Tuple<BuffEffect, float>>(_data.buffs.Length);
            foreach (var buff in _data.buffs)
            {
                var effect = new Tuple<BuffEffect, float>(buff.buffEffect, buff.value);
                effects.Add(effect);
            }
            _unitModel.AddModuleEffect(_slotIndex, effects);
        }

        public override string ToString()
        {
            return _toString;
        }

        public void Disconnect()
        {
            _unitModel.RemoveModuleEffect(_slotIndex);
        }
    }
}