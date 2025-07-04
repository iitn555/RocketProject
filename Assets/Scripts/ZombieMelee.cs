using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZombieMelee : Unit
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

    
    private const float AttackCoolDownTimer = 4;
    private const float JumpCoolDownTimer = 4;
    private const float BackPushCoolDownTimer = 4;

    private bool bBackPushing = false;
    private Coroutine myCoroutine = null;


    public Vector3 MoveDirection = Vector3.left;

    Animator ZombieAnimator;

    private Rigidbody2D m_RigidBody;

    Vector3 RespawnPosition = new Vector3(5f, -2.9f);


    CooldownManager ZombieMelee_CDManager = new CooldownManager(); // 몬스터마다 각각 쿨다운 매니저 할당

    enum SkillNames
    {
        ZombieMeleeAttack, 
        ZombieMeleeJump,
        ZombieMeleeBackPush
    }
    public override void Respawn()
    {
        gameObject.SetActive(true);
        bDie = false;
        transform.position = RespawnPosition;

    }
    private void Awake()
    {
        MaxHp = 10;
        Damage = 1;
        Init();

        var boxcom = gameObject.AddComponent<BoxCollider2D>();
        boxcom.offset = new Vector3(-0.2f, 0.5f);
        boxcom.size = new Vector3(0.6f, 1.2f);
        var Rigid = gameObject.AddComponent<Rigidbody2D>(); // 좀비끼리 충돌처리를 위해
        Rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rigid.gravityScale = 0;

        if (Managers.Pool_Instance.Dictionary_AllGameObject.ContainsKey(typeof(ZombieMelee).Name))
            TestNumber = Managers.Pool_Instance.Dictionary_AllGameObject[typeof(ZombieMelee).Name].Count;



        //Managers.Cooldown_Instance.RegisterSkill(SkillNames.ZombieMeleeAttack.ToString(), AttackCoolDownTimer);


        ZombieMelee_CDManager.RegisterSkill(SkillNames.ZombieMeleeAttack.ToString(), AttackCoolDownTimer);
        ZombieMelee_CDManager.RegisterSkill(SkillNames.ZombieMeleeJump.ToString(), JumpCoolDownTimer);
        ZombieMelee_CDManager.RegisterSkill(SkillNames.ZombieMeleeBackPush.ToString(), BackPushCoolDownTimer);

    }

    private void OnEnable()
    {
        bDie = false;
        CurrentHp = MaxHp;

        if (myCoroutine != null)
        {
            bBackPushing = false;
            StopCoroutine(myCoroutine);
            myCoroutine = null;
        }

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

        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }
        

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

                if (ZombieMelee_CDManager.CheckCooldownSkill(SkillNames.ZombieMeleeAttack.ToString())) // 작성중
                {
                    Box _box = hit.collider.gameObject.GetComponent<Box>();
                    if (_box != null)
                    {
                        ZombieAnimator.Play("Attack");
                        SetMyState(P_State.Attacking);
                        //AttackCoolTime = GetRandomNumber(0, 3f); // 쿨타임 재설정 하기 넣을까...?
                        _box.GetDamage(Damage);
                    }

                }

                
            }

            if (hit.collider.gameObject.layer == 7) // 몬스터
            {
                if (!bBackPushing || myCoroutine == null)
                {

                    

                    if (GetMyState(P_State.Idle) && ZombieMelee_CDManager.CheckCooldownSkill(SkillNames.ZombieMeleeJump.ToString()))
                    {
                        StartCoroutine(MyJump());
                        ZombieMelee_CDManager.ResetCooldownTime(SkillNames.ZombieMeleeJump.ToString(), GetRandomNumber(0, 3f));
                    }
                }
            }
        }
    }

    

    public void OnAttackEnd()
    {
        //Debug.Log("OnAttackEnd");
        SetMyState(P_State.Idle);

    }


    public void StartPushBackSide()
    {
        if (!bBackPushing && ZombieMelee_CDManager.CheckCooldownSkill(SkillNames.ZombieMeleeBackPush.ToString()))
        {
            bBackPushing = true;
            myCoroutine = StartCoroutine(PushBackSide());
            ZombieMelee_CDManager.ResetCooldownTime(SkillNames.ZombieMeleeBackPush.ToString(), GetRandomNumber(0, 3f));
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
                ZombieMelee zm = hitcollider.gameObject.GetComponent<ZombieMelee>();
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
