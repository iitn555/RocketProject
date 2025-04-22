//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Managers : MonoBehaviour
//{
//    static Managers s_instance;
//    static Managers Instance { get { Init(); return s_instance; } }

//    PoolManager _pool = new PoolManager();
//    public static PoolManager Pool { get { return Instance._pool; } }

//    void Start()
//    {
//        Init();
//    }


//    static void Init()
//    {
//        if (s_instance == null)
//        {
//            GameObject go = GameObject.Find("@Managers");
//            if (go == null)
//            {
//                go = new GameObject { name = "@Managers" };
//                go.AddComponent<Managers>();
//            }

//            s_instance = go.GetComponent<Managers>();

//            s_instance._pool.Init();

//            go.transform.SetParent(GameObject.Find("MainGame").transform);
//        }
//    }

//    public static void Clear()
//    {
//        //Pool.Clear();
//    }

//}
