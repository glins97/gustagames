using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{

    [SerializeField]
    public int scoreToWin = 3;
    public PaddleMovement player1Paddle;
    public PaddleMovement player2Paddle;
    public BallMovement ball;

    private int _player1Score = 0;
    private int _player2Score = 0;

    private bool _isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        StartMatch();        
        StartRound();
    }

    void StartMatch() {
        _isGameOver = false;
        _player1Score = 0;
        _player2Score = 0;
    }

    void StartRound() {
        ResetRound();
        ball.GetComponent<BallMovement>().Release();
    }

    void ResetRound() {
        ball.GetComponent<BallMovement>().Reset();
        player1Paddle.GetComponent<PaddleMovement>().Reset();
        player2Paddle.GetComponent<PaddleMovement>().Reset();
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
