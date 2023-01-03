using System;
using System.Collections;
using System.Collections.Generic;
using CustomDic;
using UnityEngine;

[ExecuteInEditMode]
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    // public Material matBasic, matBlock, matStart, matEnd, matTrail;
    public SerializableDictionary<Node.State, Material> mats;
    private void OnEnable()
    {
        instance = this;
    }
        
    
}
