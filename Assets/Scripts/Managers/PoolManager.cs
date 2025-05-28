using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PoolManager
{
    Vector3 RespawnPosition = new Vector3(5f, -2.9f);
    //public GameObject ZombieMeleeSample;
    //public GameObject BulletSample;
    private Transform ZombieMelee_Root;
    private Transform Bullet_Root;
    private Transform Box_Root;

    //private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>(); // ALLSAMPLE
    public Dictionary<string, List<Unit>> Dictionary_AllGameObject = new Dictionary<string, List<Unit>>();

    Transform _root;

    private GameObject[] AllBox = new GameObject[5];
    private GameObject LastPlayer;


    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            _root.transform.SetParent(GameObject.Find("@Managers").transform);

            //UnityEngine.Object.DontDestroyOnLoad(_root);
        }

        Setting();
    }

    void Setting() // 필수오브젝트 생성
    {
        if (ZombieMelee_Root == null)
        {
            GameObject ZB = new GameObject();
            ZombieMelee_Root = ZB.transform;
            ZombieMelee_Root.name = "ZombieMelee_Root";
            ZombieMelee_Root.transform.SetParent(_root);
        }

        if (Bullet_Root == null)
        {
            GameObject BR = new GameObject();
            Bullet_Root = BR.transform;
            Bullet_Root.name = "Bullet_Root";
            Bullet_Root.transform.SetParent(_root);
        }

        if (Box_Root == null)
        {
            GameObject BR = new GameObject();
            Box_Root = BR.transform;
            Box_Root.name = "Box_Root";
            Box_Root.transform.SetParent(_root);
        }

        CreatePlayer();
        CreateBox();

    }

    public void GetObjTEST()
    {
        var p = GetObject<Player>();
        var z = GetObject<ZombieMonster>();

        int q = 0;
    }







    public void CreatePlayer()
    {

        var _playercomponent = GameObject.Find("Hero").AddComponent<Player>();
        LastPlayer = _playercomponent.gameObject;
        LastPlayer.layer = 6;
        var P_Rigid = LastPlayer.AddComponent<Rigidbody2D>();
        P_Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        var BC2D = LastPlayer.AddComponent<BoxCollider2D>();
        BC2D.size = new Vector3(0.95f, 1.45f);
        RegisterUnit<Player>(_playercomponent, "player");
    }

    public T MakeOrGetObject<T>() where T : Unit // 여기개선?
    {
        if (typeof(T) == typeof(ZombieMonster))
        {
            if (Dictionary_AllGameObject.ContainsKey("zombie"))
            {
                foreach (var zom in Dictionary_AllGameObject["zombie"])
                {
                    if (zom.bDie)
                    {

                        zom.Get_GameObject.SetActive(true);
                        zom.SetDie(false);
                        zom.transform.position = RespawnPosition;
                        Debug.Log("좀비 리필");
                        return zom as T;
                    }

                }
                Debug.Log("좀비 생성");
                return CreateZombie() as T;
            }
            else
            {
                return CreateZombie() as T;
            }

        }

        if (typeof(T) == typeof(Bullet))
        {

            if (Dictionary_AllGameObject.ContainsKey("bullet"))
            {
                foreach (var bul in Dictionary_AllGameObject["bullet"])
                {
                    if (bul.bDie)
                    {
                        if (!bul.GetComponent<Bullet>().bFlying)
                        {
                            bul.Get_GameObject.SetActive(true);
                            bul.SetDie(false);
                            return bul as T;

                        }
                    }

                }

                return CreateBullet() as T;
            }
            else
                return CreateBullet() as T;
        }

        return null;

    }

    public T GetObject<T>() where T : Unit // 여기개선?
    {
        if (typeof(T) == typeof(Player))
        {
            if (Dictionary_AllGameObject.ContainsKey("player"))
            {
                foreach (var p in Dictionary_AllGameObject["player"])
                {
                    if (!p.bDie) // 죽어있지않다면 
                    {
                        return p as T; //반환
                    }

                }


            }
        }

            if (typeof(T) == typeof(ZombieMonster))
        {
            if (Dictionary_AllGameObject.ContainsKey("zombie"))
            {
                foreach (var zom in Dictionary_AllGameObject["zombie"])
                {
                    if (!zom.bDie) // 죽어있지않다면 
                    {
                        return zom as T; //반환
                    }

                }
            }

        }

        if (typeof(T) == typeof(Bullet))
        {

            if (Dictionary_AllGameObject.ContainsKey("bullet"))
            {
                foreach (var bul in Dictionary_AllGameObject["bullet"])
                {
                    if (!bul.bDie)
                    {
                        if (bul.GetComponent<Bullet>().bFlying)
                        {

                            return bul as T;

                        }
                    }

                }

            }
        }

        if (typeof(T) == typeof(Box))
        {
            if (Dictionary_AllGameObject.ContainsKey("box"))
            {
                foreach (var box in Dictionary_AllGameObject["box"])
                {
                    if (!box.bDie)
                    {

                        return box as T;

                    }

                }

            }
        }


        return null;
    }




    void CreateBox()
    {
        var BoxTrans = GameObject.Find("BoxTrans");

        for (var i = 0; i < 5; ++i)
        {
            GameObject box = BoxTrans.transform.GetChild(i).gameObject;
            var boxcom = box.AddComponent<BoxCollider2D>();
            boxcom.size = new Vector3(1.9f, 1.5f);
            box.layer = 6; // Wall
            var Rigid = box.AddComponent<Rigidbody2D>();
            Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            Box BoxComponent = box.AddComponent<Box>();
            RegisterUnit<Box>(BoxComponent, "box");
            AllBox[i] = box;

        }
    }

    public ZombieMonster CreateZombie()
    {

        GameObject _zom = Managers.Resource_Instance.Instantiate("ZombieMelee", ZombieMelee_Root);

        _zom.SetActive(true);
        _zom.transform.position = RespawnPosition;

        ZombieMonster zombiecomponent = _zom.AddComponent<ZombieMonster>();
        RegisterUnit<ZombieMonster>(zombiecomponent, "zombie");

        var boxcom = _zom.AddComponent<BoxCollider2D>();
        boxcom.offset = new Vector3(-0.2f, 0.5f);
        boxcom.size = new Vector3(0.6f, 1.2f);
        zombiecomponent.TestNumber = Dictionary_AllGameObject["zombie"].Count;
        var Rigid = _zom.AddComponent<Rigidbody2D>(); // 좀비끼리 충돌처리를 위해
        Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rigid.gravityScale = 0;

        return zombiecomponent;
    }

    Bullet CreateBullet()
    {

        GameObject _bul = Managers.Resource_Instance.Instantiate("Bullet", Bullet_Root);

        _bul.SetActive(true);
        _bul.transform.position = new Vector3(-99, -99);
        Bullet BulletComponent = _bul.AddComponent<Bullet>();
        RegisterUnit<Bullet>(BulletComponent, "bullet");

        return BulletComponent;

    }


    T RegisterUnit<T>(T New_CObj, string _name) where T : Unit, new()
    {

        if (!Dictionary_AllGameObject.ContainsKey(_name))
        {
            List<Unit> New_ = new List<Unit>();
            Dictionary_AllGameObject.Add(_name, New_);
            Dictionary_AllGameObject[_name].Add(New_CObj);
        }
        else
        {
            Dictionary_AllGameObject[_name].Add(New_CObj);
        }

        return New_CObj;
    }


    void BoxPushMonster()
    {
        if (!Dictionary_AllGameObject.ContainsKey("zombie"))
            return;

        foreach (var zom2 in Dictionary_AllGameObject["zombie"])
        {
            foreach (var box1 in Dictionary_AllGameObject["box"])
            {
                //if(box1.gameObject.activeSelf)
                CollisionObjectManagers.bRectCollsionPushFirstObject(zom2, box1);
            }
        }
    }

}
