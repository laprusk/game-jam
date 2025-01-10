using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    StartTurn,
    RollDice,
    MovePlayer,
    EndTurn
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject playerPrefab;
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

    // Start is called before the first frame update
    void Start()
    {
        players = new Player[playerCount];

        // プレイヤーをインスタンス化
        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerObject = Instantiate(playerPrefab);
            Player player = playerObject.GetComponent<Player>();
            player.CurrentNode = startNode;
            playerObject.name = "Player" + (i + 1);

            players[i] = player;
        }

        Instance = this;
        SetState(GameState.StartTurn);
    }

    // Update is called once per frame
    void Update()
    {
        // players[currentPlayerIndex].CurrentNode = nextNode;

        switch (currentState)
        {
            case GameState.RollDice:
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
            case GameState.EndTurn:
                EndTurn();
                break;
        }
    }

    void StartTurn()
    {
        SetState(GameState.RollDice);
    }

    void RollDice()
    {
        dice.StartRolling();
    }

    void MovePlayer()
    {
        remainingMoves = dice.Value;
    }

    void EndTurn()
    {
        SetState(GameState.StartTurn);
        currentPlayerIndex = (currentPlayerIndex + 1) % playerCount;
        
        if (currentPlayerIndex == 0)
        {
            turn++;
        }

        if (turn >= maxTurn)
        {
            Debug.Log("Game Over");
        }
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
            remainingMoves--;
            Debug.Log("Remaining Moves: " + remainingMoves);
            if (remainingMoves == 0)
            {
                SetState(GameState.EndTurn);
            }
        }
    }
}
