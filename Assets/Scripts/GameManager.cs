using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    private Vector2Int mapSize;
    private Block[,] arrBlock = new Block[10, 10];
    [SerializeField] private Transform blockHolder;
    private List<Node> openList = new List<Node>();
    private List<Node> closedList = new List<Node>();
    private Node startNode, endNode;

    private void Start()
    {
        var blocks = blockHolder.GetComponentsInChildren<Block>();
        foreach (var block in blocks)
        {
            arrBlock[block.node.pos.x, block.node.pos.y] = block;
            if (block.node.state == Node.State.Start)
                startNode = block.node;
            else if (block.node.state == Node.State.End)
                endNode = block.node;
        }

        mapSize.x = blocks.Max(b => b.node.pos.x);
        mapSize.y = blocks.Max(b => b.node.pos.y);

        Debug.Log($"start : {startNode.pos} , end : {endNode.pos}");
    }

    bool InRange(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.x <= mapSize.x && pos.y >= 0 && pos.y <= mapSize.y);
    }

    public void DoAStar()
    {
        StartCoroutine(AStar());
    }

    IEnumerator AStar()
    {
        openList.Add(startNode);
        Vector2Int[] dPos = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

        Node currentNode = startNode;
        while (currentNode != endNode && openList.Count != 0)
        {
          
            foreach (var d in dPos)
            {
                Vector2Int neighborPos = currentNode.pos + d;
                if (InRange(neighborPos))
                {
                    var node = arrBlock[neighborPos.x, neighborPos.y].node;
                    if (node.state != Node.State.Block && closedList.Contains(node) == false)
                    {
                        openList.Add(node);
                        node.prev = currentNode;
                    }
                }
            }

            Node nextNode = openList.OrderBy(node =>
            {
                int g = (currentNode.pos - node.pos).sqrMagnitude;
                int h = (endNode.pos - node.pos).sqrMagnitude;
                int f = g + h;
                return f;
            }).First();
            
            currentNode = nextNode;

            arrBlock[currentNode.pos.x, currentNode.pos.y].SetState(Node.State.Trail);
            
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            
            yield return new WaitForSeconds(0.1f);
        }

        if (currentNode == endNode)
        {
            arrBlock[currentNode.pos.x, currentNode.pos.y].SetState(Node.State.Result);
            while (currentNode != startNode || currentNode == null)
            {
                currentNode = currentNode.prev;
                arrBlock[currentNode.pos.x, currentNode.pos.y].SetState(Node.State.Result);
            }
        }

        yield return null;
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            MouseDown();
        }
    }

    void MouseDown()
    {
        Vector3 mousePos = Event.current.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(mousePos.x, Screen.height - mousePos.y, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Block"))
            {
                Block block = hit.collider.GetComponent<Block>();
                block.SetState(block.node.state == Node.State.Normal ? Node.State.Block : Node.State.Normal);
            }
        }
    }
}