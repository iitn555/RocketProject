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
    private const float AttackCoolDownTimer = 4;
    private float JumpCoolTime = JumpCoolDownTimer;
    private const float JumpCoolDownTimer = 4;
    private float BackPushCoolTime = BackPushCoolDownTimer;
    private const float BackPushCoolDownTimer = 4;

    private bool bBackPushing = false;
    private Coroutine myCoroutine = null;


    public Vector3 MoveDirection = Vector3.left;

    Animator ZombieAnimator;

    private Rigidbody2D m_RigidBody;

    private void OnEnable()
    {
        bDie = false;

        CurrentHp = MaxHp;

        AttackCoolTime = AttackCoolDownTimer;
        JumpCoolTime = JumpCoolDownTimer;
        BackPushCoolTime = BackPushCoolDownTimer;

        if(myCoroutine != null)
        {
            bBackPushing = false;
            StopCoroutine(myCoroutine);
            myCoroutine = null;
        }
        
    }

    private void Awake()
    {
        MaxHp = 10;
        Damage = 1;
        Init();
    }

    private void Start()
    {
        obstacleLayer = LayerMask.GetMask("Wall", "Monster");
        this.gameObject.layer = 7; // Monster
        m_BoxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        OffsetPosition = new Vector3(m_BoxCollider.offset.x, m_BoxCollider.offset.y, 0);

        ZombieAnimator = GetComponent<Animator>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_RigidBody.excludeLayers = LayerMask.GetMask("Wall");

    }


    void Update()
    {
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

        if (GetMyState(P_State.Falling))
        {
            transform.Translate(Vector2.down * fGravity * Time.deltaTime * 0.5f);
        }

       

        if (!GetMyState(P_State.Attacking))
        {
                    Moving();
        }

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    if (!GetMyState(P_State.Jumping))
        //        StartCoroutine(MyJump());

        //}

        

    }

    private void FixedUpdate()
    {


        //if (!GetMyState(P_State.Attacking))
        //{
        //    if (!GetMyState(P_State.Jumping))
        //        Moving();

        //}


    }

    private void LateUpdate()
    {

        if (!GetMyState(P_State.Jumping))
            GroundCheck();

        if (!GetMyState(P_State.Attacking))
            FindOther();
    }

    void FindOther()
    {

        Vector3 LayPosition = transform.position;
        LayPosition.x -= 0.7f;
        LayPosition.y += 0.4f;

        Debug.DrawRay(LayPosition, Vector3.left * rayDistance);
        RaycastHit2D hit = Physics2D.Raycast(LayPosition, Vector2.left, rayDistance, obstacleLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.activeSelf && hit.collider.gameObject.layer == 6) // Wall
            {
                if (AttackCoolTime == AttackCoolDownTimer)
                {


                    Box _box = hit.collider.gameObject.GetComponent<Box>();
                    if (_box != null)
                    {
                        ZombieAnimator.Play("Attack");
                        AttackCoolTime = GetRandomNumber(0, 3f);

                        _box.GetDamage(Damage);
                    }

                }
            }

            if (hit.collider.gameObject.layer == 7) // 몬스터
            {
                if (!bBackPushing || myCoroutine == null)
                {
                    if (GetMyState(P_State.Idle) && JumpCoolTime == JumpCoolDownTimer)
                    {
                        StartCoroutine(MyJump());
                        JumpCoolTime = GetRandomNumber(0, 3f);
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
            SetMyState(P_State.Attacking);

            AttackCoolTime = GetRandomNumber(0, 3f);
        }

    }

    public void OnAttackEnd()
    {

        SetMyState(P_State.Idle);

    }


    public void StartPushBackSide()
    {
        if (!bBackPushing && BackPushCoolTime == BackPushCoolDownTimer)
        {
            bBackPushing = true;
            myCoroutine = StartCoroutine(PushBackSide());

            BackPushCoolTime = GetRandomNumber(0, 3f);
        }

    }

    IEnumerator PushBackSide()
    {
        //Debug.Log("밀기시작");
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

        //Debug.Log("밀기끝");

        bBackPushing = false;
        myCoroutine = null;
        MoveDirection = Vector3.left;

        
    }

    void Moving()
    {
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
        //float m_fJumpingPower = 5.3f;
        float m_fJumpingPower = 5.5f;

        SetMyState(P_State.Jumping);
        //Debug.Log("점프시작");

        float elapsed = 0f;
        float jumpDuration = 3f;

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

        SetMyState(P_State.Idle);
        //Debug.Log("점프종료");

    }


    Collider2D ShotRayDown()
    {
        float GroundRayDistance = 0.01f;
        Vector3 ColliderPosition = transform.position + OffsetPosition;
        ColliderPosition.y -= (m_BoxCollider.size.y * 0.5f) + GroundRayDistance;

        RaycastHit2D hit2 = Physics2D.Raycast(ColliderPosition, Vector2.down, GroundRayDistance);
        //Debug.DrawRay(ColliderPosition, Vector3.down * GroundRayDistance);

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

    float GetRandomNumber(float min, float max)
    {
        float num = Random.Range(min, max);

        return num;
    }
}
