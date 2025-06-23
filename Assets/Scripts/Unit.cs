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
    protected float CurrentHp = 0;
    protected float MaxHp = 10;
    protected float Damage = 0;

    protected Slider Hpbar = null;

    public MYRECT m_tRect = new MYRECT();

    public abstract void Respawn(); // 죽은상태에서 살아났을때 반드시 실행.

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


    public void GetDamage(float _damage)
    {
        CurrentHp -= _damage;

        if(Hpbar != null)
        {
            Hpbar.value = CurrentHp / MaxHp;
        }

        if(CurrentHp <= 0)
        {
            bDie = true;
            ThisGameObject.SetActive(false);
        }
    }



    protected virtual void Init()
    {
        ThisGameObject = this.gameObject;
        CurrentHp = MaxHp;

        var a = gameObject.transform.Find("HpPanel");
        if (a == null)
            return;

        var b = a.GetChild(0);
        var c = b.GetChild(0);

        

        Hpbar = gameObject.transform.Find("HpPanel").GetChild(0).GetChild(0).GetComponent<Slider>();


    }

}
