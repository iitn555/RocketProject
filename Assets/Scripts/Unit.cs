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

    protected virtual void Init()
    {
        ThisGameObject = this.gameObject;
    }

}
