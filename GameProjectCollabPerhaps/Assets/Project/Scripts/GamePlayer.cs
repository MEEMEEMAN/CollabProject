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

}

public class GamePlayer : MonoBehaviour
{
    public ActionQuery query;
    public PlayermodelController pmc;
    public PlayerController pc;
    public QuatCamController cam;

    void Start()
    {
        /*
        if (pmc == null)
        {
            pmc = transform.GetComponentInChildren<PlayermodelController>();
        }

        if (pc == null)
        {
            pc = transform.GetComponentInChildren<PlayerController>();
        }

        if (cam == null)
        {
            cam = pmc.playerCam;
        }
        */
    }
    /*
    public void EquipItem(Equippable item)
    {
        Equippable.SetSocket(item, pmc.rightSocket);
        item.OnEquipped();
    }
    */
    private void Update()
    {

    }
}
