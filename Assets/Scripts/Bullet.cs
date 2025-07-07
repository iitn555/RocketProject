using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Unit
{

    public bool bFlying = false;
    Vector3 MovingDirection = new Vector3();

    float fSpeed = 10;

    Player _player;

    private void Awake()
    {
        Damage = 5;
        Init();
        
    }

    public override void Respawn()
    {
        gameObject.SetActive(true);
        bDie = false;
    }

    private void OnEnable()
    {
        bDie = false;
        CurrentHp = MaxHp;

    }

    

    private void Update()
    {

        if (bFlying)
        {
            transform.Translate(MovingDirection * fSpeed * Time.deltaTime);
        }

        
        CheckOutofMap();
    }

    public void SetPlayer(Player _p)
    {
        _player = _p;
    }

    public void StartShot(Vector3 _dir, Vector3 _startpos)
    {
        bFlying = true;
        MovingDirection = _dir;
        transform.position = _startpos;
    }

    void CheckOutofMap()
    {
        if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 10)
        {
            BulletQuit();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.layer == 7) // Monster
        {
            ZombieMelee monster = other.GetComponent<ZombieMelee>();
            if (monster != null)
            {
                monster.GetDamage(Damage, _player, monster);
                BulletQuit();
            }
        }
    }

    void BulletQuit() // 총알 움직임 종료
    {
        bFlying = false;

        bDie = true;
        gameObject.SetActive(false);
    }

    
}
