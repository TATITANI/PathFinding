using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(Block))]
[CanEditMultipleObjects]
public class BlockEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // script.UpdateState();
        foreach (var t in targets)
        {
            ((Block)t).UpdateState();
            if (t.GameObject().transform.localPosition.x < 0 ||
                t.GameObject().transform.localPosition.z < 0)
            {
                Debug.LogError($"{t.name} 좌표 양수여야 함");
            }
        }
    }
}
