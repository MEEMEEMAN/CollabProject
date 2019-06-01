using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        [Header("Setup")]
        public QuatCamController PlayerCamera;
        public CapsuleCollider MainCollider;
        public LayerMask raycastIncludeMask;
        public float maxSlopeAngle = 50f;
        public float groundRayLength = 2f;
        public bool debugDraw = true;
        [Header("GamePlay")]
        public float movementSpeed = 5f;
        public float sprintMultiplier = 2f;
        public float crouchMultiplier = 0.5f;
        public float momentumSwitch = 10f;
    }
    public Settings PlayerSettings;

    GamePlayer gp;
    Animator localAnimator;
    CustomInputManager m_input = new CustomInputManager();
    Vector3 movementSum = Vector3.zero;
    Rigidbody rb;
    bool m_sprint = false;
    bool m_crouch = false;
    float movementFactor = 0f;
    
    private void Start()
    {
        //gp = GameManager.instance.GetLocalPlayer();
        gp = transform.root.GetComponent<GamePlayer>();
        localAnimator = transform.root.GetComponent<Animator>();
        rb = transform.root.GetComponent<Rigidbody>();
        timer = timerStart;
        pastPos = transform.root.position;
    }

    private void Update()
    {
        CollectInput();
        Rotate();
        GroundProbe();
        CollectVectors();
        MovementManage();
        CorrectPosition();
        FinalMove();
    }

    float movFactor = 1f;
    public void SetMovFactor(float f)
    {
        movFactor = f;
    }

    Vector3 movVector = Vector3.zero;
    void CollectInput()
    {
        Vector2 container = m_input.GetWASDVector();

        gp.query.VerticalMovement = (int)container.y;
        gp.query.HorizontalMovement = (int)container.x;

        container = container.normalized;
        movVector = new Vector3(container.x, 0f, container.y);

        m_crouch = Input.GetKey(KeyCode.LeftControl);
        gp.query.crouching = m_crouch;

        if(m_crouch)
        {
            localAnimator.SetFloat("crouch", 1);
        }
        else
        {
            localAnimator.SetFloat("crouch", 0);
        }
    }

    Vector3 m_camEuler;
    void Rotate()
    {
        //m_camEuler = PlayerSettings.PlayerCamera.GetCamEuler();
        //transform.localEulerAngles = new Vector3(0f, m_camEuler.y, 0f);
    }

    bool grounded = false;
    RaycastHit currentGround;
    Ray[] groundRays = new Ray[9];
    List<RaycastHit> hits = new List<RaycastHit>();
    RaycastHit ceneterCast;
    void GroundProbe()
    {
        grounded = false;
        hits.Clear();
        Vector3 transformPos = PlayerSettings.MainCollider.transform.position + PlayerSettings.MainCollider.center;
        
        groundRays[0] = new Ray(transformPos, Vector3.down);
        groundRays[1] = new Ray(transformPos + new Vector3(0f, 0f, PlayerSettings.MainCollider.radius), Vector3.down);
        groundRays[2] = new Ray(transformPos - new Vector3(0f, 0f, PlayerSettings.MainCollider.radius), Vector3.down);
        groundRays[3] = new Ray(transformPos + new Vector3(PlayerSettings.MainCollider.radius, 0f, 0f), Vector3.down);
        groundRays[4] = new Ray(transformPos - new Vector3(PlayerSettings.MainCollider.radius, 0f, 0f), Vector3.down);
        groundRays[5] = new Ray(transformPos - new Vector3(1, 0f, 1).normalized * PlayerSettings.MainCollider.radius, Vector3.down);
        groundRays[6] = new Ray(transformPos + new Vector3(1, 0f, 1).normalized * PlayerSettings.MainCollider.radius, Vector3.down);
        groundRays[7] = new Ray(transformPos + new Vector3(-1, 0f, 1).normalized * PlayerSettings.MainCollider.radius, Vector3.down);
        groundRays[8] = new Ray(transformPos + new Vector3(1, 0f, -1).normalized * PlayerSettings.MainCollider.radius, Vector3.down);
        
        RaycastHit groundHit;
        ceneterCast = new RaycastHit();
        float height = jump ? PlayerSettings.groundRayLength / 2 : PlayerSettings.groundRayLength;

        for (int i = 0; i < groundRays.Length; i++)
        {
            bool hit = Physics.Raycast(groundRays[i], out groundHit, height, PlayerSettings.raycastIncludeMask);
            if (hit)
            {
                float angle = GetSurfaceAngle(groundHit);
                if (angle > PlayerSettings.maxSlopeAngle)
                {
                    continue;
                }

                grounded = true;
                hits.Add(groundHit);
                if (i == 0)
                {
                    ceneterCast = groundHit;
                }

                if (PlayerSettings.debugDraw)
                {
                    Debug.DrawRay(groundRays[i].origin, groundRays[i].direction * height, Color.magenta);
                }
            }
        }

    }

    bool anchored = false;
    Vector3 forwardVec;
    Vector3 rightVec;
    Vector3 upVec;
    Vector3 momentum = Vector3.zero;
    void CollectVectors()
    {
        if(grounded)
        {
            if(ceneterCast.transform != null)
            {
                forwardVec = Vector3.Cross(ceneterCast.normal, -transform.right);
                rightVec = Vector3.Cross(ceneterCast.normal, transform.forward);
            }
            else
            {
                forwardVec = Vector3.Cross(hits[0].normal, -transform.right);
                rightVec = Vector3.Cross(hits[0].normal, transform.forward);
            }
        }
        else
        {
            forwardVec = transform.forward;
            rightVec = transform.right;
            upVec = transform.up;
        }

        m_sprint = !m_crouch && Input.GetKey(KeyCode.LeftShift) && gp.query.currentStance == ActionQuery.Stance.CHILL && movVector.x == 0 && movVector.z > 0 ? true : false;
        gp.query.sprinting = m_sprint;

        float movementAdditive = 1f;
        if(m_sprint)
        {
            movementAdditive = PlayerSettings.sprintMultiplier;
        }
        else if(m_crouch)
        {
            movementAdditive = PlayerSettings.crouchMultiplier;
        }

        Vector3 forward = forwardVec * movVector.z * Time.deltaTime * PlayerSettings.movementSpeed * movementAdditive * movFactor;
        Vector3 rightward = rightVec * movVector.x * Time.deltaTime * PlayerSettings.movementSpeed * movFactor;
        movFactor = 1f;

        Vector3 currentMomentum = forward + rightward;
        momentum = Vector3.Lerp(momentum, currentMomentum, PlayerSettings.momentumSwitch * Time.deltaTime);

        movementSum += momentum;

        if (!m_sprint && (forward.magnitude > Mathf.Epsilon || rightward.magnitude > Mathf.Epsilon))
            gp.query.jogging = true;
        else
            gp.query.jogging = false;
       
        if(PlayerSettings.debugDraw)
        {
            Debug.DrawRay(transform.position, forwardVec.normalized, Color.blue);
            Debug.DrawRay(transform.position, rightVec.normalized, Color.red);
        }
    }

    bool jump = false;
    public float jumpPeak = 4f;
    float m_peak;
    float jumpVelocity;
    public float jumpTime = 0.5f;
    void MovementManage()
    {
        if (grounded)
        {
            if(m_input.GetKeyTap(KeyCode.Space) && jump == false)
            {
                grounded = false;
                jumpVelocity = (jumpPeak / jumpTime);
                m_peak = jumpPeak;
                jump = true;
            }
            if (grounded)
                gp.query.falling = false;

            m_elapsedFallTimer = -1f;
        }
        else
        {
            Fall();
        }
       
        if(jump)
        {
            Jump();
        }

        gp.query.jumping = jump;
    }

    public float timerStart = .1f;
    float timer;
    void Jump()
    {
        if(m_peak <= 0f)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                jump = false;
                m_elapsedFallTimer = 1.25f;
                timer = timerStart;
            }
        }
        else
        {
            grounded = false;
            movementSum += Vector3.up * jumpVelocity * Time.deltaTime * (m_peak / 2);
            m_peak -= jumpVelocity * Time.deltaTime;
        }
    }

    float m_elapsedFallTimer = -1f;
    public float gravityStrength = 5f;
    void Fall()
    {
        if (jump)
            return;

        if(m_elapsedFallTimer < 0)
        {
            m_elapsedFallTimer = 1f;
        }

        if(m_elapsedFallTimer > 0)
        {
            movementSum += Vector3.down * gravityStrength * Time.deltaTime * (m_elapsedFallTimer * m_elapsedFallTimer);
            m_elapsedFallTimer += Time.deltaTime;
            gp.query.falling = true;
        }
    }


    void CorrectPosition()
    {  
        if(grounded)
        {
            float highestPoint = 500000;
            if(ceneterCast.transform != null)
            {
                highestPoint = ceneterCast.point.y - transform.position.y;
            }
            else
            {
                for (int i = 0; i < hits.Count; i++)
                {
                    float diff = hits[i].point.y - transform.position.y;
                    if (diff < highestPoint)
                    {
                        highestPoint = diff;
                    }
                }
            }  
            movementSum += new Vector3(0f, highestPoint, 0f) * Time.deltaTime * 30f;
        }
    }

    Vector3 pastPos;
    void FinalMove()
    {
        Vector3 travelForce = movementSum / Time.deltaTime;
        rb.velocity = travelForce;
        //movementFactor = Vector3.Distance(transform.root.position, pastPos) > 0.001f ? movementFactor = Mathf.Lerp(movementFactor, 1f, 10 * Time.deltaTime) : movementFactor = Mathf.Lerp(movementFactor, 0f, 10 * Time.deltaTime);
        movementFactor = 1f;

        movementSum = Vector3.zero;
        pastPos = transform.root.position;
    }

    public float GetMovFactor()
    {
        return movementFactor;
    }
    
    static float GetSurfaceAngle(RaycastHit hit)
    {
        return Vector3.Angle(Vector3.up, hit.normal);
    }
}
