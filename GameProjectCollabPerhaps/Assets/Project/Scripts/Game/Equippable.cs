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

/// <summary>
/// All items that can be worn or used by the hands of the player derive from this class.
/// </summary>
public class Equippable : ItemBase
{
    public WeaponLayer animationType;
    public OffsetData offsetData;

    public enum WeaponLayer
    {
        FISTS = 3, PISTOL, RIFLE
    }
    
   /// <summary>
   /// Applies OffsetData on the equippable item.
   /// </summary>
   public void ApplyOffsets()
   {
        transform.localPosition = offsetData.localPosition;
        transform.localRotation = offsetData.localRotation;
        transform.localScale = offsetData.localScale;
   }

   public Animator GetAnimator()
   {
        return transform.GetComponentInChildren<Animator>();
   }
}
