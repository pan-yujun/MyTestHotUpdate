using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{

    void Start()
    {
        Debug.Log($"Application.streamingAssetsPath:{Application.streamingAssetsPath}");

        // 先补充元数据
        LoadMetadataForAOTAssemblies();

        //编辑器运行
#if UNITY_EDITOR
        Debug.Log($"编辑器运行");
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
        RunUpdateAssembly(hotUpdateAss);
        return;
#endif

        // android运行
#if UNITY_ANDROID
        Debug.Log($"android运行");
        string pathAndroid = $"{Application.streamingAssetsPath}/Assemblies/HotUpdate.dll.bytes";
        StartCoroutine(LoadAssemblyFromPath(pathAndroid));
        return;
#endif

        // window运行
#if !UNITY_EDITOR
        Debug.Log($"非编辑器（window）运行");
        Assembly hotUpdateAssAndroid = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/Assemblies/HotUpdate.dll.bytes"));
        RunUpdateAssembly(hotUpdateAssAndroid);
        return;
#endif
    }

    private IEnumerator LoadAssemblyFromPath(string path)
    {
        var request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        if (!request.isDone)
        {
            yield break;
        }
        var assembleData = request.downloadHandler.data;
        var assembly = Assembly.Load(assembleData);

        RunUpdateAssembly(assembly);
    }

    /// <summary>
    /// 运行热更程序集
    /// </summary>
    private void RunUpdateAssembly(Assembly hotUpdateAss)
    {
        // 从热更程序集中找到Hello类，并调用Run方法
        Type type = hotUpdateAss.GetType("Hello");
        type.GetMethod("Run").Invoke(null, null);
    }

    /// <summary>
    /// 在AOT中补充元数据
    /// </summary>
    private void LoadMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll",
            //"UnityEngine.dll",//65类不可通过这个添加元数据！！！
            //"System.dll",
            //"System.Core.dll", // 如果使用了Linq，需要这个
            //"UnityEngine.IMGUIModule.dll",
            //"UnityEngine.CoreModule.dll",
            //"UnityEngine.AssetBundleModule.dll",
            // "Newtonsoft.Json.dll", 
            // "protobuf-net.dll",
        };

        foreach (var aotDllName in aotDllList)
        {
            string path = $"{Application.streamingAssetsPath}/Assemblies/{aotDllName}.bytes";
            StartCoroutine(LoadMetadataAssemblyFromPath(aotDllName, path));
            //byte[] dllBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/Assemblies/{aotDllName}.bytes");
            //int err = (int)HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            //Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }
    }

    private IEnumerator LoadMetadataAssemblyFromPath(string aotDllName, string path)
    {
        var request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        if (!request.isDone)
        {
            yield break;
        }
        var assembleData = request.downloadHandler.data;
        int err = (int)HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(assembleData, HomologousImageMode.SuperSet);
        Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
    }
}
