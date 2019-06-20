using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Equippable), true)]
public class EquippableInspector : Editor
{
    Equippable comp;
    bool cloth = false;

    void OnEnable()
    {
        comp = (Equippable)target;
        if(comp is Clothing)
        {
            cloth = true;
        }
        else
        {
            cloth = false;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawItem();
        DrawDefaultInspector();
    }

    void DrawItem()
    {
        if(!cloth)
        {
            if (GUILayout.Button("Write offset data"))
            {
                Undo.RecordObject(comp, "Undo Write Offset");
                Write();
            }
        }
    }

    void Write()
    {
        comp.offsetData.localPosition = comp.transform.localPosition;
        comp.offsetData.localRotation = comp.transform.localRotation;
        comp.offsetData.localScale = comp.transform.localScale;
    }
}
