using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainGame : MonoBehaviour
{

    private float ZombieGenerationCycleTime = 0.9f;

    public GameObject BigWall;

    void Start()
    {

        Managers.Input_Instance.Test();

        StartCoroutine(ExecuteEverySeconds());
        //Managers.Pool_Instance.MakeOrGetObject<ZombieMelee>();


    }

    IEnumerator ExecuteEverySeconds() // 좀비 계속 생성
    {
        Debug.Log("좀비 생성 시작!");

        while (true)
        {
            Managers.Pool_Instance.MakeOrGetObject<ZombieMelee>();

            yield return new WaitForSeconds(ZombieGenerationCycleTime);

        }


    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Managers.Pool_Instance.MakeOrGetObject<ZombieMelee>();
        }

        CheckGameEnd();
    }

    private void LateUpdate()
    {
        //CollisionZombies();
    }

    void CheckGameEnd()
    {
        if (!BigWall.activeSelf)
            return;

        bool checkbox = false;

        if (Managers.Pool_Instance.GetObject<Box>() != null)
        {
            checkbox = true;

        }
        //for (int i = 0; i < 5; ++i)
        //{

        //    if (Managers.Pool_Instance.GetObject<Box>().gameObject)
        //    {
        //        checkbox = true;
        //    }
        //}

        if (!checkbox) //모든 박스가 비활성화 되었을때 충돌하지 않도록 하여 게임 종료를 알림
        {
            StartCoroutine(DeactiveBigWall());
        }
    }

    IEnumerator DeactiveBigWall()
    {

        yield return new WaitForSeconds(0.5f);

        BigWall.SetActive(false);
        Managers.Pool_Instance.GetObject<Player>().gameObject.SetActive(false);

        yield return null;
    }











    //void CollisionZombies()
    //{
    //    if (!Dictionary_AllGameObject.ContainsKey(typeof(ZombieMelee).Name))
    //        return;

    //    foreach (var zom1 in Dictionary_AllGameObject[typeof(ZombieMelee).Name])
    //    {
    //        foreach (var zom2 in Dictionary_AllGameObject[typeof(ZombieMelee).Name])
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
