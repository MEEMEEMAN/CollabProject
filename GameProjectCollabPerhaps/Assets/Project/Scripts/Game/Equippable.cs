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
    [Header("Types")]
    public EquippableType EquipmentType;
    public WeaponLayer animationType;
    [Header("Data")]
    public OffsetData offsetData;
    public AnimatorOverrideController AnimationsToOverride;
    [Header("Components")]
    [SerializeField]protected Animator localAnimator;

    public enum WeaponLayer
    {
        NONE = -1, FISTS = 3, HANDHELD = 4
    }
    
    public enum EquippableType
    {
        HAND, HEAD, TORSO, LEGS, SHOES
    }

    void OnValidate()
    {
        try
        {
            if(GetAnimator() != null)
            localAnimator = GetAnimator();
        }
        catch
        {

        }
    }

    public Animator GetAnimator()
   {
        return transform.GetComponentInChildren<Animator>();
   }

   public virtual void OnEquip()
   {
        WakeUp();
   }

   public virtual void OnDequip()
   {
        Sleep();
   }

    public override void ApplySockets()
    {
        transform.parent = Owner.GetInventory().GetHandSocketTransform();
    }

    public override void OnAddedToInventory()
    {
        ApplySockets();
        ApplyOffsets();
    }

    public virtual void AnimatorUpdate(Animator CharacterModelAnimator)
    {
        
    }

    public virtual void OffensiveStanceUpdate(Animator CharacterModelAnimator)
    {

    }

    public virtual void ChillStanceUpdate(Animator CharacterModelAnimator)
    {

    }

    /// <summary>
    /// Applies OffsetData on the equippable item.
    /// </summary>
    public virtual void ApplyOffsets()
    {
        transform.localPosition = offsetData.localPosition;
        transform.localRotation = offsetData.localRotation;
        transform.localScale = offsetData.localScale;
    }

    public virtual IEnumerator AnimatorResetWait(float time = 0.05f, Animator characterModelAnimator = null)
    {
        yield return new WaitForSeconds(time);
    }
}
