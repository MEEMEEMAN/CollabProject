using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemBaseInspector : Editor
{
    ItemBase comp;
    void OnEnable()
    {
        comp = (ItemBase)target;
    }

    public override void OnInspectorGUI()
    {
        Draw();
        DrawDefaultInspector();
    }

    void Draw()
    {
        
    }
}
