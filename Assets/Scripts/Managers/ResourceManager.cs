using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager 
{
    Transform _root;

    Dictionary<string, GameObject> List_Prefabs = new Dictionary<string, GameObject>();

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Resource_Root" }.transform;
            _root.transform.SetParent(GameObject.Find("@Managers").transform);

            //UnityEngine.Object.DontDestroyOnLoad(_root);
        }             
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            //GameObject go = Managers.Pool.GetOriginal(name);
            //if (go != null)
            //    return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null) // 리소스폴더에서 로드
    {
        GameObject original = null;
        if (List_Prefabs.TryGetValue(path, out GameObject obj))
        {
            original = List_Prefabs[path];
        }
        else
        {
            original = Load<GameObject>($"Prefabs/{path}");
            if (original == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }
            else
            {
                List_Prefabs.Add(path, original);
            }
        }

            
        

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }


    public string[] PrefabList = { typeof(ZombieMelee).Name, "Hero" };
    public Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();

    public GameObject GetPrefab(string address)
    {
        if (prefabCache.TryGetValue(address, out var prefab))
            return prefab;

        Debug.LogError($" 캐시에 없음: {address}");
        return null;
    }

}
