using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Equippable))]
public class EquippableInspector : Editor
{
    Equippable comp;

    void OnEnable()
    {
        comp = (Equippable)target;
    }

    public override void OnInspectorGUI()
    {
        Draw();
        DrawDefaultInspector();
    }

    void Draw()
    {
        if(GUILayout.Button("Write offset data"))
        {
            Write();
        }
    }

    void Write()
    {
        comp.offsetData.localPosition = comp.transform.localPosition;
        comp.offsetData.localRotation = comp.transform.localRotation;
        comp.offsetData.localScale = comp.transform.localScale;
    }
}
