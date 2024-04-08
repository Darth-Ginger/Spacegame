using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM
{
    [FilePath("Assets/MM/Managers/ManagersContainer.foo", FilePathAttribute.Location.ProjectFolder)]
    public class MM_ManagersContainer : ScriptableSingleton<MM_ManagersContainer>
    {
        public List<MM_SceneManagerPair> Managers = new();

        public void SaveData()
        {
            Save(true);
        }
    }

    [Serializable]
    public class MM_SceneManagerPair
    {
        public SceneAsset Scene;
        public GameObject Prefab;
        public string Path;
        public string DisplayName;
        public string Type;
    }
}