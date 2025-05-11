using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : MonoBehaviour
{

    enum PrefabListE { ZombieMelee, Hero }
    string[] PrefabList = { "ZombieMelee", "Hero" };


    private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();

    void Start()
    {

        StartCoroutine(LoadCoroutine());
    }


    private System.Collections.IEnumerator LoadCoroutine()
    {
        
        foreach (string address in PrefabList)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                prefabCache[address] = handle.Result;
                Debug.Log($"�ε� �Ϸ�: {address}");
            }
            else
            {
                Debug.LogError($"�ε� ����: {address}");
            }
        }

        //onComplete?.Invoke();
    }

    public GameObject GetPrefab(string address)
    {
        if (prefabCache.TryGetValue(address, out var prefab))
            return prefab;

        Debug.LogError($" ĳ�ÿ� ����: {address}");
        return null;
    }

}
