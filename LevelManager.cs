using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
		
	protected GameManager gManager;
	protected List<Constants.Card> deck;
	protected int upperRange;
	protected IDictionary<string, PlayerObject> players;
	protected Constants.handState handstate;

	protected Constants.Card flop1;
	protected Constants.Card flop2;
	protected Constants.Card flop3;
	protected Constants.Card turn;
	protected Constants.Card river;

	public CardFlip cflop1;
	public CardFlip cflop2;
	public CardFlip cflop3;
	public CardFlip cturn;
	public CardFlip criver;

	private int pot;
	private int blind;

	private int playerRotationCount;
	private int startingRotationCount;
	private int minimumBet;
	private int callAmount;

	private bool handledLargeBlind;
	private bool handledSmallBlind;
	private bool hasFlippedFlop;
	private bool hasFlippedTurn;
	private bool hasFlippedRiver;

	private bool guiShowCards;

	private bool isNewHand;
	private bool promptNextPlayer;

	private List<PlayerObject> winners;

	private PlayerObject currentPlayer;

	// Use this for initialization
	void Awake() {
		gManager = GameManager.Instance;
	}

	//used for communication between objects after all objects have been initialized.

	void Start () {
		deck = Constants.getCardDeck();
		upperRange = 51;
		players = new Dictionary<string, PlayerObject>();
		handstate = Constants.handState.HAND_STATE_WAITING;
		playerRotationCount = 1;
		startingRotationCount = 1;
		pot = 0;
		minimumBet = 0;
		callAmount = 0;
		winners = new List<PlayerObject>();
		hasFlippedFlop = false;
		hasFlippedTurn = false;
		hasFlippedRiver = false;
		guiShowCards = false;
	}

	void Update () {
				
		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();	
		}
		if(handstate == Constants.handState.HAND_STATE_DEALING){

			//new hand setup
			isNewHand = true;
			winners = null;
			setPlayersActive();
			deck = Constants.getCardDeck();
			upperRange = 51;
			guiShowCards = false;
			pot = 0;
			minimumBet = 0;
			callAmount = 0;
			hasFlippedFlop = false;
			hasFlippedTurn = false;
			hasFlippedRiver = false;
			cflop1.doHide();
			cflop2.doHide();
			cflop3.doHide();
			cturn.doHide();
			criver.doHide();
			handledLargeBlind = false;
			handledSmallBlind = false;
			//---------------

			PlayerObject player = null;
			foreach(string key in players.Keys){
				players.TryGetValue(key, out player);
				if(null != player){
					player.Card1 = getCard();
					player.Card2 = getCard();
//					players[player.Guid] = player;
				}
			}
			flop1 = getCard ();
			flop2 = getCard ();
			flop3 = getCard ();
			turn =  getCard ();
			river = getCard ();
			moveCardsForward();
			handstate = Constants.handState.HAND_STATE_DEAL_BETTING;
		}

		if(handstate == Constants.handState.HAND_STATE_DEAL_BETTING){
			if(!handledSmallBlind){
				currentPlayer = getPlayerAtIndex(playerRotationCount);

				int halfBlind = 0;
				if(blind % 2 == 0){
					halfBlind = blind / 2;
				}else{
					halfBlind = (blind / 2) + 1;
				}
				
				currentPlayer.Cash -= halfBlind;
				currentPlayer.AmountBet = halfBlind;


				pot += halfBlind;
				handledSmallBlind = true;
				updatePlayerTurn();
			}
			if(!handledLargeBlind){
				PlayerObject player = getPlayerAtIndex(playerRotationCount);
				player.Cash -= blind;
				player.AmountBet = blind;
				pot += blind;
				handledLargeBlind = true;
				callAmount = blind;
				updatePlayerTurn();
				promptNextPlayer = true;
			}

		}
		if(handstate == Constants.handState.HAND_STATE_FLOP_BETTING){
			if(!hasFlippedFlop){
				cflop1.doShow(flop1);
				cflop2.doShow(flop2);
				cflop3.doShow(flop3);
				hasFlippedFlop = true;
			}
		}
		if(handstate == Constants.handState.HAND_STATE_TURN_BETTING){
			if(!hasFlippedTurn){
				cturn.doShow(turn);
				hasFlippedTurn = true;
			}
		}
		if(handstate == Constants.handState.HAND_STATE_RIVER_BETTING){
			if(!hasFlippedRiver){
				criver.doShow(river);
				hasFlippedRiver = true;
			}
		}
	}

	public PlayerObject getPlayerAtIndex(int index){
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);
			if(null != player){
				if(player.getPlayerNumber() == index){
					return player;
				}
			}
		}
		return null;
	}

	public void setPlayerCash(int startingCash){
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);
			if(null != player){
				player.Cash = startingCash;
			}
		}
	}

	public void OnApplicationPause(){
		//TODO: add behavior for when game is left.
	}

	public int getGameState(){
		return gManager.getGameState();
	}
	public void setGameState(int newState){
		gManager.setGameState(newState);
	}

	public Constants.Card getCard(){
		int cardIndex = Random.Range(0,upperRange);
		Constants.Card card = deck[cardIndex];
		deck.Remove(card);
		upperRange--;
		return card;
	}
	public void addPlayer(NetworkPlayer player){
		players.Add(player.guid, new PlayerObject(player, (players.Count + 1)));
	}
