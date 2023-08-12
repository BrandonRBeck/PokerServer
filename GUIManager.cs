using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

public class GUIManager : MonoBehaviour {

	public Transform felt;
	public Transform card;

	protected GameManager gManager;
	public LevelManager levelManager;
	private int connectionPort;
	private string yourName;
	private string serverIp;
	private bool blockGUI;
	private bool hasInitClient;
	private bool hasSentCards;
	private bool disableFlip;
	private float disableFlipTime;
	private float winnerTimer;
	private bool showingCards;
	private bool gameStarted;
	private bool promptedShowHands;

	CardFlip card1;
	CardFlip card2;
	Constants.Card ccard1;
	Constants.Card ccard2;
	private bool isTurn;
	private bool isRaiseDialogue;
	private bool hasUpdatedWinnerCash;
	private int minimumBid;
	private int callAmount;
	private string bidAmount;

	private string startingCash;
	private string startingBlind;
	private string errorMessage;

	private int playerCash;

	void Awake() {
		gManager = GameManager.Instance;
	}

	// Use this for initialization
	void Start () {
		//TODO: Remove this;
//		gManager.setGameState(GameManager.GAMESTATE_SERVER); // TESTING

		serverIp = "192.168.1.100";
		yourName = "Brandon";
		startingCash = "100";
		startingBlind = "2";
		connectionPort = 25001;
		disableFlip = false;
		hasInitClient = false;
		hasSentCards = false;
		card1 = null;
		card2 = null;
		ccard1 = null;
		ccard2 = null;
		disableFlipTime = 0;
		showingCards = false;
		blockGUI = false;
		isTurn = false;
		isRaiseDialogue = false; 
		bidAmount = "";
		minimumBid = 0;
		callAmount = 0;
		errorMessage = "";
		playerCash = 0;
		winnerTimer = 0;
		gameStarted = false;
		promptedShowHands = false;
		hasUpdatedWinnerCash = false;

		if(gManager.getGameState() == GameManager.GAMESTATE_SERVER){
			Network.InitializeServer(10, connectionPort, false);
		}

	}

	// Update is called once per frame
	void Update () {

		if(Network.isClient && !hasInitClient){
			Instantiate(felt);
			Transform placeHolder = Instantiate (card, new Vector3(15.5f, 20, -1), Quaternion.identity) as Transform;

			card1 = placeHolder.gameObject.GetComponent<CardFlip>();
			placeHolder = Instantiate (card, new Vector3(19, 20, -1), Quaternion.identity) as Transform;
			card2 = placeHolder.gameObject.GetComponent<CardFlip>();
			Camera.main.transform.position = new Vector3(20,20,Camera.main.transform.position.z);
			hasInitClient = true;
		}

		if(Network.isServer && levelManager.getHandState() == Constants.handState.HAND_STATE_DEAL_BETTING){
			if(!hasSentCards){
				IDictionary<string, PlayerObject> players = levelManager.getPlayers();
				PlayerObject player = null;
				foreach(string key in players.Keys){
					players.TryGetValue(key, out player);
					networkView.RPC ("sendCard", player.NetworkPlayer, 1, player.Card1.getSuit().ToString(), player.Card1.getValue().ToString());
					networkView.RPC ("sendCard", player.NetworkPlayer, 2, player.Card2.getSuit().ToString(), player.Card2.getValue().ToString());
				}
				hasSentCards = true;
			}
		}
		if(disableFlip){
			disableFlipTime += Time.deltaTime;
			if(disableFlipTime > 1){
				disableFlipTime = 0;
				disableFlip = false;
			}
		}
		if(levelManager.newHand()){
			hasSentCards = false;
			promptedShowHands = false;
			winnerTimer = 0;
			hasUpdatedWinnerCash = false;
			levelManager.setNewHand(false);
		}
	}
	//Calls to Client ------------------------------------------------------------------------------------
	[RPC]
	void promptSendName(){
		networkView.RPC("sendPlayerName", RPCMode.Server, yourName + ":" + Network.player.guid);
	}
	[RPC]
	void startGame(int playerStartingCash){
		gameStarted = true;
		playerCash = playerStartingCash;
	}

