using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all clothing items in the game.
/// </summary>
public class Clothing : Equippable
{

    public enum ClothingType
    {
        HAT, SHIRT, PANTS, SHOES
    }

    public ClothingType clothingType;

    public override void OnEquip()
    {
        WakeUp();
    }

    public override void OnAddedToInventory()
    {
        ManageBones();
    }

    void ManageBones()
    {

        transform.localPosition = Vector3.zero;
        SkinnedMeshRenderer target = Owner.GetAnimationController().skin;
        SkinnedMeshRenderer self = GetComponent<SkinnedMeshRenderer>();
        
    }
}