//	public void addPlayer(PlayerObject player){
//		players.Add(player.Guid, player);
//	}
	public IDictionary<string, PlayerObject> getPlayers(){
		return players;
	}
	public void addPlayerName(string playerName, string guid){
		PlayerObject player = null;
		players.TryGetValue(guid, out player);
		if(null != player){
			player.Name = playerName;
		}
	}
	public void removePlayer(NetworkPlayer player){
		if(players.ContainsKey(player.guid)){
			players.Remove(player.guid);
		}
	}

	public Constants.handState getHandState(){
		return handstate;
	}
	public void setHandState(Constants.handState handstate){
		this.handstate = handstate;
	}
	public int getPlayerNumber(string guid){
		int playerNumber = 0;
		PlayerObject player = null;
		players.TryGetValue(guid, out player);
		if(null != player){
			playerNumber = player.getPlayerNumber();
		}
		return playerNumber;
	}
	public Constants.Card playerCard(int cardNumber, string guid){
		Constants.Card ret = null;
		PlayerObject player = null;
		players.TryGetValue(guid, out player);
		if(null != player){
			if(cardNumber == 1){
				ret = player.Card1;
			}else{
				ret = player.Card2;
			}
		}
		return ret;
	}

	public void moveCardsForward(){
		cflop1.transform.position = new Vector3(cflop1.transform.position.x, cflop1.transform.position.y, -1);
		cflop2.transform.position = new Vector3(cflop2.transform.position.x, cflop2.transform.position.y, -1);
		cflop3.transform.position = new Vector3(cflop3.transform.position.x, cflop3.transform.position.y, -1);
		cturn.transform.position = new Vector3(cturn.transform.position.x, cturn.transform.position.y, -1);
		criver.transform.position = new Vector3(criver.transform.position.x, criver.transform.position.y, -1);
	}

	public void doFlop(){
		cflop1.doShow(flop1);
		cflop2.doShow(flop2);
		cflop3.doShow(flop3);
	}
	public void doTurn(){
		cturn.doShow(turn);
	}
	public void doRiver(){
		criver.doShow(river);
	}

	public List<PlayerObject> compareHands(){
		List<PlayerObject> finalists = new List<PlayerObject>();
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);

			if(!player.IsActive){
				continue;
			}

			List<Constants.Card> cards = new List<Constants.Card>();

			cards.Add(flop1);
			cards.Add(flop2);
			cards.Add(flop3);
			cards.Add(turn);
			cards.Add(river);
			cards.Add(player.Card1);
			cards.Add(player.Card2);

			player.Playerhand = HandUtil.getThisHand(cards);
			finalists.Add(player);
		}
		finalists.Sort();
		finalists.Reverse();
		PlayerObject currentBest = null;
		List<PlayerObject> removeFromFinalists = new List<PlayerObject>();

		foreach(PlayerObject finalist in finalists){
			if(currentBest == null){
				currentBest = finalist;
			}
			if(finalist.CompareTo(currentBest) == -1){
				removeFromFinalists.Add(finalist);
			}else{
				currentBest = finalist;
			}
		}

		foreach(PlayerObject loser in removeFromFinalists){
			finalists.Remove(loser);
		}
		return finalists;
	}

	public void prepNextPlayer(int betAmount, string betType){
		pot += betAmount;
		if(betType == Constants.BetType.BET_TYPE_RAISE.ToString()){
			minimumBet = betAmount;
			setPlayersPending();
			callAmount = betAmount + currentPlayer.AmountBet;
		}else if(betType == Constants.BetType.BET_TYPE_FOLD.ToString()){
			currentPlayer.IsActive = false;
			checkNumberActive();
		}
		currentPlayer.BetPending = false;
		currentPlayer.AmountBet += betAmount;
		currentPlayer.Cash -=  betAmount;

		updatePlayerTurn();
	}

	void checkNumberActive ()
	{
		int activePlayerCount = 0;
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);
			if(null != player){
				if(player.IsActive){
					activePlayerCount++;
				}
			}
		}
		if(activePlayerCount < 2){
			endHand(false);
		}
	}

	void setPlayersPending ()
	{
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);
			if(null != player){
				player.BetPending = true;
			}
		}
	}

	void setPlayersActive(){
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);
			if(null != player){
				if(player.Cash > 0){
					player.IsActive = true;
				}else{
					if(player.getPlayerNumber() < playerRotationCount){
						playerRotationCount--;
					}
					players.Remove(player.Guid);
					Network.CloseConnection(player.NetworkPlayer, true);
				}
			}
		}

	}

	void resetPlayersBetAmounts ()
	{
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);
			if(null != player){
				player.AmountBet = 0;
			}
		}
	}

	public void updatePlayerTurn(){
		playerRotationCount++;
		if(playerRotationCount > players.Count){
			playerRotationCount = 1;
		}
		currentPlayer = getPlayerAtIndex(playerRotationCount);
		if(currentPlayer.IsActive){
			if(currentPlayer.BetPending){
				promptNextPlayer = true;
			}else{
				endTurn();
			}
		}else{
			updatePlayerTurn();
		}
	}

	public void endHand(bool showCards){
		winners = compareHands();
		guiShowCards = showCards;
		foreach(PlayerObject player in winners){
			if(null != player){
				player.Cash += pot / winners.Count;
			}
		}
		handstate = Constants.handState.HAND_STATE_SHOW_WINNER;
		startingRotationCount++;
		if(startingRotationCount > players.Count){
			startingRotationCount = 1;
		}
		playerRotationCount = startingRotationCount;

	}

	public void endTurn(){
		minimumBet = blind;
		playerRotationCount = startingRotationCount + 2;
		if(playerRotationCount == players.Count + 1){
			playerRotationCount = 1;
		}else if(playerRotationCount == players.Count + 2){
			playerRotationCount = 2;
		}
		currentPlayer = getPlayerAtIndex(playerRotationCount);
		setPlayersPending();
		resetPlayersBetAmounts();
		callAmount = 0;
		if(handstate == Constants.handState.HAND_STATE_DEAL_BETTING){
			handstate = Constants.handState.HAND_STATE_FLOP_BETTING;
			promptNextPlayer = true;
		}else if(handstate == Constants.handState.HAND_STATE_FLOP_BETTING){
			handstate = Constants.handState.HAND_STATE_TURN_BETTING;
			promptNextPlayer = true;
		}else if(handstate == Constants.handState.HAND_STATE_TURN_BETTING){
			handstate = Constants.handState.HAND_STATE_RIVER_BETTING;
			promptNextPlayer = true;
		}else if(handstate == Constants.handState.HAND_STATE_RIVER_BETTING){
			endHand(true);
		}
	}
	public int getCurrentPlayerCash(){
		return currentPlayer.Cash;
	}

	public int getPot(){
		return pot;
	}

	public bool newHand(){
		return isNewHand;
	}
	public void setNewHand(bool newHandValue){
		isNewHand = newHandValue;
	}

	public NetworkPlayer getCurrentNetworkPlayer(){
		return currentPlayer.NetworkPlayer;
	}

	public int getMinimumBet(){
		return minimumBet;
	}
	public int getCallAmount(){
		return callAmount - currentPlayer.AmountBet;
	}
	public void setCallAmount(int newAmount){
		callAmount = newAmount;
	}
	public void setBlind(int blind){
		this.blind = blind;
	}
	public bool getPromptNextPlayer(){
		return promptNextPlayer;
	}
	public void setPromptNextPlayer(bool prompt){
		 promptNextPlayer = prompt;
	}
	public int getPlayerRotationCount(){
		return playerRotationCount;
	}
	public string getCurrentPlayerName(){
		return currentPlayer.Name;
	}
	public List<PlayerObject> Winners {
		get {
			return winners;
		}
		set {
			winners = value;
		}
	}
	public string getWinnerNames(){
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		int winnerCount= 0;
		foreach(PlayerObject player in winners){
			if(winnerCount == 0){
				sb.Append(player.Name);
			}else{
				if(winnerCount + 1 == winners.Count){
					sb.Append(" and " + player.Name);
				}else{
					sb.Append(", " + player.Name);
				}
			}
			winnerCount++;
		}
		if(winners.Count > 1){
			sb.Append(" tied");
		}else{
			sb.Append(" wins");
		}
		return sb.ToString();
	}

	public bool GuiShowCards {
		get {
			return guiShowCards;
		}
		set {
			guiShowCards = value;
		}
	}
}
