using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    PoolManager _pool = new PoolManager();
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    //CooldownManager _cooldown = new CooldownManager();

    public static PoolManager Pool_Instance { get { return Instance._pool; } }
    public static InputManager Input_Instance { get { return Instance._input; } }
    public static ResourceManager Resource_Instance { get { return Instance._resource; } }
    //public static CooldownManager Cooldown_Instance { get { return Instance._cooldown; } }


    void Start()
    {
        Init();


        //StartCoroutine(LoadCoroutine());

    }


    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            s_instance = go.GetComponent<Managers>();
            go.transform.SetParent(GameObject.Find("MainGame").transform);

            s_instance._pool.Init();


        }
    }

    public static void Clear()
    {
        //Pool.Clear();
    }


    public System.Collections.IEnumerator LoadCoroutine() //로딩하는시간이 필요하다
    {

        foreach (string address in Resource_Instance.PrefabList)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Resource_Instance.prefabCache[address] = handle.Result;
                Debug.Log($"로드 완료: {address}");
            }
            else
            {
                Debug.LogError($"로드 실패: {address}");
            }
        }

        //onComplete?.Invoke();
    }
}
