using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Node CurrentNode { get; set; }
    private int supporters = 0;
    [SerializeField]
    private float movementSpeed = 5.0f;
    public bool IsMoving { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの初期位置を設定
        transform.position = CurrentNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーを次のノードに移動
        // Vector2 targetPosition = CurrentNode.transform.position;
        // if (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        // }
    }

    void FixedUpdate()
    {
        // プレイヤーを次のノードに移動
        Vector2 targetPosition = CurrentNode.transform.position;
        if (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    public bool IsMovable(Node node)
    {
        return CurrentNode.IsNeighbor(node);
    }
}
