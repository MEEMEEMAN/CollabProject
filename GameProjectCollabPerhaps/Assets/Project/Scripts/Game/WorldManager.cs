using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager world;

    void Awake()
    {
        if(world == null)
        {
            world = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform World { get { return transform; } }
}
