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
        List<Node> uniqueNodes = new List<Node>();
        foreach (Node node in nodes)
        {
            node.SetRandomNodeType();
            if (node.isUniquePotential)
            {
                uniqueNodes.Add(node);
            }
        }

        if (uniqueNodes.Count > 0)
        {
            int randomIndex = Random.Range(0, uniqueNodes.Count);
            uniqueNodes[randomIndex].SetUniqueNodeType();
        }
    }
}