using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestMain : MonoBehaviour
{
	string AssetBundleName = "cube1.assetbundle";

	private string dir = "";
	private AssetBundle bundle = null;
	private UnityEngine.Object asset = null;
	private GameObject go = null;

 //   List<AssetBundle> assetBundles = new List<AssetBundle>();
	//private List<Object> assetObList = new List<Object>();
	//private List<GameObject> goObList = new List<GameObject>();

	private void Start() 
	{ 
		dir = Application.dataPath + "/StreamingAssets/"; 
	}

	private void OnGUI()
	{
		// 这些操作一遍走下来，申请的资源是可以完全回收的
		if (GUILayout.Button("LoadAssetBundle", GUILayout.Width(400), GUILayout.Height(100))) { LoadBundle(); }
		if (GUILayout.Button("LoadAsset", GUILayout.Width(400), GUILayout.Height(100))) { LoadAsset(); }
		if (GUILayout.Button("Instantiate", GUILayout.Width(400), GUILayout.Height(100))) { Instantiate(); }
		if (GUILayout.Button("Destroy", GUILayout.Width(400), GUILayout.Height(100))) { Destroy(); }
		if (GUILayout.Button("Unload", GUILayout.Width(400), GUILayout.Height(100))) { Unload(); }
		if (GUILayout.Button("UnloadForce", GUILayout.Width(400), GUILayout.Height(100))) { UnloadForce(); }
		if (GUILayout.Button("UnloadUnusedAssets", GUILayout.Width(400), GUILayout.Height(100))) { UnloadUnusedAssets(); }
	}

	// 加载AssetBundle
	private void LoadBundle()
	{
        AssetBundleName = "icon.assetbundle";
        string strPath = Application.streamingAssetsPath + "/" + AssetBundleName;
        bundle = AssetBundle.LoadFromFile(strPath);

        if (bundle == null)
        {
            Debug.LogError($"LoadBundle Failed");
        }
        else
        {
			Debug.Log("LoadBundle Success!");
        }
    }

	// 从AssetBundle加载Asset
	private void LoadAsset()
	{
        asset = bundle.LoadAsset("Bluetooth.prefab");
		if (asset != null)
		{
			Debug.Log("LoadAsset Success ...");
		}
		else
		{
			Debug.LogError($" LoadAsset faild!");
		}
	}

	// 根据Asset实例化GameObject
	private void Instantiate()
	{
		go = Instantiate(asset, transform) as GameObject;
		if (go == null)
		{ 
			Debug.LogError("Instantiate Failed");
		}
		else
		{
			Debug.Log($"Instantiate Success>>name:{go.name}");
		}
	}

	// 销毁GameObject
	private void Destroy()
	{
		if (go == null)
		{
			Debug.LogError("Destroy go == null");
			return;
		}

		GameObject.Destroy(go);
		go = null;
		Debug.Log("Destroy success");
	}

	// 弱卸载，释放AssetBundle本身的内存
	private void Unload()
	{
		if (bundle == null)
		{
			Debug.LogError("Unload bundle == null");
			return;
		}

		// unload完，bundle就不能再用了，记得要置空
		bundle.Unload(false);
		asset = null;
		bundle = null;
		Debug.Log("Unload(false) success");
	}

	// 强卸载（无视引用的卸载），释放AssetBundle本身的内存，同时回收从AssetBundle抽取的Asset
	private void UnloadForce()
	{
		if (bundle == null)
		{
			Debug.LogError("UnloadForce bundle == null");
			return;
		}

		// unload完，bundle就不能再用了，记得要置空
		bundle.Unload(true);
		//asset = null;
		bundle = null;
		Debug.Log("Unload(true) success");
	}

	// 全局弱卸载，回收无引用Asset
	private void UnloadUnusedAssets()
	{
		Resources.UnloadUnusedAssets();
		Debug.Log("UnloadUnusedAssets success");
	}

}