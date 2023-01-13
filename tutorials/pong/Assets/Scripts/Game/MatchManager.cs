using System.Collections;
using System.Collections.Generic;

using FishNet.Object;
using FishNet.Object.Synchronizing;

using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MatchManager : NetworkBehaviour
{

	public static MatchManager Instance { get; private set; }

    [SerializeField]
    public int scoreToWin = 3;
    public PaddleMovement player1Paddle;
    public PaddleMovement player2Paddle;
    public BallMovement ball;

    private int _player1Score = 0;
    private int _player2Score = 0;

    private bool _isGameOver = false;

	[SyncObject]
	public readonly SyncList<Player> players = new();

	[SyncVar]
	public bool canStart;

    private void Start()
    {  
        StartMatch();
        StartRound();
    }


	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
        for (int i = 0; i < players.Count; i++) {
            Player player = players[i];
            Paddle paddle = player.controlledPaddle;
            // Debug.Log($"Player: {i} isReady: {player.isReady} isPlayer1: {player.isPlayer1} Paddle: {paddle.GetInstanceID()}");
        }
		if (!IsServer) return;
		canStart = players.All(player => player.isReady);
	}

    public bool addPlayer(Player player) {
        if (players.Count >= 2) {
            players.Remove(players[1]);
        }

        players.Add(player);
        player.isPlayer1 = players.Count == 1;
        player.isReady = false;
        return true;
    }

    void StartMatch() {
        _isGameOver = false;
        _player1Score = 0;
        _player2Score = 0;
    }

    void StartRound() {
        ResetRound();
        //ball.GetComponent<BallMovement>().Release();
    }

    void ResetRound() {
        //ball.GetComponent<BallMovement>().Reset();
        PaddleMovement player1Movement = player1Paddle.GetComponent<PaddleMovement>();
        player1Movement.isPlayer1 = true;
        player1Movement.Reset();

        PaddleMovement player2Movement = player2Paddle.GetComponent<PaddleMovement>();
        player2Movement.isPlayer1 = false;
        player2Movement.Reset();
    }
    
    void Score(int player) {
        if (player == 1) {
            _player1Score++;
        } else if (player == 2) {
            _player2Score++;
        }

        if (_player1Score >= scoreToWin || _player2Score >= scoreToWin) {
            EndGame();
        } else {
            StartRound();
        }
    }

    void EndGame()
    {
        _isGameOver = true;
    }

}
