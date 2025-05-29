using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Unit
{

    public bool bFlying = false;
    Vector3 MovingDirection = new Vector3();

    float fSpeed = 10;

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

    private void Awake()
    {
        Damage = 5;
        Init();
    }

    private void Update()
    {

        if (bFlying)
        {
            transform.Translate(MovingDirection * fSpeed * Time.deltaTime);
        }

        
        CheckOutofMap();
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
            SetDie();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.layer == 7) // Monster
        {
            ZombieMelee monster = other.GetComponent<ZombieMelee>();
            if (monster != null)
            {
                monster.GetDamage(Damage);
                SetDie();
            }
        }
    }

    void SetDie() // 총알 움직임 종료
    {
        bFlying = false;

        bDie = true;
        gameObject.SetActive(false);
    }

    
}
