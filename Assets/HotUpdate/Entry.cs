using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"[Entry] GameObject:{name},����ҵ���߼�");

        // ����AB��
        string AssetBundleName = "icon"; //"icon.assetbundle";
        string strPath = Application.streamingAssetsPath + "/" + AssetBundleName;
        AssetBundle bundle = AssetBundle.LoadFromFile(strPath);
        // ����Asset
        UnityEngine.Object prefabAsset = bundle.LoadAsset("Cube.prefab");
        // ʵ����Ԥ��
        GameObject go = Object.Instantiate(prefabAsset) as GameObject;

        // ����Ԫ����
        var arr = new List<float>() { 111,222,333,444};
        for (int i = 0; i < arr.Count;i++)
        {
            Debug.Log($"[Entry] ����Ԫ����:{arr[i]}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
