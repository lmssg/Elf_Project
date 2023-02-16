using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromFileExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AssetBundle ab = AssetBundle.LoadFromFile("AssetBundles/Test.unity2d");
        GameObject texst = ab.LoadAsset<GameObject>("bullet");
        Instantiate(texst, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
