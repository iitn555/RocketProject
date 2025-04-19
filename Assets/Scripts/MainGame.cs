using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainGame : MonoBehaviour
{
    private GameObject ZombieMelee;
    private Transform ZombieBox;
    private Dictionary<string, List<Unit>> Dictionary_AllGameObject = new Dictionary<string, List<Unit>>();

    //TEST
    public int MonsterCount = 0;

    void Start()
    {
        ZombieMelee = this.transform.Find("ZombieMelee").gameObject;
        ZombieBox = this.transform.Find("ZombieBox");

        StartCoroutine(ExecuteEvery3Seconds());

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
        BoxPushMonster();
        CollisionTest();
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
                    
                }
            }
        }
    }

    void CollisionTest()
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

    IEnumerator ExecuteEvery3Seconds()
    {
        while (true)
        {
            CreateZombie();

            if (Dictionary_AllGameObject["zombie"].Count >= MonsterCount)
                break;

            yield return new WaitForSeconds(2f);
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
        MonsterClimber2D zombiecomponent = _zombie.AddComponent<MonsterClimber2D>();
        _zombie.transform.position = new Vector3(5f, _zombie.transform.position.y, _zombie.transform.position.z);
        var boxcom = _zombie.AddComponent<BoxCollider2D>();
        boxcom.offset = new Vector3(-0.2f, 0.5f);
        boxcom.size = new Vector3(0.5f, 1.2f);

        RegisterUnit<MonsterClimber2D>(zombiecomponent, "zombie");
        zombiecomponent.TestNumber = Dictionary_AllGameObject["zombie"].Count;


    }


}
