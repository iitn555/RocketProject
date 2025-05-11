using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PoolManager
{
    Vector3 RespawnPosition = new Vector3(5f, -2.9f);
    public GameObject ZombieMeleeSample;
    public GameObject BulletSample;
    private Transform ZombieMelee_Root;
    private Transform Bullet_Root;

    //private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>(); // ALLSAMPLE
    public Dictionary<string, List<Unit>> Dictionary_AllGameObject = new Dictionary<string, List<Unit>>();

    Transform _root;

    //private static PoolManager Instance = null;
    //public static PoolManager GetInstance() { SingletonInit(); return Instance; }

    public float MonsterCreateCycleSecond = 3;

    private GameObject[] AllBox = new GameObject[5];
    private GameObject LastPlayer;

    public GameObject BigWall;

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

    void Setting()
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
    }

    private void Start()
    {
        //CreateZombie();
        //CreateZombie();
        //Debug.Log("PoolManagerSTART!");




        //StartCoroutine(ExecuteEverySeconds());

        //var _player = transform.parent.Find("Truck").Find("BoxTrans").Find("Hero").gameObject.AddComponent<Player>();
        //LastPlayer = _player.gameObject;
        //LastPlayer.layer = 6;
        //var P_Rigid = LastPlayer.AddComponent<Rigidbody2D>();
        //P_Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        //var BC2D = LastPlayer.AddComponent<BoxCollider2D>();
        //BC2D.size = new Vector3(0.95f, 1.45f);

        //RegisterUnit<Player>(_player, "player");



        //var BoxTrans = transform.parent.Find("Truck").Find("BoxTrans");

        //for (var i = 0; i < 5; ++i)
        //{
        //    GameObject box = BoxTrans.GetChild(i).gameObject;
        //    var boxcom = box.AddComponent<BoxCollider2D>();
        //    boxcom.size = new Vector3(1.9f, 1.5f);
        //    box.layer = 6; // Wall

        //    var Rigid = box.AddComponent<Rigidbody2D>();
        //    Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        //    Box BoxComponent = box.AddComponent<Box>();
        //    RegisterUnit<Box>(BoxComponent, "box");

        //    AllBox[i] = box;

        //}


    }

    private void Update()
    {
        if (MonsterCreateCycleSecond > 1)
            MonsterCreateCycleSecond -= Time.deltaTime * 0.02f;

        if (Input.GetKeyDown(KeyCode.Z))
            GetObject<ZombieMonster>();
    }

    private void LateUpdate()
    {

        CheckGameEnd();


        //BoxPushMonster();

    }
    void CheckGameEnd()
    {
        bool checkbox = false;

        for (int i = 0; i < 5; ++i)
        {
            if (AllBox[i].activeSelf)
            {
                checkbox = true;
            }
        }

        //if (!checkbox) //모든 박스가 비활성화 되었을때 충돌하지 않도록 하여 게임 종료를 알림
        //{
        //    StartCoroutine(DeactiveBigWall());

        //}
    }

    IEnumerator DeactiveBigWall()
    {

        yield return new WaitForSeconds(0.5f);

        BigWall.SetActive(false);
        LastPlayer.SetActive(false);

        yield return null;
    }


    public T GetObject<T>() where T : Unit
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
                        return zom as T;
                    }

                }

                return CreateZombie() as T;
            }
            else
                return CreateZombie() as T;

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

    IEnumerator ExecuteEverySeconds()
    {
        while (true)
        {
            GetObject<ZombieMonster>();

            yield return new WaitForSeconds(MonsterCreateCycleSecond);
        }

        //Debug.Log("좀비생성끝");

    }

    public ZombieMonster CreateZombie()
    {
        

        GameObject test = Managers.Resource_Instance.Instantiate("ZombieMelee", ZombieMelee_Root);
        GameObject _zom = Managers.Resource_Instance.Instantiate("Monster/ZombieMelee", ZombieMelee_Root);
        //GameObject _zom = Managers. Instantiate(ZombieMeleeSample, ZombieMelee_Root);

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
        

        //GameObject _bul = Instantiate(BulletSample, Bullet_Root);
        GameObject _bul = null;
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
