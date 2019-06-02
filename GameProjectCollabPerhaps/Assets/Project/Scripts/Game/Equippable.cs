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

    public static void ParentToSocket(Equippable item, Transform socket)
    {
        item.transform.SetParent(socket);
        item.transform.localPosition = item.offsetData.localPosition;
        item.transform.localRotation = item.offsetData.localRotation;
        item.transform.localScale = item.offsetData.localScale;
    }


    public void CreateItem(Equippable item, GamePlayer player)
    {
        GameObject instance = Instantiate(item.gameObject, player.pmc.rightSocketTransform);
        ItemBase itemInstance = instance.GetComponent<ItemBase>();
        if(itemInstance == item)
        {
            Debug.Log("yep");
        }

        instance.transform.localPosition = item.offsetData.localPosition;
        instance.transform.localRotation = item.offsetData.localRotation;
        instance.transform.localScale = item.offsetData.localScale;
    }

    /// <summary>
    /// by the time you enter the constructors, youve done more than half the work.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool CreateInHands(GamePlayer player, Equippable item)
    {
        bool success = player.EquipItem(item);

        if(!success)
        {
            Debug.Log("Failed to CreateInHands, Hand is occupied: " + player.pmc.rightHand.isOccupied());
            item.DestroyItem();
        }

        return success;
    }

}
