using UnityEngine;

public struct MYRECT
{
    public float left;
    public float top;
    public float right;
    public float bottom;

};

public class Unit : MonoBehaviour
{
    protected GameObject ThisGameObject { get; set; }
    protected int CurrentHp = 0;
    protected int MaxHp = 10;
    protected int Damage = 0;


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

    public MYRECT m_tRect = new MYRECT();

    private void Start()
    {

    }

    public void GetDamage(int _damage)
    {
        CurrentHp -= _damage;

        if(CurrentHp <= 0)
        {
            bDie = true;
            ThisGameObject.SetActive(false);
        }
    }

    public void CheckDie()
    {

    }

    protected virtual void Init()
    {
        ThisGameObject = this.gameObject;
        CurrentHp = MaxHp;
    }

}
