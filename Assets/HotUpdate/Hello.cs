using System.Collections;
using UnityEngine;

public class Hello
{
    public static void Run()
    {
        Debug.Log("Hello, ��Ҫ�ȸ��ĳ���-����������");

        GameObject go = new GameObject("Entry");
        go.AddComponent<Entry>();
    }
}