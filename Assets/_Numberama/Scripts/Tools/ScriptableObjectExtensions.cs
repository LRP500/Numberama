using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Tools
{
    public static class ScriptableObjectExtensions
    {
        public static T CreateSubAsset<T>(this ScriptableObject so) where T : ScriptableObject
        {
            // Create sub asset
            T child = ScriptableObject.CreateInstance<T>();
            child.name = child.GetType().Name.ToString();

            // Add sub asset to parent
            AssetDatabase.AddObjectToAsset(child, so);

            // Refresh asset database
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(child));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return child;
        }

        public static void RemoveSubAsset(this ScriptableObject so, ScriptableObject child)
        {
            Object.DestroyImmediate(child, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

#endif