	[RPC]
	void forceShowCards(){
		if(!showingCards){
			card1.doShow(ccard1);
			card2.doShow(ccard2);
			showingCards = true;
			disableFlip = true;
		}
	}

	[RPC]
	void forceHideCards(){
		if(showingCards){
			card1.doHide();
			card2.doHide();
			showingCards = false;
			disableFlip = true;
		}
	}

	[RPC]
	void sendCard(int cardNum, string suit, string value){
		if(cardNum == 1){
			ccard1 = Constants.getCardByValues(suit, value);
		}else if (cardNum == 2){
			ccard2 = Constants.getCardByValues(suit, value);
		}
	}

	[RPC]
	void updatePlayerCash(int newCash){
		playerCash = newCash;
	}

	[RPC]
	void setPlayerTurn(int minBet, int callAmount){
		minimumBid = minBet;
		this.callAmount = callAmount;
		bidAmount = minBet.ToString();//default set the bid prompt to the minimum amount.
		isTurn = true;
//		Handheld.Vibrate();
	}

	//	-------------------------------------------------------------------------------------------------


	//Calls to Server ------------------------------------------------------------------------------------
	[RPC]
	void sendPlayerName(string name){
		string[] parsedName = name.Split(':');
		levelManager.addPlayerName(parsedName[0], parsedName[1]);
	}

	[RPC]
	void sendBet(int betAmount, string betType){
		levelManager.prepNextPlayer(betAmount, betType);
	}

	//	-------------------------------------------------------------------------------------------------

	void OnPlayerConnected(NetworkPlayer player) {
		levelManager.addPlayer(player);
		networkView.RPC("promptSendName", player);
	}

	void OnPlayerDisconnected(NetworkPlayer player){
		levelManager.removePlayer(player);
	}

