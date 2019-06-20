using UnityEngine;

[System.Serializable]
public class HandSocket
{
    Transform socketTransform;
    Equippable currentOccupyingItem = null;
    bool occupied = false;

    public HandSocket(Transform socket)
    {
        socketTransform = socket;
    }

    public Transform GetSocketTransform()
    {
        return socketTransform;
    }

    public bool DequipItem()
    {
        if(currentOccupyingItem == null && !occupied)
        {
            return false;
        }
        else
        {
            occupied = false;
            currentOccupyingItem.DestroyItem(); //no inventory system yet, we destroy the item sadly.
            currentOccupyingItem = null;
            return true;
        }
    }

    public Equippable getCurretOcuppying()
    {
        return currentOccupyingItem;
    }

    public bool isOccupied()
    {
        return occupied;
    }
}


public class AnimationController : MonoBehaviour
{
    [Header("Setup")]
    public GameObject Playermodel;
    public RuntimeAnimatorController animController;
    public QuatCamController playerCam;
    public Transform rightSocketTransform;
    [SerializeField] Transform clothParent;
    [HideInInspector]public HandSocket rightHand;
    public Equippable.WeaponLayer currentEquipLayer = Equippable.WeaponLayer.FISTS;
    public SkinnedMeshRenderer skin;

    Equippable currentlyEquipped;
    bool isEquipped;
    GamePlayer gp;
    PlayerController pc;
    Animator animator;
    float blendLerp = 10f;
    float animLerpFactor = 5f;
    Vector2 camEuler;

    void Awake()
    {
        rightHand = new HandSocket(rightSocketTransform);
        currentEquipLayer = Equippable.WeaponLayer.FISTS;
    }

    void HandEquipUpdateEvent(Equippable item)
    {
        if(item == null)
        {
            currentEquipLayer = Equippable.WeaponLayer.FISTS;
            animator.runtimeAnimatorController = animController;
            return;
        }

        if(item.GetOnwer() == gp)
        {
            currentEquipLayer = item.animationType;
            currentlyEquipped = item;
            if(currentlyEquipped.AnimationsToOverride != null)
            animator.runtimeAnimatorController = currentlyEquipped.AnimationsToOverride;
        }
    }

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
        InventoryUI.EquipEvent += HandEquipUpdateEvent;
    }

    public Transform GetClothParent()
    {
        return clothParent;
    }

    void OnValidate()
    {
        
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
        StanceManage();
        AnimationSwitch();
    }

    ActionQuery.Stance currentStance;
    float blend = 0f;
    void StanceManage()
    {
        camEuler = playerCam.GetCamEuler();
        transform.localEulerAngles = new Vector3(0f, camEuler.y, 0f);
        float lerpFactor = animLerpFactor * Time.deltaTime;
        float pitch = playerCam.getPitch();

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

        if(currentStance != gp.query.currentStance) //runs once per stance swap
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

        float posBlend = 1f;
        float negBlend = 0f;

        if (currentlyEquipped != null)
            currentlyEquipped.AnimatorUpdate(animator);

        switch (currentStance) //runs every frame of stance
        {
            case ActionQuery.Stance.CHILL:
                negBlend = Mathf.Lerp(animator.GetLayerWeight(2), 0, lerpFactor);
                animator.SetLayerWeight(2, negBlend);
                ChillStance();
                break;
            case ActionQuery.Stance.OFFENSIVE:
                posBlend = Mathf.Lerp(animator.GetLayerWeight(2), 1, lerpFactor);
                animator.SetLayerWeight(2, posBlend);
                OffensiveStance();
                break;
            case ActionQuery.Stance.ANIMATED:
                break;
            default:
                break;
        }

        for (int i = 3; i < animator.layerCount; i++)
        {
            if(i == (int)currentEquipLayer && currentStance == ActionQuery.Stance.OFFENSIVE)
            {
                float blend = Mathf.Lerp(animator.GetLayerWeight(i), 1, lerpFactor);
                animator.SetLayerWeight(i, blend);
            }
            else if(i == (int)currentEquipLayer)
            {
                animator.SetLayerWeight(i, negBlend);
            }
            else
            {
                float blend = Mathf.Lerp(animator.GetLayerWeight(i), 0, lerpFactor);
                animator.SetLayerWeight(i, Mathf.Lerp(animator.GetLayerWeight(i), 0, blend));
            }
        }

        animator.SetFloat("pitch", pitch);
    }

    void ChillStance()
    {
        if (currentEquipLayer == Equippable.WeaponLayer.FISTS)
        {
            //PunchControls();
        }
        else
        {
            currentlyEquipped.ChillStanceUpdate(animator);
        }
    }

    void OffensiveStance()
    {
        if(currentEquipLayer == Equippable.WeaponLayer.FISTS)
        {
            PunchControls();
        }
        else
        {
            currentlyEquipped.OffensiveStanceUpdate(animator);
        }
    }

    int fist = 0;
    void PunchControls()
    {
        if (currentStance != ActionQuery.Stance.OFFENSIVE)
            return;

        if(CustomInputManager.GetMouseTap(0))
        { 
            if (fist == 0)
            {
                fist++;
                animator.SetTrigger("PUNCH_R");
            }
            else if(fist == 1)
            {
                fist--;
                animator.SetTrigger("PUNCH_L");
            }
        }
    }

    public Transform GetPlayermodel()
    {
        return Playermodel.transform;
    }
}
