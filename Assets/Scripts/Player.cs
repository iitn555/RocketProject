using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    
    private const float AttackCoolDownTimer = 1;

    CooldownManager CoolDownManager = new CooldownManager();

    Bullet[] m_Bullets = new Bullet[12];

    int count = 0;

    public bool bPlayerAttackStart = true; // 테스트용

    public override void Respawn()
    {

    }

    private void Awake()
    {
        Init();

        CoolDownManager.RegisterSkill("Shot", AttackCoolDownTimer);
    }

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Shot();
        }

        if (bPlayerAttackStart)
        {
            if (CoolDownManager.CheckCooldownSkill("Shot"))
                Shot(); //플레이어가 몬스터를 공격
        }

    }


    public void ReadyBullet(Bullet[] _Bullets)
    {
        m_Bullets = _Bullets;
    }

    void Shot()
    {
        

        if (!Managers.Pool_Instance.Dictionary_AllGameObject.ContainsKey(typeof(ZombieMelee).Name))
            return;

        ZombieMelee ZM = FindCloseMonster();

        if (ZM == null)
            return;

        var dir = (ZM.transform.position - transform.position).normalized;

        Bullet bul = Managers.Pool_Instance.MakeOrGetObject<Bullet>();
        bul.StartShot(dir, transform.position);
      
    }

    ZombieMelee FindCloseMonster()
    {
        if (!Managers.Pool_Instance.Dictionary_AllGameObject.ContainsKey(typeof(ZombieMelee).Name))
            return null;

        float MinDistance = 9999;
        ZombieMelee CloseMonster = null;

        foreach (var mon1 in Managers.Pool_Instance.Dictionary_AllGameObject[typeof(ZombieMelee).Name])
        {

            if (mon1.bDie) // 죽어있는 몬스터는 패스
                continue;

            float dis = Vector3.Distance(mon1.transform.position, transform.position);

            if (dis < MinDistance)
            {
                MinDistance = dis;
                CloseMonster = (ZombieMelee)mon1;
            }

        }

        return CloseMonster;
    }

}
