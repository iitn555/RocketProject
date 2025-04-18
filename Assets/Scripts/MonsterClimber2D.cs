using UnityEngine;
using System.Collections;

public class MonsterClimber2D : Unit
{
    public float moveSpeed = 5f;
    public float rayDistance = 0.3f;
    public float climbHeight = 1.2f; //boxcollider2D.size.y = 1.2f
    public float jumpHeight = 1f;
    public LayerMask obstacleLayer;
    public LayerMask GroundLayer;
    
    public Vector2 rayDirection = Vector2.left;

    //private bool isJumping = false;
    private bool Falling = false;

    BoxCollider2D m_BoxColliderComponent;
    Vector3 OffsetPosition;

    private void Start()
    {
        obstacleLayer = LayerMask.GetMask("Wall", "Monster");
        GroundLayer = LayerMask.GetMask("Ground");
        //obstacleLayer = LayerMask.GetMask("Wall");
        this.gameObject.layer = 7; // Monster

        m_BoxColliderComponent = this.gameObject.GetComponent<BoxCollider2D>();

        OffsetPosition = new Vector3(m_BoxColliderComponent.offset.x, m_BoxColliderComponent.offset.y, 0);
        

    }

    void Update()
    {

        if (Input.GetKey(KeyCode.C))
        {
            if (!Falling)
                StartCoroutine(MyJump());
        }

       

    }

    private void LateUpdate()
    {
        FindOther();
    }

    void FindOther()
    {
        //if (isJumping) return;

        Vector3 LayPosition = transform.position;
        LayPosition.x -= 0.7f;
        LayPosition.y += 0.8f;

        Debug.DrawRay(LayPosition, Vector3.left * rayDistance);
        RaycastHit2D hit = Physics2D.Raycast(LayPosition, Vector2.left, rayDistance, obstacleLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == 7) // Monster
            {
                Vector3 targetPos = hit.collider.transform.position + Vector3.up * climbHeight;

                //StartCoroutine(JumpTo(targetPos));

                if (!Falling)
                    StartCoroutine(MyJump());
            }

            //Debug.Log("충돌!");

        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }

        //transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }
    IEnumerator MyJump()
    {
        float m_fJumpingPower = 10;
        float Gravity = 10;
        Falling = true;

        bool bNowSky = true;
        float elapsed = 0f;
        float jumpDuration = 0.2f;

        //while (bNowSky)
        while (elapsed < jumpDuration)
        {
            transform.Translate(Vector2.up * m_fJumpingPower * Time.deltaTime);

            //if(m_fJumpingPower > -Gravity )
            if (m_fJumpingPower > 0)
            {
                m_fJumpingPower -= Time.deltaTime * Gravity;
            }
            else
                m_fJumpingPower = 0;

            Vector3 LayPosition = transform.position;
            LayPosition.x -= 0.7f;
            LayPosition.y += 0.8f;


            Vector3 ColliderPosition = transform.position + OffsetPosition;
            RaycastHit2D hit2 = Physics2D.Raycast(ColliderPosition, Vector2.down, 0.6f, GroundLayer);
            Debug.DrawRay(ColliderPosition, Vector3.down * 0.6f);
            if (hit2.collider != null)
            {
                if (hit2.collider.gameObject.layer == 8)
                {

                    bNowSky = false;
                    Debug.Log("바닥충돌!");

                    break;


                }
            }

            elapsed += Time.deltaTime;

            yield return null;
        }

        Falling = false;
        
    }
    void OnDrawGizmos()
    {
        //Vector3 LayPosition = transform.position;
        //LayPosition.x -= 0.7f;
        //LayPosition.y += 0.8f;

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(LayPosition, LayPosition + Vector3.left * rayDistance );
        //Gizmos.DrawLine(LayPosition, transform.position + Vector3.left * detectDistance);
    }
}
