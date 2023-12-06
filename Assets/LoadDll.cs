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

        // �Ȳ���Ԫ����
        LoadMetadataForAOTAssemblies();

        //�༭������
#if UNITY_EDITOR
        Debug.Log($"�༭������");
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
        RunUpdateAssembly(hotUpdateAss);
        return;
#endif

        // android����
#if UNITY_ANDROID
        Debug.Log($"android����");
        string pathAndroid = $"{Application.streamingAssetsPath}/Assemblies/HotUpdate.dll.bytes";
        StartCoroutine(LoadAssemblyFromPath(pathAndroid));
        return;
#endif

        // window����
#if !UNITY_EDITOR
        Debug.Log($"�Ǳ༭����window������");
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
    /// �����ȸ�����
    /// </summary>
    private void RunUpdateAssembly(Assembly hotUpdateAss)
    {
        // ���ȸ��������ҵ�Hello�࣬������Run����
        Type type = hotUpdateAss.GetType("Hello");
        type.GetMethod("Run").Invoke(null, null);
    }

    /// <summary>
    /// ��AOT�в���Ԫ����
    /// </summary>
    private void LoadMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll",
            //"UnityEngine.dll",//65�಻��ͨ��������Ԫ���ݣ�����
            //"System.dll",
            //"System.Core.dll", // ���ʹ����Linq����Ҫ���
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
