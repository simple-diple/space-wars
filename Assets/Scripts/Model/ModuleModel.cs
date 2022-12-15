using Data;
using View;

namespace Model
{
    public class ModuleModel
    {
        private ModuleView _view;
        private readonly string _name;
        private readonly string _effects;
        private readonly ModuleData _data;

        public ModuleModel(ModuleView view, ModuleData data)
        {
            _view = view;
            _name = data.moduleName;
            _data = data;

            foreach (BuffData buffData in data.buffs)
            {
                _effects += string.Format(buffData.buffName, buffData.value + " ");
            }
        }

        public void Connect(UnitModel unitModel)
        {
            _data.Connect(unitModel);
        }

        public override string ToString()
        {
            return _name + "(" + _effects + ")";
        }

        public void Disconnect(UnitModel unitModel)
        {
            _data.Disconnect(unitModel);
        }
    }
}