	void OnGUI(){

		GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.fontSize =30;
		buttonStyle.alignment =TextAnchor.MiddleCenter;
		
		GUIStyle textFieldstyle = new GUIStyle(GUI.skin.textField);
		textFieldstyle.fontSize =30;
		textFieldstyle.alignment = TextAnchor.MiddleLeft;

		GUIStyle bigStyle = new GUIStyle(GUI.skin.textField);
		bigStyle.fontSize =50;
		bigStyle.alignment = TextAnchor.MiddleCenter;

		GUIStyle smallTextFieldstyle = new GUIStyle(GUI.skin.textField);
		smallTextFieldstyle.fontSize =20;
		smallTextFieldstyle.alignment = TextAnchor.MiddleLeft;
		
		GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
		labelStyle.fontSize =30;
		labelStyle.alignment = TextAnchor.MiddleRight;

		GUIStyle errorLabelStyle = new GUIStyle(GUI.skin.label);
		errorLabelStyle.fontSize =15;
		errorLabelStyle.alignment = TextAnchor.MiddleCenter;
		errorLabelStyle.normal.textColor = Color.red;

		GUIStyle smallLabelStyle = new GUIStyle(GUI.skin.label);
		smallLabelStyle.fontSize =20;

		GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
		boxStyle.fontSize =30;
		boxStyle.alignment = TextAnchor.UpperCenter;

		// Server GUI ------------------------------------------------------------------------------------
		if(Network.isServer){

			if(levelManager.getHandState() == Constants.handState.HAND_STATE_WAITING){
				labelStyle.alignment = TextAnchor.MiddleCenter;
				GUI.Label(new Rect(10, 10, Screen.width, Screen.height/10), "Server Address: " + Network.player.ipAddress, labelStyle);

				GUILayout.BeginArea(new Rect((Screen.width)/2 + 40,Screen.height/10 + 20,(Screen.width)/2 - 80, Screen.height/10 * 8), boxStyle);
				{
					GUILayout.BeginVertical(); 
					{
						GUILayout.Label("Connected Players", labelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 1: " + getPlayer(1), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 2: " + getPlayer(2), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 3: " + getPlayer(3), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 4: " + getPlayer(4), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 5: " + getPlayer(5), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 6: " + getPlayer(6), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 7: " + getPlayer(7), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 8: " + getPlayer(8), smallLabelStyle, GUILayout.ExpandHeight(true));
						GUILayout.Label("Player 9: " + getPlayer(9), smallLabelStyle, GUILayout.ExpandHeight(true));
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();


				GUILayout.BeginArea(new Rect(40,Screen.height/10 + 20,(Screen.width)/2 - 80, Screen.height/10 * 3));
				{
					GUILayout.BeginVertical();
					{
	//					labelStyle.alignment = TextAnchor.MiddleRight;
						textFieldstyle.alignment = TextAnchor.UpperLeft;
						GUILayout.Label("Starting Cash", labelStyle, GUILayout.ExpandHeight(true));
						startingCash = GUILayout.TextField(startingCash, textFieldstyle, GUILayout.ExpandHeight(true));
						startingCash = Regex.Replace(startingCash, @"[^0-9]", "");
						if(startingCash.Length > 12){
							startingCash = startingCash.Substring(0,12);
						}
						GUILayout.Label("Large Blind", labelStyle, GUILayout.ExpandHeight(true));
						startingBlind = GUILayout.TextField(startingBlind, textFieldstyle, GUILayout.ExpandHeight(true));
						startingBlind = Regex.Replace(startingBlind, @"[^0-9]", "");
						if(startingBlind.Length > 9){
							startingBlind = startingBlind.Substring(0,9);
						}
						if(!"".Equals(startingBlind) && !"".Equals(startingCash)){
							if(System.Int32.Parse(startingBlind) > System.Int32.Parse(startingCash) / 10){
								startingBlind = (System.Int32.Parse(startingCash) / 10).ToString();
							}
						}

					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();

				GUILayout.BeginArea(new Rect(40,Screen.height/10 * 5 + 20,(Screen.width)/2 - 80, Screen.height/10 * 4));
				{
					GUILayout.BeginVertical();
					{
						if(levelManager.getPlayers().Count < 2){
							GUI.enabled = false;
						}
						if(GUILayout.Button("Begin", buttonStyle, GUILayout.ExpandHeight(true)))
						{
							networkView.RPC("startGame", RPCMode.Others, System.Int32.Parse(startingCash)); 
							Network.maxConnections = -1;
							levelManager.setPlayerCash(System.Int32.Parse(startingCash));
							levelManager.setBlind(System.Int32.Parse(startingBlind));
							levelManager.setHandState(Constants.handState.HAND_STATE_DEALING);
						}
						GUI.enabled = true;
						if (GUILayout.Button("Disconnect", buttonStyle, GUILayout.ExpandHeight(true)))
						{ 
							blockGUI = true;
							Network.Disconnect(200);
							gManager.setScene("menu");
							Application.LoadLevel("transitionScene");
						}
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();

			}else if(levelManager.getHandState() == Constants.handState.HAND_STATE_DEAL_BETTING || 
			         levelManager.getHandState() == Constants.handState.HAND_STATE_FLOP_BETTING ||
			         levelManager.getHandState() == Constants.handState.HAND_STATE_TURN_BETTING ||
			         levelManager.getHandState() == Constants.handState.HAND_STATE_RIVER_BETTING){
				GUILayout.BeginArea(new Rect(0,0, Screen.width,Screen.height));
				{
					GUILayout.BeginVertical();
					{
						GUILayout.Label("Pot: " + levelManager.getPot().ToString(), labelStyle);
						GUILayout.Label(levelManager.getCurrentPlayerName() +"'s turn.", labelStyle);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();

				if(levelManager.getPromptNextPlayer()){
					updateAllPlayersCash();
					networkView.RPC("setPlayerTurn", levelManager.getCurrentNetworkPlayer(), levelManager.getMinimumBet(),
					    levelManager.getCallAmount());
					levelManager.setPromptNextPlayer(false);
				}
			}else if(levelManager.getHandState() == Constants.handState.HAND_STATE_SHOW_WINNER){

				if(!hasUpdatedWinnerCash){
					updateAllPlayersCash();
					hasUpdatedWinnerCash = true;
				}

				if(winnerTimer < 5f && levelManager.GuiShowCards){
					if(!promptedShowHands){
						networkView.RPC("forceShowCards", RPCMode.Others);
						promptedShowHands = true;
					}
					winnerTimer += Time.deltaTime;
					GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));
					{
						GUILayout.BeginVertical();
						{
							GUILayout.Label("Show Hands", bigStyle);
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}else{
					GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height));
					{
						GUILayout.BeginVertical();
						{
							GUILayout.Label("Pot: " + levelManager.getPot().ToString(), labelStyle);
							GUILayout.Label("Winner: " + levelManager.getWinnerNames(), labelStyle);
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
					GUILayout.BeginArea(new Rect(0,Screen.height/10 * 8, Screen.width, Screen.height/10 * 2));
					{
						GUILayout.BeginHorizontal();
						{
							if(GUILayout.Button("New Hand", buttonStyle, GUILayout.ExpandHeight(true))){
								networkView.RPC("forceHideCards", RPCMode.Others); 
								levelManager.setHandState(Constants.handState.HAND_STATE_DEALING);
							}
							if(GUILayout.Button("Change Blind", buttonStyle, GUILayout.ExpandHeight(true))){
								//do something, eventually.
							}
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndArea();
				}
			}


		// Client GUI ------------------------------------------------------------------------------------
		}else if((Network.peerType == NetworkPeerType.Disconnected || Network.isClient) && !blockGUI){
				
			if (Network.peerType == NetworkPeerType.Disconnected)
			{
				serverIp = GUI.TextField(new Rect(Screen.width/2,Screen.height/8 ,Screen.width/2 -20,Screen.height/8), serverIp, textFieldstyle);
				serverIp = Regex.Replace(serverIp, @"[^0-9.]", "");
				if(serverIp.Length > 15){
					serverIp = serverIp.Substring(0,15);
				}
				
				yourName = GUI.TextField(new Rect(Screen.width/2,(Screen.height/8 * 3) , Screen.width/2 - 20, Screen.height/8), yourName, textFieldstyle);
				
				GUI.Label(new Rect(0,Screen.height/8,Screen.width/2 -20,Screen.height/8), "Server Address", labelStyle);
				GUI.Label(new Rect(0,Screen.height/8*3,Screen.width/2 -20,Screen.height/8), "Your Name", labelStyle);
				
				if(GUI.Button(new Rect((Screen.width)/9,Screen.height/8*5,(Screen.width)/3, Screen.height/4), "Connect", buttonStyle)){
					Network.Connect(serverIp, connectionPort);
				}
				if(GUI.Button(new Rect((Screen.width)/9 * 5,Screen.height/8*5,(Screen.width)/3, Screen.height/4), "Back", buttonStyle)){
					gManager.setScene("menu");
					Application.LoadLevel("transitionScene");
				}
			}else{
				if(!gameStarted){
					GUILayout.BeginArea(new Rect(0,0, Screen.width,Screen.height));
					{
						GUILayout.BeginVertical();
						{
							GUILayout.Label("Connected. Waiting for server.",labelStyle);
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}else{

					GUILayout.BeginArea(new Rect(Screen.width/2 + 40, Screen.height/10, Screen.width/2 - 80,Screen.height/10 * 8));
					{
						GUILayout.BeginVertical();
						{
							GUILayout.Label("cash " + playerCash, labelStyle);
							if(disableFlip){
								GUI.enabled = false;
							}else{
								GUI.enabled = true;
							}
								if(showingCards){
									if(GUILayout.Button("Hide Cards", buttonStyle, GUILayout.ExpandHeight(true)))
									{
										card1.doHide();
										card2.doHide();
										showingCards = false;
										disableFlip = true;
									}
								}else{
									if(GUILayout.Button("Show Cards", buttonStyle, GUILayout.ExpandHeight(true)))
									{
										card1.doShow(ccard1);
										card2.doShow(ccard2);
										showingCards = true;
										disableFlip = true;
									}
								}
							
							if(!isTurn){
								GUI.enabled = false;
							}

							if(isRaiseDialogue){
								GUILayout.Label(errorMessage, errorLabelStyle);
								bidAmount = GUILayout.TextField(bidAmount, textFieldstyle);
								bidAmount = Regex.Replace(bidAmount, @"[^0-9]", "");
								if(GUILayout.Button("Bet", buttonStyle, GUILayout.ExpandHeight(true)))
								{
									int bidInt = 0;
									System.Int32.TryParse(bidAmount, out bidInt);
									if(bidInt < minimumBid){
										bidAmount = minimumBid.ToString();
										errorMessage = "Raise cannot be less than last bet ("+minimumBid+").";
									}else if(bidInt > playerCash){
										bidAmount = playerCash.ToString ();
										errorMessage = "Raise cannot be more than available cash ("+playerCash+").";
									}else{
										isTurn = false;
										isRaiseDialogue = false;
										playerCash -= bidInt;
										networkView.RPC("sendBet", RPCMode.Server, bidInt, Constants.BetType.BET_TYPE_RAISE.ToString());
									}
								}
								if(GUILayout.Button("Cancel", buttonStyle, GUILayout.ExpandHeight(true)))
								{
									isRaiseDialogue = false;
								}
							}else{
								if(GUILayout.Button("Raise", buttonStyle, GUILayout.ExpandHeight(true)))
								{
									isRaiseDialogue = true;
									
								}
								if(callAmount > 0){
									if(GUILayout.Button("Call ("+callAmount+")", buttonStyle, GUILayout.ExpandHeight(true)))
									{
										isTurn = false;
										playerCash -= callAmount;
										networkView.RPC("sendBet", RPCMode.Server, callAmount, Constants.BetType.BET_TYPE_CALL.ToString());
									}
								}else{
									if(GUILayout.Button("Check", buttonStyle, GUILayout.ExpandHeight(true)))
									{
										isTurn = false;
										networkView.RPC("sendBet", RPCMode.Server, 0, Constants.BetType.BET_TYPE_CHECK.ToString());
									}
								}
								if(GUILayout.Button("Fold", buttonStyle, GUILayout.ExpandHeight(true)))
								{
									isTurn = false;
									networkView.RPC("sendBet", RPCMode.Server, 0, Constants.BetType.BET_TYPE_FOLD.ToString());
								}
								GUI.enabled = true;
								if(GUILayout.Button("Disconnect", buttonStyle, GUILayout.ExpandHeight(true)))
								{
									Network.Disconnect(200);
								}

							}


						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
		}
		//	-------------------------------------------------------------------------------------------------
	}
	private string getPlayer(int index){
		IDictionary<string, PlayerObject> players = levelManager.getPlayers();
		if(players.Count < index){
			return "";
		}
		int i = 1;
		string playerKey = "";
		foreach(string key in players.Keys){
			if(i == index){
				playerKey = key;
			}
			i++;
		}
		string playername = "";
		PlayerObject player = null;
		players.TryGetValue(playerKey, out player);
		if(null != player){
			playername = player.Name;
		}

		return playername;
	}

	private void updateAllPlayersCash(){
		IDictionary<string, PlayerObject> players = levelManager.getPlayers();
		
		PlayerObject player = null;
		foreach(string key in players.Keys){
			players.TryGetValue(key, out player);
			networkView.RPC("updatePlayerCash", player.NetworkPlayer, player.Cash);
		}
	}
}
