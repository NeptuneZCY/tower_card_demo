using System.Collections.Generic;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    private static PrefabFactory _instance;

    public static PrefabFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("PrefabFactory");
                _instance = obj.AddComponent<PrefabFactory>();
                DontDestroyOnLoad(obj); // 确保工厂在场景切换时不被销毁
            }
            return _instance;
        }
    }

    // 用来存储Prefab的引用
    private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();

    // 加载并缓存Prefab
    public GameObject LoadPrefab(string path)
    {
        if (!prefabCache.ContainsKey(path))
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"Prefab at {path} not found!");
                return null;
            }
            prefabCache[path] = prefab;
        }
        return prefabCache[path];
    }

    // 生成Prefab实例
    public GameObject CreateInstance(string path, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = LoadPrefab(path);
        if (prefab != null)
        {
            return Instantiate(prefab, position, rotation);
        }
        return null;
    }
}
