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
/// All equippable items eg Guns and clothes derive from this class.
/// </summary>
public class Equippable : ItemBase
{
    public RuntimeAnimatorController animController;
    public OffsetData offsetData;


    public static void SetSocket(Equippable item, Transform socket)
    {
        item.transform.SetParent(socket);
        item.transform.localPosition = item.offsetData.localPosition;
        item.transform.localRotation = item.offsetData.localRotation;
        item.transform.localScale = item.offsetData.localScale;
    }

    public void CreateItem(Equippable item, GamePlayer player)
    {
        GameObject instance = Instantiate(item.gameObject, player.pmc.rightHandSocket);
        ItemBase itemInstance = instance.GetComponent<ItemBase>();
        if(itemInstance == item)
        {
            Debug.Log("yep");
        }

        instance.transform.localPosition = item.offsetData.localPosition;
        instance.transform.localRotation = item.offsetData.localRotation;
        instance.transform.localScale = item.offsetData.localScale;
    }

}
