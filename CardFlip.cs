using UnityEngine;
using System.Collections;

public class CardFlip : MonoBehaviour {

	bool isRotate;
	bool isRotateBack;
	SpriteRenderer spriteRenderer;
	public LevelManager levelManager;
	private Constants.Card thisCard;
	private bool isHide;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		isRotate = false;
		isRotateBack = false;
		thisCard = null;
		isHide = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isRotate){
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 3.33f, 0);
			if(transform.eulerAngles.y >= 90){
				isRotate = false;
				isRotateBack = true;
				transform.eulerAngles = new Vector3(0, 90, 0);
				if(isHide){
					spriteRenderer.sprite = Constants.CARDBACK;
				}else{
					spriteRenderer.sprite = thisCard.getSprite();
				}
			}
		}
		if(isRotateBack){
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 3.33f, 0);
			if(transform.rotation.y <= 0){
				isRotate = false;
				isRotateBack = false;
				transform.eulerAngles = new Vector3(0, 0, 0);
			}
		}

	}

	public void doShow(Constants.Card card) {
		thisCard = card;
		isRotate = true;
		isHide = false;
	}

	public void doHide(){
		isHide = true;
		isRotate = true;
	}
}
