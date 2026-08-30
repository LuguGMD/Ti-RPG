using UnityEngine;

namespace RPG.Save
{
    public abstract class SaveAdapter<Class>
    {
        public abstract void DataToClass(Class classSave);
        public abstract void ClassToData(Class classSave);
    }
}
