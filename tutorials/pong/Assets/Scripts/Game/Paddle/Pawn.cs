using FishNet.Object;
using FishNet.Object.Synchronizing;

public sealed class Pawn : NetworkBehaviour
{
	[SyncVar]
	public Player controllingPlayer;

}
