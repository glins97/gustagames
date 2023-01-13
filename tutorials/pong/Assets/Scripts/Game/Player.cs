using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;

using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class Player : NetworkBehaviour
{
	public static Player Instance { get; private set; }
    
	[SyncVar]
	public bool isReady;

    [SyncVar]
    public bool isPlayer1;

	[SyncVar]
    public Paddle controlledPaddle;

	public override void OnStartServer() {
		base.OnStartServer();

		bool added = MatchManager.Instance.addPlayer(this);

        Debug.Log($"Players: {MatchManager.Instance.players.Count}");
        if (added) {
            SpawnPaddle();
        }
        else {
            Debug.LogError("Failed to add player to match manager");
        }
	}

    public void SpawnPaddle() {
        GameObject paddlePrefab = Addressables.LoadAssetAsync<GameObject>("Paddle").WaitForCompletion();
        GameObject paddle = Instantiate(paddlePrefab, Vector3.zero, Quaternion.identity);
        Spawn(paddle, Owner);
        
        PaddleMovement paddleScript = paddle.GetComponent<PaddleMovement>();
        controlledPaddle = paddle.GetComponent<Paddle>();
        Debug.Log($"Paddle: {controlledPaddle.GetInstanceID()}");
        paddleScript.isPlayer1 = isPlayer1;
    }

	public override void OnStopServer()
	{
		base.OnStopServer();

		MatchManager.Instance.players.Remove(this);
	}


}
