using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node[] neighbors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsNeighbor(Node node)
    {
        foreach (Node neighbor in neighbors)
        {
            if (neighbor == node)
            {
                return true;
            }
        }
        return false;
    }

    void OnMouseDown()
    {
        GameManager.Instance.TryMovePlayer(this);
    }
}