using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainGame : MonoBehaviour
{

    

    public PoolManager _pool;

 
    //private static MainGame Instance = null;
    //public static MainGame GetInstance() { Init(); return Instance; }

    //static void Init()
    //{
    //    GameObject go = GameObject.Find("MainGame");

    //    //if (go == null)
    //    //{
    //    //    go = new GameObject { name = "MainGame" };
    //    //    go.AddComponent<MainGame>();
    //    //}

    //    Instance = go.GetComponent<MainGame>();
    //}

    void Start()
    {

        Managers.Input_Instance.Test();
        
        


        //StartCoroutine(ExecuteEverySeconds());
        //Managers.Pool.GetObject<ZombieMonster>();








        //var _player = BoxTrans.Find("Hero").gameObject.AddComponent<Player>();
        //RegisterUnit<Player>(_player, "player");
        ////_player.ReadyBullet(_bullets);


        //Bullet[] _bullets = new Bullet[12];

        //for (int i = 0; i < 12; ++i)
        //{
        //    _bullets[i] = CreateBullet();
        //}


    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Q))
        {

        }

        

    }

    private void LateUpdate()
    {
        //CollisionZombies();
    }

    //void CollisionZombies()
    //{
    //    if (!Dictionary_AllGameObject.ContainsKey("zombie"))
    //        return;

    //    foreach (var zom1 in Dictionary_AllGameObject["zombie"])
    //    {
    //        foreach (var zom2 in Dictionary_AllGameObject["zombie"])
    //        {
    //            if (zom1 != zom2)
    //            {
    //                if (CollisionObjectManagers.bRectCollsionPushTopObject(zom1, zom2))
    //                {
                        
    //                }
    //            }
    //        }
    //    }

    //}

    

}
