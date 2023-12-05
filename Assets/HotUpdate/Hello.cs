using System.Collections;
using UnityEngine;

public class Hello
{
    public static void Run()
    {
        Debug.Log("Hello, 需要热更的程序集-更新啦啦啦");

        GameObject go = new GameObject("Entry");
        go.AddComponent<Entry>();
    }
}