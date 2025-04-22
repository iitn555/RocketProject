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

    // Update is called once per frame
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

    

    //void RegisterUnit<T>(T New_CObj, string _name) where T : Unit, new()
    //{
    //    //T New_CObj2 = new T();

    //    if (!Dictionary_AllGameObject.ContainsKey(_name))
    //    {
    //        List<Unit> New_ = new List<Unit>();
    //        Dictionary_AllGameObject.Add(_name, New_);
    //        Dictionary_AllGameObject[_name].Add(New_CObj);
    //    }
    //    else
    //    {
    //        Dictionary_AllGameObject[_name].Add(New_CObj);
    //    }
    //}

    //void CreateZombie()
    //{
    //    if (ZombieMelee_Root == null)
    //    {
    //        GameObject ZB = new GameObject();
    //        ZombieMelee_Root = ZB.transform;
    //        ZombieMelee_Root.SetParent(transform);
    //    }

    //    GameObject _zombie = Instantiate(ZombieMeleeSample, ZombieMelee_Root);
    //    _zombie.SetActive(true);
    //    ZombieMonster zombiecomponent = _zombie.AddComponent<ZombieMonster>();
    //    _zombie.transform.position = new Vector3(5f, _zombie.transform.position.y, _zombie.transform.position.z);
    //    var boxcom = _zombie.AddComponent<BoxCollider2D>();
    //    boxcom.offset = new Vector3(-0.2f, 0.5f);
    //    boxcom.size = new Vector3(0.6f, 1.2f);

    //    RegisterUnit<ZombieMonster>(zombiecomponent, "zombie");
    //    zombiecomponent.TestNumber = Dictionary_AllGameObject["zombie"].Count;

    //    var Rigid = _zombie.AddComponent<Rigidbody2D>(); // 좀비끼리 충돌처리를 위해
    //    Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    //    Rigid.gravityScale = 0;

    //}

    //Bullet CreateBullet ()
    //{
    //    if(Bullet_Root == null)
    //    {
    //        GameObject BR = new GameObject();
    //        Bullet_Root = BR.transform;
    //        Bullet_Root.SetParent(transform);
    //    }

    //    GameObject _bul = Instantiate(BulletSample, Bullet_Root);
    //    _bul.SetActive(true);
    //    Bullet bulletcomponent = _bul.AddComponent<Bullet>();
    //    _bul.transform.position = new Vector3(-99,-99);

    //    //var boxcom = _bul.AddComponent<BoxCollider2D>();
    //    //boxcom.offset = new Vector3(-0.2f, 0.5f);
    //    //boxcom.size = new Vector3(0.6f, 1.2f);

    //    RegisterUnit<Bullet>(bulletcomponent, "bullet");

    //    return bulletcomponent;

    //}
}
