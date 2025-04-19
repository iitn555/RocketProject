using UnityEngine;
using System.Collections;

public class MonsterClimber2D : Unit
{
    enum P_State
    {
        Idle,
        Jumping,
        Falling
    }

    private P_State NowState = P_State.Idle;
    private float moveSpeed = 5f;
    private float rayDistance = 0.3f;
    private LayerMask obstacleLayer;
    private Vector2 rayDirection = Vector2.left;
    private float fGravity = 10;

    private BoxCollider2D m_BoxColliderComponent;
    private Vector3 OffsetPosition;
    public int TestNumber = 0;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        obstacleLayer = LayerMask.GetMask("Monster");
        this.gameObject.layer = 7; // Monster
        m_BoxColliderComponent = this.gameObject.GetComponent<BoxCollider2D>();
        OffsetPosition = new Vector3(m_BoxColliderComponent.offset.x, m_BoxColliderComponent.offset.y, 0);

    }

    void Update()
    {

        if (Input.GetKey(KeyCode.C))
        {
            if (!GetMyState(P_State.Jumping))
                StartCoroutine(MyJump());
        }

        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (GetMyState(P_State.Falling) && !this.gameObject.GetComponent<Rigidbody2D>())
        {
            transform.Translate(Vector2.down * fGravity * Time.deltaTime * 0.5f);
        }

        if (!GetMyState(P_State.Jumping))
            GroundCheck();

        FindOther();
    }

    void FindOther()
    {

        Vector3 LayPosition = transform.position;
        LayPosition.x -= 0.7f;
        LayPosition.y += 0.8f;

        Debug.DrawRay(LayPosition, Vector3.left * rayDistance);
        RaycastHit2D hit = Physics2D.Raycast(LayPosition, Vector2.left, rayDistance, obstacleLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == 7) // Monster
            {
                if (!GetMyState(P_State.Jumping))
                    StartCoroutine(MyJump());
            }

        }


    }

    void GroundCheck()
    {
       
        Collider2D hitcollider = ShotRayDown();

        if (hitcollider != null)
        {
            SetMyState(P_State.Idle);
        }
        else
        {
            SetMyState(P_State.Falling);
        }
    }


    IEnumerator MyJump()
    {
        float m_fJumpingPower = 5;

        SetMyState(P_State.Jumping);

        float elapsed = 0f;
        float jumpDuration = 3f;

        Debug.Log("점프시작!");

        while (elapsed < jumpDuration)
        {
            transform.Translate(Vector2.up * m_fJumpingPower * Time.deltaTime);

            if (m_fJumpingPower > -fGravity)
            {
                m_fJumpingPower -= Time.deltaTime * fGravity;
            }


            Collider2D hitcollider = ShotRayDown();
            if (hitcollider != null)
                break;

            elapsed += Time.deltaTime;

            yield return null;
        }

        Debug.Log("점프끝!");

        SetMyState(P_State.Idle);


    }


    Collider2D ShotRayDown()
    {
        float GroundRayDistance = 0.01f;
        Vector3 ColliderPosition = transform.position + OffsetPosition;
        ColliderPosition.y -= (m_BoxColliderComponent.size.y * 0.5f) + GroundRayDistance;

        RaycastHit2D hit2 = Physics2D.Raycast(ColliderPosition, Vector2.down, GroundRayDistance);
        Debug.DrawRay(ColliderPosition, Vector3.down * GroundRayDistance);

        if (hit2.collider == null)
            return null;
        else
        {
            if (hit2.collider.gameObject == this.gameObject) // 자기자신이면 취소
                return null;
            else
                return hit2.collider;

        }

    }


    bool GetMyState(P_State currentstate)
    {
        if (NowState == currentstate)
            return true;
        else
            return false;
    }

    void SetMyState(P_State state)
    {
        if (NowState != state)
        {
            NowState = state;
        }
    }
}
