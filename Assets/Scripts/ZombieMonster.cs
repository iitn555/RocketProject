using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZombieMonster : Unit
{
    enum P_State
    {
        Idle,
        Jumping,
        Falling,
        Attacking
    }

    private P_State NowState = P_State.Idle;
    public float fMoveSpeed = 4f;
    private float rayDistance = 0.3f;
    private LayerMask obstacleLayer;
    private Vector2 rayDirection = Vector2.left;
    private float fGravity = 10;

    private BoxCollider2D m_BoxCollider;
    private Vector3 OffsetPosition;
    public int TestNumber = 0;

    public bool bAttacking = false;

    private float AttackCoolTime = AttackCoolDownTimer;
    private const float AttackCoolDownTimer = 2;
    private float JumpCoolTime = JumpCoolDownTimer;
    private const float JumpCoolDownTimer = 2;

    private float BackPushCoolTime = BackPushCoolDownTimer;
    private const float BackPushCoolDownTimer = 2;

    public bool bBackPushing = false;

    public Vector3 MoveDirection = Vector3.left;

    Animator ZombieAnimator;

    private Rigidbody2D m_RigidBody;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        obstacleLayer = LayerMask.GetMask("Wall", "Monster");
        this.gameObject.layer = 7; // Monster
        m_BoxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        OffsetPosition = new Vector3(m_BoxCollider.offset.x, m_BoxCollider.offset.y, 0);

        ZombieAnimator = GetComponent<Animator>();
        AttackCoolTime = AttackCoolDownTimer;

        m_RigidBody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!GetMyState(P_State.Jumping))
                StartCoroutine(MyJump());
        }

        if (GetMyState(P_State.Falling))
        {
            transform.Translate(Vector2.down * fGravity * Time.deltaTime * 0.5f);
        }


        //CheckCollisionBox();

        if (AttackCoolTime < AttackCoolDownTimer)
            AttackCoolTime += Time.deltaTime;
        else
            AttackCoolTime = AttackCoolDownTimer;

        if (JumpCoolTime < JumpCoolDownTimer)
            JumpCoolTime += Time.deltaTime;
        else
            JumpCoolTime = JumpCoolDownTimer;

        if (BackPushCoolTime < BackPushCoolDownTimer)
            BackPushCoolTime += Time.deltaTime;
        else
            BackPushCoolTime = BackPushCoolDownTimer;

        //if (!GetMyState(P_State.Attacking))
        //{
        //        Moving();
        //}
    }

    private void FixedUpdate()
    {
        if (!GetMyState(P_State.Attacking))
            Moving();

    }

    private void LateUpdate()
    {
        //if (!GetMyState(P_State.Attacking) || !GetMyState(P_State.Jumping))
        //    Moving();

        //if (TestNumber == 1)
        //{
        //    if (Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        //transform.Translate(Vector2.left * fMoveSpeed * Time.deltaTime);

        //        if (m_RigidBody.velocity.x > -fMaxSpeed)
        //            m_RigidBody.AddForce(Vector2.left * fMoveSpeed * 10);
        //    }

        //    if (Input.GetKey(KeyCode.RightArrow))
        //    {
        //        //transform.Translate(Vector2.right * fMoveSpeed * Time.deltaTime);
        //        if (m_RigidBody.velocity.x < fMaxSpeed)
        //            m_RigidBody.AddForce(Vector2.right * fMoveSpeed * 10);
        //    }
        //}



        if (!GetMyState(P_State.Jumping))
            GroundCheck();

        if (!GetMyState(P_State.Attacking))
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
            if (hit.collider.gameObject.layer == 6) // Wall
            {
                if (AttackCoolTime == AttackCoolDownTimer)
                {
                    ZombieAnimator.Play("Attack");
                    AttackCoolTime = 0;
                }
            }



            if (hit.collider.gameObject.layer == 7) // 몬스터
            {
                if (!bBackPushing)
                {
                    if (GetMyState(P_State.Idle) && JumpCoolTime == JumpCoolDownTimer)
                    {
                        StartCoroutine(MyJump());
                        JumpCoolTime = 0;
                    }
                }
            }
        }
    }

    void AttackStart()
    {
        ZombieAnimator.Play("Attack");

    }
    public void OnAttack()
    {

        if (!GetMyState(P_State.Attacking) && AttackCoolTime == AttackCoolDownTimer)
        {
            Debug.Log("좀비 공격");
            SetMyState(P_State.Attacking);

            AttackCoolTime = 0;
        }

    }

    public void OnAttackEnd()
    {
        Debug.Log("좀비 공격 종료");

        SetMyState(P_State.Idle);

    }


    public void StartPushBackSide()
    {
        if (!bBackPushing && BackPushCoolTime == BackPushCoolDownTimer)
        {
            bBackPushing = true;
            StartCoroutine(PushBackSide());

            BackPushCoolTime = 0;
        }

    }

    IEnumerator PushBackSide()
    {
        Debug.Log("밀기시작");
        bBackPushing = true;

        float elapsed = 0f;
        float jumpDuration = 0.2f;


        while (elapsed < jumpDuration)
        {
            MoveDirection = Vector3.right;

            elapsed += Time.deltaTime;

            if (elapsed > jumpDuration)
                break;

            yield return null;

        }

        Debug.Log("밀기끝");

        bBackPushing = false;
        MoveDirection = Vector3.left;
    }

    void Moving()
    {
        //transform.Translate(MoveDirection * fMoveSpeed * Time.deltaTime);

        m_RigidBody.velocity = MoveDirection * fMoveSpeed; // 좀비끼리 비비는것을 방지하기 위해

    }

    void GroundCheck()
    {

        Collider2D hitcollider = ShotRayDown();

        if (hitcollider != null)
        {
            SetMyState(P_State.Idle);

            if (hitcollider.gameObject.layer == 7) // 몬스터일때
            {
                ZombieMonster zm = hitcollider.gameObject.GetComponent<ZombieMonster>();
                zm.StartPushBackSide();

            }
        }
        else
        {
            SetMyState(P_State.Falling);
        }
    }


    IEnumerator MyJump()
    {
        float m_fJumpingPower = 5.3f;

        SetMyState(P_State.Jumping);

        float elapsed = 0f;
        float jumpDuration = 2f;

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
        ColliderPosition.y -= (m_BoxCollider.size.y * 0.5f) + GroundRayDistance;

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

    void CheckCollisionBox()
    {
        //if (!MainGame.GetInstance().Dictionary_AllGameObject.ContainsKey("box"))
        //    return;

        //foreach (var box1 in MainGame.GetInstance().Dictionary_AllGameObject["box"])
        //{
        //    if (CollisionObjectManagers.bRectCollsionCheck(this, box1))
        //    {
        //        //if(!GetMyState(P_State.Attacking))
        //        if (AttackCoolTime == AttackCoolDownTimer)
        //            ZombieAnimator.Play("Attack");

        //    }
        //}



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
