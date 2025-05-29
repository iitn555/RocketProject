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

    public T MakeOrGetObject<T>() where T : Unit
    {

        if (Dictionary_AllGameObject.ContainsKey(typeof(T).Name))
        {
            foreach (var _Object in Dictionary_AllGameObject[typeof(T).Name])
            {
                if (_Object.bDie)
                {
                    _Object.Respawn();
                    return _Object as T;
                }

            }
            return CreateObject<T>() as T;
        }
        else
        {
            return CreateObject<T>() as T;
        }

    }

    public T GetObject<T>() where T : Unit // 여기개선?
    {
        if (typeof(T) == typeof(T))
        {
            if (Dictionary_AllGameObject.ContainsKey(typeof(T).Name))
            {
                foreach (var obj in Dictionary_AllGameObject[typeof(T).Name])
                {
                    if (!obj.bDie) // 죽어있지않다면 
                    {
                        return obj as T; //반환
                    }

                }
            }
        }

        return null;
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
        RegisterUnit<Player>(_playercomponent, typeof(Player).Name);

        
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
            RegisterUnit<Box>(BoxComponent, typeof(Box).Name);
            AllBox[i] = box;

        }
    }

    public T CreateObject<T>() where T : Unit
    {
        GameObject _unitobject = Managers.Resource_Instance.Instantiate(typeof(T).Name, _root);
        T _unitcomponent = _unitobject.AddComponent<T>();
        RegisterUnit<T>(_unitcomponent, typeof(T).Name);
        _unitcomponent.Respawn();

        return _unitcomponent;
    }

    T RegisterUnit<T>(T New_CObj, string _name) where T : Unit

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


 

}
