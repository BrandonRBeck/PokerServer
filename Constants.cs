using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constants{

	public static Card TWO_HEARTS = new Card(Suit.HEARTS, Value.TWO, Resources.Load<Sprite>("Textures/Cards/2h"));
	public static Card TWO_CLUBS = new Card(Suit.CLUBS, Value.TWO, Resources.Load<Sprite>("Textures/Cards/2c"));
	public static Card TWO_DIAMONDS = new Card(Suit.DIAMONDS, Value.TWO, Resources.Load<Sprite>("Textures/Cards/2d"));
	public static Card TWO_SPADES = new Card(Suit.SPADES, Value.TWO, Resources.Load<Sprite>("Textures/Cards/2s"));

	public static Card THREE_HEARTS = new Card(Suit.HEARTS, Value.THREE, Resources.Load<Sprite>("Textures/Cards/3h"));
	public static Card THREE_CLUBS = new Card(Suit.CLUBS, Value.THREE, Resources.Load<Sprite>("Textures/Cards/3c"));
	public static Card THREE_DIAMONDS = new Card(Suit.DIAMONDS, Value.THREE, Resources.Load<Sprite>("Textures/Cards/3d"));
	public static Card THREE_SPADES = new Card(Suit.SPADES, Value.THREE, Resources.Load<Sprite>("Textures/Cards/3s"));

	public static Card FOUR_HEARTS = new Card(Suit.HEARTS, Value.FOUR, Resources.Load<Sprite>("Textures/Cards/4h"));
	public static Card FOUR_CLUBS = new Card(Suit.CLUBS, Value.FOUR, Resources.Load<Sprite>("Textures/Cards/4c"));
	public static Card FOUR_DIAMONDS = new Card(Suit.DIAMONDS, Value.FOUR, Resources.Load<Sprite>("Textures/Cards/4d"));
	public static Card FOUR_SPADES = new Card(Suit.SPADES, Value.FOUR, Resources.Load<Sprite>("Textures/Cards/4s"));

	public static Card FIVE_HEARTS = new Card(Suit.HEARTS, Value.FIVE, Resources.Load<Sprite>("Textures/Cards/5h"));
	public static Card FIVE_CLUBS = new Card(Suit.CLUBS, Value.FIVE, Resources.Load<Sprite>("Textures/Cards/5c"));
	public static Card FIVE_DIAMONDS = new Card(Suit.DIAMONDS, Value.FIVE, Resources.Load<Sprite>("Textures/Cards/5d"));
	public static Card FIVE_SPADES = new Card(Suit.SPADES, Value.FIVE, Resources.Load<Sprite>("Textures/Cards/5s"));

	public static Card SIX_HEARTS = new Card(Suit.HEARTS, Value.SIX, Resources.Load<Sprite>("Textures/Cards/6h"));
	public static Card SIX_CLUBS = new Card(Suit.CLUBS, Value.SIX, Resources.Load<Sprite>("Textures/Cards/6c"));
	public static Card SIX_DIAMONDS = new Card(Suit.DIAMONDS, Value.SIX, Resources.Load<Sprite>("Textures/Cards/6d"));
	public static Card SIX_SPADES = new Card(Suit.SPADES, Value.SIX, Resources.Load<Sprite>("Textures/Cards/6s"));
	
	public static Card SEVEN_HEARTS = new Card(Suit.HEARTS, Value.SEVEN, Resources.Load<Sprite>("Textures/Cards/7h"));
	public static Card SEVEN_CLUBS = new Card(Suit.CLUBS, Value.SEVEN, Resources.Load<Sprite>("Textures/Cards/7c"));
	public static Card SEVEN_DIAMONDS = new Card(Suit.DIAMONDS, Value.SEVEN, Resources.Load<Sprite>("Textures/Cards/7d"));
	public static Card SEVEN_SPADES = new Card(Suit.SPADES, Value.SEVEN, Resources.Load<Sprite>("Textures/Cards/7s"));

	public static Card EIGHT_HEARTS = new Card(Suit.HEARTS, Value.EIGHT, Resources.Load<Sprite>("Textures/Cards/8h"));
	public static Card EIGHT_CLUBS = new Card(Suit.CLUBS, Value.EIGHT, Resources.Load<Sprite>("Textures/Cards/8c"));
	public static Card EIGHT_DIAMONDS = new Card(Suit.DIAMONDS, Value.EIGHT, Resources.Load<Sprite>("Textures/Cards/8d"));
	public static Card EIGHT_SPADES = new Card(Suit.SPADES, Value.EIGHT, Resources.Load<Sprite>("Textures/Cards/8s"));

	public static Card NINE_HEARTS = new Card(Suit.HEARTS, Value.NINE, Resources.Load<Sprite>("Textures/Cards/9h"));
	public static Card NINE_CLUBS = new Card(Suit.CLUBS, Value.NINE, Resources.Load<Sprite>("Textures/Cards/9c"));
	public static Card NINE_DIAMONDS = new Card(Suit.DIAMONDS, Value.NINE, Resources.Load<Sprite>("Textures/Cards/9d"));
	public static Card NINE_SPADES = new Card(Suit.SPADES, Value.NINE, Resources.Load<Sprite>("Textures/Cards/9s"));

	public static Card TEN_HEARTS = new Card(Suit.HEARTS, Value.TEN, Resources.Load<Sprite>("Textures/Cards/10h"));
	public static Card TEN_CLUBS = new Card(Suit.CLUBS, Value.TEN, Resources.Load<Sprite>("Textures/Cards/10c"));
	public static Card TEN_DIAMONDS = new Card(Suit.DIAMONDS, Value.TEN, Resources.Load<Sprite>("Textures/Cards/10d"));
	public static Card TEN_SPADES = new Card(Suit.SPADES, Value.TEN, Resources.Load<Sprite>("Textures/Cards/10s"));
	
	public static Card JACK_HEARTS = new Card(Suit.HEARTS, Value.JACK, Resources.Load<Sprite>("Textures/Cards/jh"));
	public static Card JACK_CLUBS = new Card(Suit.CLUBS, Value.JACK, Resources.Load<Sprite>("Textures/Cards/jc"));
	public static Card JACK_DIAMONDS = new Card(Suit.DIAMONDS, Value.JACK, Resources.Load<Sprite>("Textures/Cards/jd"));
	public static Card JACK_SPADES = new Card(Suit.SPADES, Value.JACK, Resources.Load<Sprite>("Textures/Cards/js"));

	public static Card QUEEN_HEARTS = new Card(Suit.HEARTS, Value.QUEEN, Resources.Load<Sprite>("Textures/Cards/qh"));
	public static Card QUEEN_CLUBS = new Card(Suit.CLUBS, Value.QUEEN, Resources.Load<Sprite>("Textures/Cards/qc"));
	public static Card QUEEN_DIAMONDS = new Card(Suit.DIAMONDS, Value.QUEEN, Resources.Load<Sprite>("Textures/Cards/qd"));
	public static Card QUEEN_SPADES = new Card(Suit.SPADES, Value.QUEEN, Resources.Load<Sprite>("Textures/Cards/qs"));
	
	public static Card KING_HEARTS = new Card(Suit.HEARTS, Value.KING, Resources.Load<Sprite>("Textures/Cards/kh"));
	public static Card KING_CLUBS = new Card(Suit.CLUBS, Value.KING, Resources.Load<Sprite>("Textures/Cards/kc"));
	public static Card KING_DIAMONDS = new Card(Suit.DIAMONDS, Value.KING, Resources.Load<Sprite>("Textures/Cards/kd"));
	public static Card KING_SPADES = new Card(Suit.SPADES, Value.KING, Resources.Load<Sprite>("Textures/Cards/ks"));

	public static Card ACE_HEARTS = new Card(Suit.HEARTS, Value.ACE, Resources.Load<Sprite>("Textures/Cards/ah"));
	public static Card ACE_CLUBS = new Card(Suit.CLUBS, Value.ACE, Resources.Load<Sprite>("Textures/Cards/ac"));
	public static Card ACE_DIAMONDS = new Card(Suit.DIAMONDS, Value.ACE, Resources.Load<Sprite>("Textures/Cards/ad"));
	public static Card ACE_SPADES = new Card(Suit.SPADES, Value.ACE, Resources.Load<Sprite>("Textures/Cards/as"));

	public static Sprite CARDBACK = Resources.Load<Sprite>("Textures/Cards/cardback");

	public class Card : System.IComparable<Card> {
		Suit suit;
		Value value;
		Sprite sprite;

		public Card(Suit suit, Value value, Sprite sprite){
			this.suit = suit;
			this.value = value;
			this.sprite = sprite;
		}

		public Sprite getSprite(){
			return sprite;
		}

		public Suit getSuit(){
			return suit;
		}

		public Value getValue(){
			return value;
		}

		public int CompareTo(Card card) {
			return this.value.CompareTo(card.getValue());
		}

	}

	public static List<Card> getCardDeck(){
		List<Card> deck = new List<Card>();
		deck.Add(ACE_CLUBS);
		deck.Add(ACE_DIAMONDS);
		deck.Add(ACE_HEARTS);
		deck.Add(ACE_SPADES);
		deck.Add(TWO_CLUBS);
		deck.Add(TWO_DIAMONDS);
		deck.Add(TWO_HEARTS);
		deck.Add(TWO_SPADES);
		deck.Add(THREE_CLUBS);
		deck.Add(THREE_DIAMONDS);
		deck.Add(THREE_HEARTS);
		deck.Add(THREE_SPADES);
		deck.Add(FOUR_CLUBS);
		deck.Add(FOUR_DIAMONDS);
		deck.Add(FOUR_HEARTS);
		deck.Add(FOUR_SPADES);
		deck.Add(FIVE_CLUBS);
		deck.Add(FIVE_DIAMONDS);
		deck.Add(FIVE_HEARTS);
		deck.Add(FIVE_SPADES);
		deck.Add(SIX_CLUBS);
		deck.Add(SIX_DIAMONDS);
		deck.Add(SIX_HEARTS);
		deck.Add(SIX_SPADES);
		deck.Add(SEVEN_CLUBS);
		deck.Add(SEVEN_DIAMONDS);
		deck.Add(SEVEN_HEARTS);
		deck.Add(SEVEN_SPADES);
		deck.Add(EIGHT_CLUBS);
		deck.Add(EIGHT_DIAMONDS);
		deck.Add(EIGHT_HEARTS);
		deck.Add(EIGHT_SPADES);
		deck.Add(NINE_CLUBS);
		deck.Add(NINE_DIAMONDS);
		deck.Add(NINE_HEARTS);
		deck.Add(NINE_SPADES);
		deck.Add(TEN_CLUBS);
		deck.Add(TEN_DIAMONDS);
		deck.Add(TEN_HEARTS);
		deck.Add(TEN_SPADES);
		deck.Add(JACK_CLUBS);
		deck.Add(JACK_DIAMONDS);
		deck.Add(JACK_HEARTS);
		deck.Add(JACK_SPADES);
		deck.Add(QUEEN_CLUBS);
		deck.Add(QUEEN_DIAMONDS);
		deck.Add(QUEEN_HEARTS);
		deck.Add(QUEEN_SPADES);
		deck.Add(KING_CLUBS);
		deck.Add(KING_DIAMONDS);
		deck.Add(KING_HEARTS);
		deck.Add(KING_SPADES);
		return deck;
	}

	public enum Suit{HEARTS,SPADES,DIAMONDS,CLUBS, UNASSIGNED}
	public enum Value{TWO,THREE,FOUR,FIVE,SIX,SEVEN,EIGHT,NINE,TEN,JACK,QUEEN,KING,ACE}
	public enum Hand{HIGHCARD, PAIR, TWOPAIR, THREE, STRAIGHT, FLUSH, FULLHOUSE, FOUR, STRAIGHTFLUSH}
	public enum handState{
		HAND_STATE_WAITING,
		HAND_STATE_DEALING,
		HAND_STATE_DEAL_BETTING,
		HAND_STATE_FLOP_BETTING,
		HAND_STATE_TURN_BETTING,
		HAND_STATE_RIVER_BETTING,
		HAND_STATE_SHOW_WINNER
	};
	public enum BetType{
		BET_TYPE_RAISE,
		BET_TYPE_CALL,
		BET_TYPE_CHECK,
		BET_TYPE_FOLD
	};

	public static Card getCardByValues(string suit, string value){
		foreach(Card card in getCardDeck()){
			if(card.getSuit().ToString().Equals(suit) && card.getValue().ToString().Equals(value)){
				return card;
			}
		}
		return null;
	}

}