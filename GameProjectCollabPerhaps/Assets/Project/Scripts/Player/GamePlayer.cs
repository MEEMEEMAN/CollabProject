using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionQuery
{
    public enum Stance
    {
        CHILL = 0, OFFENSIVE, ANIMATED
    }

    public Stance currentStance = Stance.CHILL;
    public bool jogging = false;
    public bool sprinting = false;
    public bool jumping = false;
    public bool falling = false;
    public bool crouching = false;
    public int VerticalMovement = -1;
    public int HorizontalMovement = -1;
}

[System.Serializable]
public class PlayerInventory
{
    GamePlayer boundPlayer;
    HandSocket boundSocket;
    public Equippable currentlyEquipped;

   public PlayerInventory(GamePlayer player, HandSocket value)
   {
        boundPlayer = player;
        boundSocket = value;
   }

   public bool MoveToHands(Equippable equipment)
   {
        if(currentlyEquipped == null)
        {
            currentlyEquipped = equipment;
            EquippedToHand();
            return true;
        }
        else
        {
            return false;
        }
   }

   void EquippedToHand()
   {
        currentlyEquipped.transform.SetParent(boundSocket.getTransform());

        currentlyEquipped.transform.localPosition = currentlyEquipped.offsetData.localPosition;
        currentlyEquipped.transform.localRotation = currentlyEquipped.offsetData.localRotation;
        currentlyEquipped.transform.localScale = currentlyEquipped.offsetData.localScale;
        if(currentlyEquipped.colliders != null)
        {
            currentlyEquipped.colliders.gameObject.SetActive(false);
            currentlyEquipped.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

public class GamePlayer : MonoBehaviour
{
    public ActionQuery query;

    [Header("Inventory")]
    [SerializeField]PlayerInventory inventory;

    AnimationController pmc;
    PlayerController pc;
    QuatCamController cam;

    void Awake()
    {
        
    }

    void Start()
    {
        if (pmc == null)
        {
            pmc = transform.GetComponentInChildren<AnimationController>();
        }

        if (pc == null)
        {
            pc = transform.GetComponentInChildren<PlayerController>();
        }

        if (cam == null)
        {
            cam = pmc.playerCam;
        }
        inventory = new PlayerInventory(this, pmc.rightHand);

        ItemBase item = ItemBase.GetItem("CZ").Create();
        inventory.MoveToHands(item as Equippable);

        
    }
    
    public PlayerInventory getInventory()
    {
        return inventory;
    }

    public AnimationController getAnimationController()
    {
        return pmc;
    }

    public QuatCamController getCam()
    {
        return cam;
    }

    public PlayerController getPlayerController()
    {
        return pc;
    }

    public HandSocket getRightHandSocket()
    {
        return pmc.rightHand;
    }
}
