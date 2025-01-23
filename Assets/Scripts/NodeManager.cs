using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Node[] nodes;
    
    void Start()
    {
        nodes = GetComponentsInChildren<Node>();
        ResetNodes();
    }

    public void ResetNodes()
    {
        foreach (Node node in nodes)
        {
            node.SetRandomNodeType();
        }
    }
}