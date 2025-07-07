using UnityEngine;
using UnityEngine.UI;

public struct MYRECT
{
    public float left;
    public float top;
    public float right;
    public float bottom;

};

public abstract class Unit : MonoBehaviour
{
    protected GameObject ThisGameObject { get; set; }

    protected int Exp = 0;
    protected int Level = 1;


    protected float CurrentHp = 0;
    protected float MaxHp = 10;
    protected float Damage = 0;

    protected Slider Hpbar = null;

    public MYRECT m_tRect = new MYRECT();

    public abstract void Respawn(); // 죽은상태에서 살아났을때 반드시 실행.

    public void CommonRespawn() // 오브젝트 풀링, 죽은상태에서 살아났을때 반드시 실행.
    {
        gameObject.SetActive(true);
        bDie = false;
        //transform.position = RespawnPosition;

        if (Hpbar != null)
            Hpbar.value = CurrentHp / MaxHp;
    }

    public bool bDie
    {
        get;
        protected set;

    }

    public void SetDie(bool _bdie)
    {
        bDie = _bdie;
    }

    public GameObject Get_GameObject
    {
        get
        {
            return ThisGameObject;
        }
    }


    public void GetDamage(float _damage, Unit _attacker, Unit _victim)
    {
        CurrentHp -= _damage;

        if(Hpbar != null)
            Hpbar.value = CurrentHp / MaxHp;

        if(CurrentHp <= 0)
        {
            bDie = true;
            ThisGameObject.SetActive(false);

            
            GiveExp(_attacker, _victim);

        }
    }

    protected void GiveExp(Unit _attacker, Unit _victim)
    {
        _attacker.GetExp(_victim.Exp);
    }

    protected virtual void GetExp(int _exp)
    {

    }

    



    protected virtual void Init()
    {
        ThisGameObject = this.gameObject;
        CurrentHp = MaxHp;

        if (gameObject.transform.Find("HpPanel") != null)
            Hpbar = gameObject.transform.Find("HpPanel").GetChild(0).GetChild(0).GetComponent<Slider>();


    }

}
