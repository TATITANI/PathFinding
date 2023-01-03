using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Node
{
    public enum State { Normal, Block, Start, End, Trail, Result };
    public State state = State.Normal;
    [HideInInspector] public Node prev;
    [HideInInspector] public Vector2Int pos;
    [HideInInspector] public int F=0, G=0, H=0;
}
;
[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    public Node node;
    public Block next = null, prev = null;

    void Update()
    {
        transform.localPosition = new Vector3(
            Mathf.Floor(transform.localPosition.x),
            Mathf.Floor(transform.localPosition.y),
            Mathf.Floor(transform.localPosition.z));
        
        node.pos = new Vector2Int((int)transform.localPosition.x
            , (int)transform.localPosition.z);
    }

    public void SetState(Node.State state)
    {
        this.node.state = state;
        UpdateState();
    }

    public void UpdateState()
    {
        var mat = ResourceManager.instance.mats[node.state];
        GetComponent<MeshRenderer>().material = mat;
    }
}