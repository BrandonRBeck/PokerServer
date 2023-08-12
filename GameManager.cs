using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {
	
	private static GameManager instance = null;
	
	public int gameState;
	public string currentScene;
	
	public const int GAMESTATE_SERVER = 1;
	public const int GAMESTATE_CLIENT = 2;
	
	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType(typeof (GameManager)) as GameManager;
			}
			
			if (instance == null) {
				GameObject obj = new GameObject("GameManager");
				instance = obj.AddComponent(typeof (GameManager)) as GameManager;
				Debug.Log("Could not locate a GameManager object, one was generated automatically.");
			}
			
			return instance;
		}
	}
	
	void Awake() {
		DontDestroyOnLoad(this);
		currentScene = "menu";
	}
	
	void OnApplicationQuit() {
		instance = null;
	}
	// Use this for initialization
	void Start () {
				
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();	
		}
	}
	public int getGameState(){
		return gameState;	
	}
	public void setGameState(int newGameState){
		gameState = newGameState;
	}
	public string getScene(){
		return currentScene;	
	}
	public void setScene(string newScene){
		currentScene = newScene;
	}
			
}