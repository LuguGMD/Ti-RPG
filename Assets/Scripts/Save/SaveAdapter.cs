using UnityEngine;

namespace RPG.Save
{
    public abstract class SaveAdapter<T1, T2>
    {
        private T1 _class;

        public SaveAdapter(T1 classSave)
        {
            _class = classSave;
        }

        #region Properties

        public T1 ClassSave => _class;

        #endregion

        public abstract void DataToClass(T1 classSave, T2 dataSave);
    }
}
