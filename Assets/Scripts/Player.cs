using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private float AttackCoolTime = AttackCoolDownTimer;
    private const float AttackCoolDownTimer = 1;

    Bullet[] m_Bullets = new Bullet[12];

    int count = 0;

    private void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shot();
        }

        if (AttackCoolTime == AttackCoolDownTimer)
        {
            Shot();
            AttackCoolTime = 0;
        }

        if (AttackCoolTime < AttackCoolDownTimer)
            AttackCoolTime += Time.deltaTime;
        else
            AttackCoolTime = AttackCoolDownTimer;


        

    }


    public void ReadyBullet(Bullet[] _Bullets)
    {
        m_Bullets = _Bullets;
    }

    void Shot()
    {
        

        if (!Managers.Pool_Instance.Dictionary_AllGameObject.ContainsKey("zombie"))
            return;

        ZombieMonster ZM = FindCloseMonster();

        if (ZM == null)
            return;

        var dir = (ZM.transform.position - transform.position).normalized;

        Bullet bul = Managers.Pool_Instance.GetObject<Bullet>();
        bul.StartShot(dir, transform.position);
      
    }

    ZombieMonster FindCloseMonster()
    {
        if (!Managers.Pool_Instance.Dictionary_AllGameObject.ContainsKey("zombie"))
            return null;

        float MinDistance = 9999;
        ZombieMonster CloseMonster = null;

        foreach (var mon1 in Managers.Pool_Instance.Dictionary_AllGameObject["zombie"])
        {

            if (mon1.bDie) // 죽어있는 몬스터는 패스
                continue;

            float dis = Vector3.Distance(mon1.transform.position, transform.position);

            if (dis < MinDistance)
            {
                MinDistance = dis;
                CloseMonster = (ZombieMonster)mon1;
            }

        }

        return CloseMonster;
    }

}
