using UnityEngine;

namespace RPG.Save
{
    public abstract class SaveAdapter<Class, Data>
    {
        public abstract void DataToClass(Class classSave, Data dataSave);
        public abstract void ClassToData(Class classSave, Data dataSave);
    }
}
