using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainGame : MonoBehaviour
{
    private GameObject ZombieMelee;
    private Transform ZombieBox;

    public Dictionary<string, List<Unit>> Dictionary_AllGameObject = new Dictionary<string, List<Unit>>();

    //TEST
    public int MonsterCount = 0;

    private static MainGame Instance = null;
    public static MainGame GetInstance() { Init(); return Instance; }

    static void Init()
    {
        GameObject go = GameObject.Find("MainGame");

        if (go == null)
        {
            go = new GameObject { name = "MainGame" };
            go.AddComponent<MainGame>();
        }

        //DontDestroyOnLoad(go);
        Instance = go.GetComponent<MainGame>();
    }

    void Start()
    {
        ZombieMelee = this.transform.Find("ZombieMelee").gameObject;
        ZombieBox = this.transform.Find("ZombieBox");

        StartCoroutine(ExecuteEverySeconds());

        var tf = this.transform.Find("Truck").Find("BoxTrans");

        for (var i = 0; i < 5; ++i)
        {
            var boxcom = tf.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
            GameObject box = tf.GetChild(i).gameObject;

            boxcom.size = new Vector3(1.9f, 1.4f);

            box.layer = 6; // Wall

            Box BoxComponent = box.AddComponent<Box>();
            RegisterUnit<Box>(BoxComponent, "box");

        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void LateUpdate()
    {
        //BoxPushMonster();
        //CollisionZombies();
    }

    void BoxPushMonster()
    {
        if (!Dictionary_AllGameObject.ContainsKey("zombie"))
            return;

        foreach (var zom2 in Dictionary_AllGameObject["zombie"])
        {
            foreach (var box1 in Dictionary_AllGameObject["box"])
            {
                if (CollisionObjectManagers.bRectCollsionPushFirstObject(zom2, box1))
                {

                    //if(zombie.TestNumber == 1)
                    //    zombie.bAttacking = true;

                }
            }
        }
    }

    void CollisionZombies()
    {
        if (!Dictionary_AllGameObject.ContainsKey("zombie"))
            return;

        foreach (var zom1 in Dictionary_AllGameObject["zombie"])
        {
            foreach (var zom2 in Dictionary_AllGameObject["zombie"])
            {
                if (zom1 != zom2)
                {
                    if (CollisionObjectManagers.bRectCollsionPushTopObject(zom1, zom2))
                    {
                        
                    }
                }
            }
        }

    }

    IEnumerator ExecuteEverySeconds()
    {
        while (true)
        {
            CreateZombie();

            if (Dictionary_AllGameObject["zombie"].Count >= MonsterCount)
                break;

            yield return new WaitForSeconds(1f);
        }

        Debug.Log("좀비생성끝");

    }


    void RegisterUnit<T>(T New_CObj, string _name) where T : Unit, new()
    {
        //T New_CObj2 = new T();
        
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
    }

    void CreateZombie()
    {
        GameObject _zombie = Instantiate(ZombieMelee, ZombieBox);
        _zombie.SetActive(true);
        ZombieMonster zombiecomponent = _zombie.AddComponent<ZombieMonster>();
        _zombie.transform.position = new Vector3(5f, _zombie.transform.position.y, _zombie.transform.position.z);
        var boxcom = _zombie.AddComponent<BoxCollider2D>();
        boxcom.offset = new Vector3(-0.2f, 0.5f);
        boxcom.size = new Vector3(0.6f, 1.2f);

        RegisterUnit<ZombieMonster>(zombiecomponent, "zombie");
        zombiecomponent.TestNumber = Dictionary_AllGameObject["zombie"].Count;

        var Rigid = _zombie.AddComponent<Rigidbody2D>(); // 좀비끼리 충돌처리를 위해
        Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        //Rigid.gravityScale = 1;
        Rigid.gravityScale = 0;

    }


}
