// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    private enum NodeType
    {
        IncreaseSupporters,
        DecreaseSupporters,
        Unique,
    }

    public string nodeName;
    [SerializeField]
    private Node[] neighbors;
    [SerializeField]
    private NodeType nodeType;
    [SerializeField]
    public bool isUniquePotential;
    [SerializeField]
    private MessageWindow messageWindow;

    public AudioClip increaseSupportersSound;
    public AudioClip decreaseSupportersSound;
    public AudioClip uniqueSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            PlacePath(transform.position, neighbors[i].transform.position);
        }   
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

    public void SetRandomNodeType()
    {
        // if (!isUniquePotential)
        // {
        //     nodeType = (NodeType)Random.Range(0, 2);
        // }
        nodeType = (NodeType)Random.Range(0, 2);
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (nodeType)
        {
            case NodeType.IncreaseSupporters:
                spriteRenderer.color = Color.blue;
                break;
            case NodeType.DecreaseSupporters:
                spriteRenderer.color = Color.red;
                break;
        }
    }

    public void SetUniqueNodeType()
    {
        nodeType = NodeType.Unique;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;
    }

    public void ApplyEffect(Player player)
    {
        int value = 0;
        string message = "";
        UniqueNodeType uniqueNodeType = UniqueNodeType.PlayAgain;

        switch (nodeType)
        {
            case NodeType.IncreaseSupporters:
                (value, message) = NodeEffectManager.GetRandomIncreasingEffect();
                break;
            case NodeType.DecreaseSupporters:
                (value, message) = NodeEffectManager.GetRandomDecreasingEffect();
                break;
            case NodeType.Unique:
                (message, uniqueNodeType) = NodeEffectManager.GetRandomUniqueEffect();
                break;
        }

        string resultMessage;
        if (nodeType != NodeType.Unique)
        {
            resultMessage = $"{message}\n支持者が{System.Math.Abs(value)}人";
            if (value > 0) resultMessage += "増加した！";
            else resultMessage += "減少した…";
            player.Supporters += value;
        }
        else
        {
            resultMessage = message;
            switch (uniqueNodeType)
            {
                case UniqueNodeType.PlayAgain:
                    GameManager.Instance.PlayAgain();
                    break;
                case UniqueNodeType.SleepTop:
                    GameManager.Instance.SleepTopPlayer();
                    break;
                case UniqueNodeType.MeanSupporters:
                    GameManager.Instance.MeanSupporters();
                    break;
                case UniqueNodeType.StealSupporters:
                    GameManager.Instance.StealSupporters();
                    break;
            }
        }

        Debug.Log(resultMessage);
        Debug.Log(messageWindow);
        messageWindow.ShowMessage(resultMessage);

        audioSource = GetComponent<AudioSource>();
        switch (nodeType)
        {
            case NodeType.IncreaseSupporters:
                audioSource.clip = increaseSupportersSound;
                break;
            case NodeType.DecreaseSupporters:
                audioSource.clip = decreaseSupportersSound;
                break;
            case NodeType.Unique:
                audioSource.clip = uniqueSound;
                break;
        }
        audioSource.Play();
    }

    public void PlacePath(Vector2 startPos, Vector2 endPos)
    {
        // 道用のGameObjectを作成
        GameObject path = new GameObject("Path");

        // SpriteRendererを追加して単色のスプライトを設定
        SpriteRenderer renderer = path.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateSquareSprite(); // 単色の正方形スプライトを生成
        renderer.color = Color.gray; // 道の色を灰色に設定

        // 道の位置を設定（2つのマスの中間地点）
        Vector2 midPoint = (startPos + endPos) / 2;
        path.transform.position = midPoint;

        // 道の回転を設定（方向を向くように）
        Vector2 direction = endPos - startPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        path.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 道の長さを調整（距離に合わせてスケール変更）
        float distance = Vector2.Distance(startPos, endPos);
        path.transform.localScale = new Vector3(distance * 90, 10f, 1);

        // レイヤーを-5に設定（Playerよりも手前に表示）
        renderer.sortingOrder = -5;
    }

    // 単色の正方形スプライトを生成するヘルパーメソッド
    private Sprite CreateSquareSprite()
    {
        // 1x1ピクセルのテクスチャを生成
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white); // 白色でピクセルを設定
        texture.Apply();

        // テクスチャを使ってスプライトを作成
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }
}
