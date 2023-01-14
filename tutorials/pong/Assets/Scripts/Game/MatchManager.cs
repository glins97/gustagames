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

	[SyncObject]
	public readonly SyncList<Player> players = new();

	[SyncVar]
	private bool _canStart = false;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
        Debug.Log($"_CanStart: {_canStart}, Players: {players.Count}, isServer: {IsServer}, isClient: {IsClient}, this {this}");
        for (int i = 0; i < players.Count; i++) {
            Player player = players[i];
            Paddle paddle = player.controlledPaddle;
            // Debug.Log($"Player: {i} isReady: {player.isReady} isPlayer1: {player.isPlayer1} Paddle: {paddle.GetInstanceID()}");
        }
		if (!IsServer) return;
		_canStart = players.All(player => player.isReady);
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
    
}
