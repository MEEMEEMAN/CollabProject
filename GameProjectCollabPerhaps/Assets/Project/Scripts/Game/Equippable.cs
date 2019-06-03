using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OffsetData
{
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
}

public enum EquipLayer
{
    
}

/// <summary>
/// All equippable items eg Guns and clothes derive from this class.
/// </summary>
public class Equippable : ItemBase
{
    [HideInInspector]public bool equipped = false;
    public RuntimeAnimatorController animController;
    public OffsetData offsetData;
    public AnimatorOverrideController playermodelOverrideAnims;

    /// <summary>
    /// Fetch equippable reference from the database. ONLY REFERENCE, NO INSTANTIATION
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public static Equippable GetEquip(string itemName)
    {
        Equippable equip;
        try
        {
            equip = GetItem(itemName) as Equippable;
        }
        catch
        {
            equip = null;
        }
        return equip;
    }

    

}
