using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom input manager
/// </summary>
public class CustomInputManager
{
    public bool enabled = true;

    /// <summary>
    /// Get this frame's mouse movement vector
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMouseVector()
    {
        if (!enabled)
            return Vector2.zero;

        float x = Input.GetAxisRaw("Mouse X");
        float y = Input.GetAxisRaw("Mouse Y");

        return new Vector2(x, y);
    }

    /// <summary>
    /// uses GetMouseButtonDown()
    /// </summary>
    /// <param name="mouseButton"></param>
    /// <returns></returns>
    public bool GetMouseTap(int mouseButton)
    {
        if (!enabled)
            return false;

        if (Input.GetMouseButtonDown(mouseButton))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Uses GetMouseButton()
    /// </summary>
    /// <param name="mouseButton"></param>
    /// <returns></returns>
    public bool GetMouseHold(int mouseButton)
    {
        if (!enabled)
            return false;

        if (Input.GetMouseButton(mouseButton))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Uses GetKeyDown()
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyTap(KeyCode key)
    {
        if (!enabled)
            return false;

        if (Input.GetKeyDown(key))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Uses GetKey()
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyHold(KeyCode key)
    {
        if (!enabled)
            return false;

        if (Input.GetKey(key))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// returns the W,A,S,D input as a vector. NOT NORMALIZED
    /// </summary>
    /// <returns></returns>
    public Vector2 GetWASDVector()
    {
        if (!enabled)
            return Vector2.zero;

        Vector2 axis;
        axis.x = Input.GetAxisRaw("Horizontal");
        axis.y = Input.GetAxisRaw("Vertical");
        return axis;
    }
}