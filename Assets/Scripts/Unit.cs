using System.Collections;
using System.Collections.Generic;
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

    public GameObject MyGameObject;
    public MYRECT m_tRect = new MYRECT();
    public Vector3 m_vSize = new Vector3();

    private void Start()
    {
        
    }

    protected virtual void Init(float SizeX = 1, float SizeY = 1)
    {
        MyGameObject = this.gameObject;
        m_vSize.x = SizeX;
        m_vSize.y = SizeY;


    }


    //public void UpdateList_OneCGameObject(GameObject AllObjects)
    //{

    //    var m_vPos = AllObjects.transform.position;
    //    //var m_vSize = AllObjects.m_GameObject.transform.localScale;

    //    AllObjects.m_tRect.left = m_vPos.x - m_vSize.x * 0.5f;
    //    AllObjects.m_tRect.right = m_vPos.x + m_vSize.x * 0.5f;
    //    AllObjects.m_tRect.top = m_vPos.y + m_vSize.y * 0.5f;
    //    AllObjects.m_tRect.bottom = m_vPos.y - m_vSize.y * 0.5f;
    //    AllObjects.m_tRect.front = m_vPos.z + m_vSize.z * 0.5f;
    //    AllObjects.m_tRect.back = m_vPos.z - m_vSize.z * 0.5f;
    //    AllObjects.m_tRect.HeadBot = AllObjects.m_tRect.top - 0.1f;
    //    AllObjects.m_tRect.FootTop = AllObjects.m_tRect.bottom + 0.1f;

    //}

}
