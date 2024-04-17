using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utility
{
    public class SerializableScriptableObject : ScriptableObject
    {
        [SerializeField] Guid _guid;
        public Guid Guid => _guid;

        internal int CompareTo<T>(T x) where T : SerializableScriptableObject
        {
            return _guid.ToString().CompareTo(x._guid.ToString());
        }

#if UNITY_EDITOR
        void OnValidate()
    {
        var path = AssetDatabase.GetAssetPath(this);
        _guid = new Guid(AssetDatabase.AssetPathToGUID(path));
    }
#endif
    }
}