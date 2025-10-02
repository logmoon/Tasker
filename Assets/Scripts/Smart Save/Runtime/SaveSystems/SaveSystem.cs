using System;
using UnityEngine;

namespace SmartSaves.SaveSystems
{
    public abstract class SaveSystem<T> where T : Data<T>
    {
        #region Variables

        protected Data<T> data;

        #endregion

        #region Constructor

        public SaveSystem(Data<T> _data, Config _config)
        {
            data = _data;
        }

        #endregion

        #region Methods

        public abstract string GetSaveFileDefaultPath();
        public abstract void Save();
        public abstract void LoadFromData(string fileData);
        public abstract void LoadFromPath(string path);
        public abstract void Load();
        public abstract void Unload();
        public abstract void Delete();

        #endregion
    }
}