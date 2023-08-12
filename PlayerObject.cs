using UnityEngine;
using System.Collections;

public class PlayerObject : System.IComparable<PlayerObject>{

	string name;
	int cash;
	Constants.Card card1;
	Constants.Card card2;
	HandUtil.PlayerHand playerhand;
	NetworkPlayer networkPlayer;
	string guid;
	int playerNumber;
	bool isActive;
	int amountBet;
	bool betPending;

	public PlayerObject(NetworkPlayer networkPlayer, int playerNumber){
		this.networkPlayer = networkPlayer;
		this.guid = networkPlayer.guid;
		this.playerNumber = playerNumber;
		this.isActive = true; 
		amountBet = 0;
		betPending = true;
	}

	public int getPlayerNumber(){
		return playerNumber;
	}

	public bool IsActive {
		get {
			return isActive;
		}
		set {
			isActive = value;
		}
	}

	public HandUtil.PlayerHand Playerhand {
		get {
			return playerhand;
		}
		set {
			playerhand = value;
		}
	}

	public string Name {
		get {
			return name;
		}
		set {
			name = value;
		}
	}

	public int Cash {
		get {
			return cash;
		}
		set {
			cash = value;
		}
	}

	public Constants.Card Card1 {
		get {
			return card1;
		}
		set {
			card1 = value;
		}
	}

	public Constants.Card Card2 {
		get {
			return card2;
		}
		set {
			card2 = value;
		}
	}

	public NetworkPlayer NetworkPlayer {
		get {
			return networkPlayer;
		}
	}

	public string Guid {
		get {
			return guid;
		}
		set {
			guid = value;
		}
	}

	public int AmountBet {
		get {
			return amountBet;
		}
		set {
			amountBet = value;
		}
	}

	public bool BetPending {
		get {
			return betPending;
		}
		set {
			betPending = value;
		}
	}

	public int CompareTo(PlayerObject player) {
		return this.playerhand.CompareTo(player.playerhand);
	}

}
