using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerId { get; set; }
    public Node CurrentNode { get; set; }
    public int Supporters { get; set; }
    [SerializeField]
    private float movementSpeed = 5.0f;
    [SerializeField]
    private Sprite[] playerSprites;
    public bool IsMoving { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの初期位置を設定
        transform.position = CurrentNode.transform.position;

        // プレイヤーのスプライトを設定
        GetComponent<SpriteRenderer>().sprite = playerSprites[PlayerId % playerSprites.Length];
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
