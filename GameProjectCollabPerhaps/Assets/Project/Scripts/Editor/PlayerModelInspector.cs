using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PlayermodelController))]
public class PlayerModelInspector : Editor
{
    PlayermodelController comp;

    private void OnEnable()
    {
        comp = (PlayermodelController)target;
    }

    public override void OnInspectorGUI()
    {
        Draw();
        base.DrawDefaultInspector();
    }

void Draw()
    {
        if(GUILayout.Button("Setup"))
        {   

            Transform headEnd = GetByString(comp.Playermodel.transform, "HEAD_end");

            try
            {
                Transform camTransform = GetByType(comp.Playermodel.transform, typeof(QuatCamController));
                if (camTransform.parent == headEnd)
                {
                
                }
                else
                {
                    camTransform.parent = headEnd;
                    camTransform.localPosition = Vector3.zero;
                    camTransform.rotation = headEnd.rotation;
                }
            }
            catch
            {
                GameObject prefab = (GameObject)Resources.Load("AnimRelated/Player/MainPlayerCam");
                Instantiate(prefab, headEnd.transform.position, headEnd.transform.rotation, headEnd.transform);
                //PrefabUtility.InstantiatePrefab(prefab, headEnd.transform);
                Debug.Log(string.Format("Instantiated {0} on {1}", prefab.name, headEnd.transform.name));
            }
            
            comp.playerCam = headEnd.GetComponentInChildren<QuatCamController>();

            try
            {
                comp.GetComponent<PlayerController>().PlayerSettings.PlayerCamera = comp.playerCam;
            }
            catch
            {
                Debug.LogError("Error setting playercontroller's camera component.");
            }

            if(comp.animController != null)
            {
                comp.Playermodel.GetComponent<Animator>().runtimeAnimatorController = comp.animController;
            }

        }
    }

    void FindCam()
    {
        comp.playerCam = GetByType(comp.transform, typeof(QuatCamController)).GetComponent<QuatCamController>();
    }

    static Transform ret;

    public static Transform GetByType(Transform t, System.Type T)
    {
        ret = null;
        return GetType(t, T);
    }

    public static Transform GetByString(Transform t, string s)
    {
        ret = null;
        return GetString(t, s);
    }

    private static Transform GetType(Transform t, System.Type T)
    {
        if(t.childCount > 0)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                GetType(t.GetChild(i), T);
            }
        }

        if(t.GetComponent(T))
        {
            ret = t;
        }

        return ret;
    }

    private static Transform GetString(Transform t, string s)
    {
        if(t.childCount > 0)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                GetString(t.GetChild(i), s);
            }
        }

        if(t.name == s)
        {
            ret = t;
        }
        return ret;
    }

    List<Transform> found = new List<Transform>();
    public Transform[] GetGroupsByString(Transform t, string s)
    {
        GetGroupsString(t, s);

        Transform[] ret = found.ToArray();
        found.Clear();

        return ret;
    }

    private void GetGroupsString(Transform t, string s)
    {
        if (t.childCount > 0)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                GetGroupsString(t.GetChild(i), s);
            }
        }

        if (t.name.Contains(s))
        {
            found.Add(t);
        }
    }
}
    
