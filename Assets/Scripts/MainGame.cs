using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainGame : MonoBehaviour
{
    private GameObject ZombieMelee;

    private Transform ZombieBox;

    public Dictionary<string, List<Unit>> Dictionary_AllCGameObject = new Dictionary<string, List<Unit>>();

    // Start is called before the first frame update
    void Start()
    {
        ZombieMelee = this.transform.Find("ZombieMelee").gameObject;
        ZombieBox = this.transform.Find("ZombieBox");

        StartCoroutine(ExecuteEvery3Seconds());

        var tf = this.transform.Find("Truck").Find("BoxTrans");

        for (var i = 0; i < 5; ++i)
        {
            var boxcom = tf.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
            boxcom.size = new Vector3(1.9f, 1.4f);
            
            tf.GetChild(i).gameObject.layer = 6; // Wall
            //RegisterUnit<Box>(tf.GetChild(i).GetComponent<Box>(), "box");

        }


    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator ExecuteEvery3Seconds()
    {
        while (true)
        {
            CreateZombie();

            if (Dictionary_AllCGameObject["zombie"].Count >= 4)
                break;

            yield return new WaitForSeconds(1f);
        }

        Debug.Log("좀비생성끝");

    }


    void RegisterUnit<T>(T New_CObj, string _name) where T : Unit, new()
    {
        //T New_CObj = new T();

        if (!Dictionary_AllCGameObject.ContainsKey(_name))
        {
            List<Unit> New_ = new List<Unit>();
            Dictionary_AllCGameObject.Add(_name, New_);
            Dictionary_AllCGameObject[_name].Add(New_CObj);
        }
        else
        {
            Dictionary_AllCGameObject[_name].Add(New_CObj);
        }
    }

    void CreateZombie()
    {
        GameObject _zombie = Instantiate(ZombieMelee, ZombieBox);
        _zombie.SetActive(true);
        _zombie.AddComponent<MonsterClimber2D>();
        var boxcom = _zombie.AddComponent<BoxCollider2D>();

        boxcom.offset = new Vector3(-0.2f, 0.5f);
        //boxcom.size = new Vector3(0.9f, 1.2f);
        boxcom.size = new Vector3(0.5f, 1.2f);

        //_zombie.AddComponent<Rigidbody>();

        _zombie.transform.position = new Vector3(2f, _zombie.transform.position.y, _zombie.transform.position.z);

        MonsterClimber2D zombiecomponent = _zombie.GetComponent<MonsterClimber2D>();
        RegisterUnit<MonsterClimber2D>(zombiecomponent, "zombie");

        

    }

  
}
