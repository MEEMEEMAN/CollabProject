using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayermodelController : MonoBehaviour
{
    [Header("Setup")]
    public GameObject Playermodel;
    public RuntimeAnimatorController animController;
    public QuatCamController playerCam;
    public Transform rightHandSocket;

    GamePlayer gp;
    PlayerController pc;
    Animator animator;
    float blendLerp = 10f;
    float animLerpFactor = 5f;
    Vector2 camEuler;
    CustomInputManager input = new CustomInputManager();

    private void Start()
    {
        if (animator == null)
        {
            animator = Playermodel.GetComponent<Animator>();
        }

        gp = transform.root.GetComponent<GamePlayer>();
        if (Playermodel == null)
            Playermodel = transform.GetChild(0).gameObject;
        pc = GetComponent<PlayerController>();
    }  

    Vector2 movement = Vector2.zero;
    void AnimationSwitch()
    {
        Vector2 current = new Vector2(gp.query.HorizontalMovement, gp.query.VerticalMovement);

        if (gp.query.sprinting)
        {
            movement = Vector2.Lerp(movement, new Vector2(0, 2), blendLerp * Time.deltaTime);
        }
        else
        {
            movement = Vector2.Lerp(movement, current, blendLerp * Time.deltaTime);
        }

        float factor = current.magnitude;
        if (factor > 1)
            factor = 1;

        pc.SetMovFactor(factor);
        animator.SetFloat("movement", movement.y * pc.GetMovFactor());
        animator.SetFloat("strafe", movement.x * pc.GetMovFactor());
    }

    private void LateUpdate()
    {
        camEuler = playerCam.GetCamEuler();
        StanceManage();
        AnimationSwitch();
    }

    ActionQuery.Stance currentStance;
    float blend = 0f;
    Vector3 chestOriginalPose = Vector3.zero;
    void StanceManage()
    {
        transform.localEulerAngles = new Vector3(0f, playerCam.GetCamEuler().y, 0f);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(gp.query.currentStance == ActionQuery.Stance.OFFENSIVE)
            {
                gp.query.currentStance = ActionQuery.Stance.CHILL;
            }
            else if(gp.query.currentStance == ActionQuery.Stance.CHILL)
            {
                gp.query.currentStance = ActionQuery.Stance.OFFENSIVE;
            }
        }

        if(currentStance != gp.query.currentStance)
        {
            currentStance = gp.query.currentStance;
            switch (gp.query.currentStance)
            {
                case ActionQuery.Stance.CHILL:
                    playerCam.camComponent.fieldOfView = 80;
                    break;
                case ActionQuery.Stance.OFFENSIVE:
                    break;
                case ActionQuery.Stance.ANIMATED:
                    break;
                default:
                    break;
            }
        }
        float pitch = playerCam.GetCamEuler().x / 360;

        switch (currentStance)
        {
            case ActionQuery.Stance.CHILL:
                blend = Mathf.Lerp(blend, 0f, animLerpFactor * Time.deltaTime);
                ChillStance();
                break;
            case ActionQuery.Stance.OFFENSIVE:
                blend = Mathf.Lerp(blend, 1f, animLerpFactor * Time.deltaTime);
                OffensiveStance();
                break;
            case ActionQuery.Stance.ANIMATED:
                break;
            default:
                break;
        }

        animator.SetLayerWeight(2, blend);
        //animator.SetLayerWeight(3, blend);
        animator.SetFloat("pitch", pitch);
    }
    
    void ChillStance()
    {
        //setup.head.SetLocalEuler(new Vector3(playerCam.GetCamEuler().x,0f,0f));

    }

    void OffensiveStance()
    {
        Punch();
    }

    void Punch()
    {
        if(Input.GetMouseButton(1))
        {
            animator.SetBool("ads", true);
            playerCam.camComponent.fieldOfView = 40;
        }
        else
        {
            playerCam.camComponent.fieldOfView = 80;
            animator.SetBool("ads", false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
    }
}
