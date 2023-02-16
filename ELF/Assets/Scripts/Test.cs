using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{

    public AssetReference reference;
    public AssetReference reference2;

    // Start is called before the first frame update
    void Start()
    {
        //  reference.InstantiateAsync().Completed += e => { Debug.Log("加载成功"); };

        // Addressables.LoadAssetAsync<GameObject>("bullet.prefab").Completed += e => { Debug.Log("加载成功:" + e.Result.name); };

        // Addressables.LoadAssetsAsync<GameObject>("bullet.prefab", null).Completed += e => { Debug.Log("加载成功:" + e.Result.Count); };
        // Addressables.LoadAssetsAsync<GameObject>(new List<object> { "bullet.prefab", "bullet.prefab" }, null, Addressables.MergeMode.Intersection).Completed += e => { Debug.Log("加载成功:" + e.Result.Count); };
        WaitInsVerForC();


    }

    // Update is called once per frame
    void Update()
    {

    }


    async void WaitInsVerForC()
    {
        var handle = reference.InstantiateAsync();
        await handle.Task;

        handle = reference2.InstantiateAsync();
        await handle.Task;

        Addressables.LoadAssetAsync<GameObject>("bullet.prefab");
    }
}
