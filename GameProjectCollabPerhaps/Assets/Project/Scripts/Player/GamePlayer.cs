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
    [SerializeField]InventoryManager inventory;
    [SerializeField] Transform itemParent;

    AnimationController pmc;
    PlayerController pc;
    QuatCamController cam;

    void OnValidate()
    {
        
    }

    void Start()
    {
        GameManager.instance.SetLocalPlayer(this);
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

        inventory = new InventoryManager(this, pmc.rightHand, itemParent);
    }

    bool yes = false;
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Q))
       {
        if(yes)
            inventory.AddToInventory(ItemBase.Create("CZ"));
            else
                inventory.AddToInventory(ItemBase.Create("CHOCOLATE"));
            yes = !yes;
        }

       if(Input.GetKeyDown(KeyCode.E))
       {
            inventory.RemoveLastItem();
       }
    }
    
    public InventoryManager GetInventory()
    {
        return inventory;
    }

    public AnimationController GetAnimationController()
    {
        return pmc;
    }

    public QuatCamController GetCamera()
    {
        return cam;
    }

    public PlayerController GetPlayerController()
    {
        return pc;
    }
}
