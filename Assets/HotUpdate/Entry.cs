using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"[Entry] GameObject:{name},进入业务逻辑");

        // 加载AB包
        string AssetBundleName = "icon"; //"icon.assetbundle";
        string strPath = Application.streamingAssetsPath + "/" + AssetBundleName;
        AssetBundle bundle = AssetBundle.LoadFromFile(strPath);
        // 加载Asset
        UnityEngine.Object prefabAsset = bundle.LoadAsset("Cube.prefab");
        // 实例化预制
        GameObject go = Object.Instantiate(prefabAsset) as GameObject;

        // 测试元数据
        var arr = new List<float>() { 111,222,333,444};
        for (int i = 0; i < arr.Count;i++)
        {
            Debug.Log($"[Entry] 测试元数据:{arr[i]}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
