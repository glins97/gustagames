using FishNet.Object;
using FishNet.Object.Synchronizing;

public sealed class Paddle : NetworkBehaviour
{
	[SyncVar]
	public Player controllingPlayer;

}
