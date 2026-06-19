using UnityEngine;

namespace RPG.Save
{
    public abstract class SaveAdapter<T1, T2>
    {
        public abstract void DataToClass(T1 classSave, T2 dataSave);
        public abstract void ClassToData(T1 classSave, T2 dataSave);
    }
}
