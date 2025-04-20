        using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CollisionObjectManagers : MonoBehaviour
{


    public static bool bRectCollsionCheck(Unit first, Unit second)
    {

        UpdateList_OneCGameObject(first);
        UpdateList_OneCGameObject(second);

        MYRECT rc1 = first.m_tRect;
        MYRECT rc2 = second.m_tRect;

        if (rc1.left < rc2.right &&
            rc1.right > rc2.left &&
            rc1.top > rc2.bottom &&
            rc1.bottom < rc2.top)
            return true;

        return false;
    }



    public static bool bRectCollsionPushTopObject(Unit first, Unit second)
    {

        UpdateList_OneCGameObject(first);
        UpdateList_OneCGameObject(second);

        MYRECT rc1 = first.m_tRect;
        MYRECT rc2 = second.m_tRect;

        if (rc1.left < rc2.right &&
            rc1.right > rc2.left &&
            rc1.top > rc2.bottom &&
            rc1.bottom < rc2.top)
        {
            PushUpPositionObject(first, second);

            return true;

        }

        return false;
    }

    public static void PushUpPositionObject(Unit first, Unit second) // B가 A를 밀음
    {
        UpdateList_OneCGameObject(first);
        UpdateList_OneCGameObject(second);

        MYRECT rc1 = first.m_tRect;
        MYRECT rc2 = second.m_tRect;

        var rDestObjPosition = first.transform.position;
        var pBoxPosition = second.transform.position;

        if (rDestObjPosition.x < pBoxPosition.x) // rDestObj 가 왼쪽
        {
            if (rDestObjPosition.y < pBoxPosition.y)
            {

                float A = rc1.right - rc2.left;
                float B = rc1.top - rc2.bottom;
                //if (A < B) // 겹친 면적을 비교해서 어디서 부딪혔는지 추정
                //    rDestObjPosition.x -= A;
                //else
                //    rDestObjPosition.y -= B;

                if (A < B) // 반드시 위에있는 물체가 움직이도록
                    pBoxPosition.x += A;
                else
                    pBoxPosition.y += B;

            }
            else //rDestObj가 아래쪽
            {
                float A = rc1.right - rc2.left;
                float B = rc2.top - rc1.bottom;
                if (A < B)
                    rDestObjPosition.x -= A;
                else
                    rDestObjPosition.y += B;
            }
        }
        else
        {
            if (rDestObjPosition.y < pBoxPosition.y) 
            {
                float A = rc2.right - rc1.left;
                float B = Mathf.Abs(rc2.bottom - rc1.top);
                //if (A < B)
                //    rDestObjPosition.x += A;
                //else
                //    rDestObjPosition.y -= B;
                 
                if (A < B) // 반드시 위에있는 물체가 움직이도록
                    pBoxPosition.x -= A;
                else
                    pBoxPosition.y += B;
            }
            else
            {
                float A = rc2.right - rc1.left;
                float B = rc2.top - rc1.bottom;
                if (A < B)
                    rDestObjPosition.x += A;
                else
                    rDestObjPosition.y += B;
            }
        }


        first.transform.position = rDestObjPosition;
        second.transform.position = pBoxPosition;

    }



    public static bool bRectCollsionPushFirstObject(Unit first, Unit second)
    {

        UpdateList_OneCGameObject(first);
        UpdateList_OneCGameObject(second);


        MYRECT rc1 = first.m_tRect;
        MYRECT rc2 = second.m_tRect;

        if (rc1.left < rc2.right &&
            rc1.right > rc2.left &&
            rc1.top > rc2.bottom &&
            rc1.bottom < rc2.top)
        {
            PushDestObjPosition(first, second);

            return true;

        }

        return false;
    }

    public static void PushDestObjPosition(Unit first, Unit second) // B가 A를 밀음
    {
        UpdateList_OneCGameObject(first);   
        UpdateList_OneCGameObject(second);

        MYRECT rc1 = first.m_tRect;
        MYRECT rc2 = second.m_tRect;

        var rDestObjPosition = first.transform.position;
        var pBoxPosition = second.transform.position;

        if (rDestObjPosition.x < pBoxPosition.x) // rDestObj 가 왼쪽
        {
            if (rDestObjPosition.y < pBoxPosition.y)
            {

                float A = rc1.right - rc2.left;
                float B = rc1.top - rc2.bottom;
                if (A < B) // 겹친 면적을 비교해서 어디서 부딪혔는지 추정
                    rDestObjPosition.x -= A;
                else
                    rDestObjPosition.y -= B;
            }
            else //rDestObj가 아래쪽
            {
                float A = rc1.right - rc2.left;
                float B = rc2.top - rc1.bottom;
                if (A < B)
                    rDestObjPosition.x -= A;
                else
                    rDestObjPosition.y += B;
            }
        }
        else
        {
            if (rDestObjPosition.y < pBoxPosition.y)
            {
                float A = rc2.right - rc1.left;
                float B = Mathf.Abs(rc2.bottom - rc1.top);
                if (A < B)
                    rDestObjPosition.x += A;
                else
                    rDestObjPosition.y -= B;
            }
            else
            {
                float A = rc2.right - rc1.left;
                float B = rc2.top - rc1.bottom;
                if (A < B)
                    rDestObjPosition.x += A;
                else
                    rDestObjPosition.y += B;
            }
        }


        first.transform.position = rDestObjPosition;

    }


    public static void UpdateList_OneCGameObject(Unit AllObjects)
    {

        var m_vPos = AllObjects.Get_GameObject.transform.position;
        var m_vSize = AllObjects.Get_GameObject.transform.localScale;

        var boxcol = AllObjects.Get_GameObject.GetComponent<BoxCollider2D>();
        if(boxcol)
        {
            var OffsetPosition = new Vector3(boxcol.offset.x, boxcol.offset.y, 0);
            m_vPos = AllObjects.Get_GameObject.transform.position + OffsetPosition;
            m_vSize.x = boxcol.size.x;
            m_vSize.y = boxcol.size.y;
        }


        AllObjects.m_tRect.left = m_vPos.x - m_vSize.x * 0.5f;
        AllObjects.m_tRect.right = m_vPos.x + m_vSize.x * 0.5f;
        AllObjects.m_tRect.top = m_vPos.y + m_vSize.y * 0.5f;
        AllObjects.m_tRect.bottom = m_vPos.y - m_vSize.y * 0.5f;

        //Vector3 Start = new Vector3(AllObjects.m_tRect.left, AllObjects.m_tRect.top);
        //Vector3 End = new Vector3(AllObjects.m_tRect.right, AllObjects.m_tRect.top);
        //Debug.DrawLine(Start, End);

        //Start = new Vector3(AllObjects.m_tRect.left, AllObjects.m_tRect.bottom);
        //End = new Vector3(AllObjects.m_tRect.right, AllObjects.m_tRect.bottom);
        //Debug.DrawLine(Start, End);
    }














    //public static void PushDestObjPosition(GameObject rDestObj, GameObject pBox)
    //{
    //    MYRECT rc1 = rDestObj.m_tRect;
    //    MYRECT rc2 = pBox.m_tRect;

    //    var rDestObjPosition = rDestObj.transform.position;
    //    var pBoxPosition = pBox.transform.position;

    //    if (rDestObjPosition.x < pBoxPosition.x) // rDestObj 가 왼쪽
    //    {
    //        if (rDestObjPosition.y < pBoxPosition.y) // rDestObj가 위쪽
    //        {

    //            float A = rc1.right - rc2.left;
    //            float B = rc1.top - rc2.bottom;
    //            if (A < B) // 겹친 면적을 비교해서 어디서 부딪혔는지 추정
    //                rDestObjPosition.x -= A;
    //            else
    //                rDestObjPosition.y -= B;
    //        }
    //        else //rDestObj가 아래쪽
    //        {
    //            float A = rc1.right - rc2.left;
    //            float B = rc2.top - rc1.bottom;
    //            if (A < B)
    //                rDestObjPosition.x -= A;
    //            else
    //                rDestObjPosition.y += B;
    //        }
    //    }
    //    else
    //    {
    //        if (rDestObjPosition.y < pBoxPosition.y)
    //        {
    //            float A = rc2.right - rc1.left;
    //            float B = Mathf.Abs(rc2.bottom - rc1.top);
    //            if (A < B)
    //                rDestObjPosition.x += A;
    //            else
    //                rDestObjPosition.y -= B;
    //        }
    //        else
    //        {
    //            float A = rc2.right - rc1.left;
    //            float B = rc2.top - rc1.bottom;
    //            if (A < B)
    //                rDestObjPosition.x += A;
    //            else
    //                rDestObjPosition.y += B;
    //        }
    //    }


    //    rDestObj.transform.position = rDestObjPosition;

    //}




}
