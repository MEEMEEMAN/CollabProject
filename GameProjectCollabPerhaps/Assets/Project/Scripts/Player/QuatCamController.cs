using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatCamController : MonoBehaviour
{
    [Header("Setup")]
    /// <summary>
    /// Should be higher than pitchtransforms in hierarchy
    /// </summary>
    public Transform yawTransform;

    /// <summary>
    /// Should be lower than yawtransforms in hierarchy
    /// </summary>
    public Transform pitchTransform;

    public Transform headTransform;

    public bool applyPitchRotation = true;
    public bool applyYawRotation = true;
    [HideInInspector] public Camera camComponent;
    [Header("Settings")]
    public float Sensitivity = 1.2f;
    GamePlayer gp;
    
    float m_Pitch = 0f;
    float m_Yaw = 0f;

    private void Start()
    {
        camComponent = transform.GetComponent<Camera>();
        gp = transform.root.GetComponent<GamePlayer>();
        //float[] distances = new float[32];
        //Camera c = GetComponent<Camera>();
        //distances[LayerMask.NameToLayer("Foliage")] = 15;
        //c.layerCullDistances = distances;
    }

    Vector3 pos = Vector3.zero; 
    public void Update()
    {
        //CalculateRotation();
        pos = transform.position;
    }

    public float lerpTest = 10f;
    private void LateUpdate()
    {
        ManageRecoil();
        CalculateRotation();
        //Vector3 lerp = Vector3.Lerp(transform.position, headTransform.position, lerpTest * Time.deltaTime);
        //transform.position = lerp;
        float dist = Vector3.Distance(pos, headTransform.position);
        float blend = Mathf.Lerp(pos.y ,headTransform.transform.position.y, dist + lerpTest * Time.deltaTime);
        transform.position = new Vector3(headTransform.position.x, blend, headTransform.position.z);
        transform.rotation = headTransform.rotation;
    }

    void ManageRecoil()
    {
        float subtraction = Mathf.Lerp(recoilBalance, 0,10f * Time.deltaTime);
        recoilBalance -= subtraction;
  
        if(recoilBalance > 0.01f)
        m_Pitch -= subtraction;
    }

    void CalculateRotation()
    {
        Vector2 mouseVector = CustomInputManager.GetMouseVector() * Sensitivity;
        m_Pitch += -mouseVector.y;
        m_Yaw += mouseVector.x;

        if (m_Yaw > 360)
        {
            m_Yaw = m_Yaw - 360;
        }
        else if (m_Yaw < 0)
        {
            m_Yaw = 360 + m_Yaw;
        }

        m_Pitch = Mathf.Clamp(m_Pitch, -90f, 90f);
        Quaternion yawRot = Quaternion.AngleAxis(m_Yaw, Vector3.up);
        Quaternion pitchRot = Quaternion.AngleAxis(m_Pitch, Vector3.right);

        if(yawTransform != null)
        {
            yawTransform.localRotation = yawRot;
        }
        
        if(pitchTransform != null)
        {
            pitchTransform.localRotation = pitchRot;
        }

    }

    float recoilBalance = 0f;
    public void Recoil(float strength)
    {
        recoilBalance = strength;
    }

    public Vector3 GetCamEuler()
    {
        return new Vector3(m_Pitch, m_Yaw, 0f);
    }

    public float getPitch()
    {
        float val = GetCamEuler().x / 360;
        return val;
    }
}
