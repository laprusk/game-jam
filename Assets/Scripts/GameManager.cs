using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState
{
    StartTurn,
    RollDice,
    MovePlayer,
    VisitNode,
    EndTurn
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private NodeManager nodeManager;
    [SerializeField]
    private Node startNode;
    [SerializeField]
    private int playerCount = 3;
    [SerializeField]
    private int maxTurn = 10;
    [SerializeField]
    private Dice dice;
    private GameState currentState;
    private Player[] players;
    private int currentPlayerIndex = 0;
    private int turn = 0;
    private int remainingMoves = 0;
    [SerializeField]
    private Node nextNode;
    [SerializeField]
    private GUIStyle textStyle;
    [SerializeField]
    private MessageWindow messageWindow;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private TextMeshProUGUI resultText;
    private bool isPlayAgain = false;
    private Node[] visitedNodes = new Node[7];

    // Start is called before the first frame update
    void Start()
    {
        players = new Player[playerCount];

        // プレイヤーをインスタンス化
        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerObject = Instantiate(playerPrefab);
            Player player = playerObject.GetComponent<Player>();
            player.PlayerId = i;
            player.CurrentNode = startNode;
            playerObject.name = "プレイヤー" + (i + 1);

            players[i] = player;
        }

        Instance = this;
        resultPanel.SetActive(false);
        SetState(GameState.StartTurn);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.RollDice:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    dice.StopRolling();
                }
                if (dice.IsStopped)
                {
                    SetState(GameState.MovePlayer);
                }
                break;
        }
    }

    public void SetState(GameState state)
    {
        currentState = state;
        OnStateChange();
        Debug.Log("State: " + currentState);
    }

    void OnStateChange()
    {
        switch (currentState)
        {
            case GameState.StartTurn:
                StartTurn();
                break;
            case GameState.RollDice:
                RollDice();
                break;
            case GameState.MovePlayer:
                MovePlayer();
                break;
            case GameState.VisitNode:
                StartCoroutine(VisitNode());
                break;
            case GameState.EndTurn:
                EndTurn();
                break;
        }
    }

    void StartTurn()
    {
        Debug.Log("Start Turn");
        Debug.Log("Current Player: " + players[currentPlayerIndex].name);
        if (players[currentPlayerIndex].sleepTurn > 0)
        {
            messageWindow.ShowMessage($"{players[currentPlayerIndex].name}は{players[currentPlayerIndex].sleepTurn}ターン休み！");
            players[currentPlayerIndex].sleepTurn--;
            // SetState(GameState.EndTurn);
            return;
        }

        players[currentPlayerIndex].SetActive(true);
        SetState(GameState.RollDice);
    }

    void RollDice()
    {
        dice.StartRolling();
    }

    void MovePlayer()
    {
        remainingMoves = dice.Value;
        visitedNodes[remainingMoves] = players[currentPlayerIndex].CurrentNode;
    }

    void EndTurn()
    {
        if (isPlayAgain)
        {
            isPlayAgain = false;
            SetState(GameState.StartTurn);
            return;
        }     

        players[currentPlayerIndex].SetActive(false);
        currentPlayerIndex = (currentPlayerIndex + 1) % playerCount;     
        
        if (currentPlayerIndex == 0)
        {
            turn++;
            nodeManager.ResetNodes();
        }   

        if (turn >= maxTurn)
        {
            ShowResult();
            return;
        }

        SetState(GameState.StartTurn);
        Debug.Log("Next Player: " + players[currentPlayerIndex].name);
    }

    public void TryMovePlayer(Node node)
    {
        if (currentState != GameState.MovePlayer)
        {
            return;
        }
        Player currentPlayer = players[currentPlayerIndex];
        if (currentPlayer.IsMovable(node) && !currentPlayer.IsMoving)
        {
            players[currentPlayerIndex].CurrentNode = node;

            if (remainingMoves + 1 < 7)
            {
                if (visitedNodes[remainingMoves + 1] == node)
                {
                    ++remainingMoves;
                    return;
                }
            }
            remainingMoves--;
            visitedNodes[remainingMoves] = node;
            Debug.Log("Remaining Moves: " + remainingMoves);
            Debug.Log(visitedNodes);
            if (remainingMoves == 0)
            {
                SetState(GameState.VisitNode);
            }
        }
    }

    IEnumerator VisitNode()
    {
        while (players[currentPlayerIndex].IsMoving)
        {
            yield return null;
        }

        players[currentPlayerIndex].VisitNode();
        // SetState(GameState.EndTurn);
    }

    Player GetWinner()
    {
        int maxSupporters = -1;
        Player winner = null;

        foreach (Player player in players)
        {
            if (player.Supporters > maxSupporters)
            {
                maxSupporters = player.Supporters;
                winner = player;
            }
        }

        return winner;
    }

    private void ShowResult()
    {
        Player winner = GetWinner();
        Debug.Log("Winner: " + winner.name);
        resultText.text = winner.name;
        resultPanel.SetActive(true);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "ターン: " + (turn + 1), textStyle);
        GUI.Label(new Rect(10, 50, 100, 20), players[currentPlayerIndex].name, textStyle);
        if (currentState == GameState.MovePlayer)
        {
            GUI.Label(new Rect(10, 90, 100, 20), "残り" + remainingMoves + "マス", textStyle);
        }

        for (int i = 0; i < playerCount; i++)
        {
            GUI.Label(new Rect(10, 130 + 40 * i, 100, 20), players[i].name + ": " + players[i].Supporters, textStyle);
        }
    }

    public void PlayAgain()
    {
        players[currentPlayerIndex].Supporters += Convert.ToInt32(players[currentPlayerIndex].Supporters * 0.3);
        isPlayAgain = true;
    }

    public void SleepTopPlayer()
    {
        int maxSupporters = -1, maxIndex = -1;

        for (int i = 0; i < playerCount; i++)
        {
            if (players[i].Supporters > maxSupporters)
            {
                maxSupporters = players[i].Supporters;
                maxIndex = i;
            }
        }

        players[maxIndex].sleepTurn = 2;
    }

    public void MeanSupporters()
    {
        int sumSupporters = 0;

        for (int i = 0; i < playerCount; i++)
        {
            sumSupporters += players[i].Supporters;
        }

        int meanSupporters = sumSupporters / playerCount;

        for (int i = 0; i < playerCount; i++)
        {
            players[i].Supporters = meanSupporters;
        }
    }

    public void StealSupporters()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (i != currentPlayerIndex)
            {
                players[currentPlayerIndex].Supporters += Convert.ToInt32(players[i].Supporters * 0.2);
                players[i].Supporters -= Convert.ToInt32(players[i].Supporters * 0.2);
            }
        }
    }
}
