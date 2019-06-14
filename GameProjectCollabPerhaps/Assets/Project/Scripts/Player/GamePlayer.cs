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
        GameManager.localPlayer = this;
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
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            inventory.AddToInventory(ItemBase.GetItem("CZ"));
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            inventory.RemoveLastItem();
        }
        /*
        if (Input.GetKeyDown(KeyCode.Q))
            inventory.MoveToHands(ItemBase.GetItem("CZ").Create() as Equippable);

        if (Input.GetKeyDown(KeyCode.E))
            inventory.Dequip();
            */
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

    public void SetEquipLayer(Equippable.WeaponLayer layer)
    {
        pmc.currentEquipLayer = layer;
    }
}
