using UnityEngine;
using System.Collections;

public class TransitionScript : MonoBehaviour {
	public GameManager gManager;
	// Use this for initialization
	void Start () {
		gManager = GameManager.Instance;
		Application.LoadLevel(gManager.getScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
