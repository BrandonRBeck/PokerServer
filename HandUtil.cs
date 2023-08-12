using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandUtil {

	public static PlayerHand getThisHand(List<Constants.Card> cards){
		cards.Sort();
		
		PlayerHand hand = new PlayerHand();
				
		int numSpades = 0;
		int numClubs = 0;
		int numHearts = 0;
		int numDiamonds = 0;
		
		foreach(Constants.Card card in cards){
			if(card.getSuit() == Constants.Suit.SPADES){
				numSpades++;
			}
			if(card.getSuit() == Constants.Suit.CLUBS){
				numClubs++;
			}
			if(card.getSuit() == Constants.Suit.HEARTS){
				numHearts++;
			}
			if(card.getSuit() == Constants.Suit.DIAMONDS){
				numDiamonds++;
			}
			
		}

		hand = getStraightFlush(cards, numSpades, numClubs, numDiamonds, numHearts);
		if(null != hand){
			return hand;
		}

		hand = getFour(cards);
		if(null != hand){
			return hand;
		}

		hand = getFullHouse(cards);
		if(null != hand){
			return hand;
		}
		hand = getFlush(cards, numSpades, numClubs, numDiamonds, numHearts);
		if(null != hand){
			return hand;
		}
		hand = getStraight(cards);
		if(null != hand){
			return hand;
		}
		hand = getThree(cards);
		if(null != hand){
			return hand;
		}
		hand = getTwoPair(cards);
		if(null != hand){
			return hand;
		}

		hand = getPair(cards);
		if(null != hand){
			return hand;
		}

		hand = getHighCards(cards);
		return hand;
	}

	public static PlayerHand getStraightFlush(List<Constants.Card> cards, int numSpades, int numClubs, int numDiamonds, int numHearts){
		PlayerHand hand = new PlayerHand();
		List<Constants.Card> straightCards = new List<Constants.Card>();
		Constants.Suit flushSuit = Constants.Suit.UNASSIGNED;

		if(numSpades >= 5){
			flushSuit = Constants.Suit.SPADES;
		}
		if(numClubs >= 5){
			flushSuit = Constants.Suit.CLUBS;
		}
		if(numDiamonds >= 5){
			flushSuit = Constants.Suit.DIAMONDS;
		}
		if(numHearts >= 5){
			flushSuit = Constants.Suit.HEARTS;
		}
		if(flushSuit == Constants.Suit.UNASSIGNED){
			return null;
		}


		foreach(Constants.Card card in cards){
			if(card.getSuit() == flushSuit){
				straightCards.Add(card);
			}
		}

		int numConcurrent = 0;
		int lastVal = 0;
		foreach(Constants.Card card in straightCards){
			int cardValue = (int)card.getValue();
			if(numConcurrent == 0){
				lastVal = cardValue;
				numConcurrent++;
			}else{
				if(lastVal + 1 == cardValue){
					numConcurrent++;
				}else{
					numConcurrent = 1;
				}
				lastVal = cardValue;
			}
			if(numConcurrent >=  5){
				hand.setHand(Constants.Hand.STRAIGHTFLUSH);
				hand.setPrimaryValue(cardValue);
			}
		}
		
		if(hand.getHand() != Constants.Hand.STRAIGHTFLUSH){
			return null;
		}
		
		return hand;
	}

	public static PlayerHand getHighCards(List<Constants.Card> cards){
		PlayerHand hand = new PlayerHand();
		cards.Reverse();
		hand.setFirstHigh((int)cards[0].getValue());
		hand.setSecondHigh((int)cards[1].getValue());
		hand.setThirdHigh((int)cards[2].getValue());
		hand.setFourthHigh((int)cards[3].getValue());
		hand.setFifthHigh((int)cards[4].getValue());

		return hand;
	}
	
	public static PlayerHand getFlush(List<Constants.Card> cards, int numSpades, int numClubs, int numDiamonds, int numHearts){
		PlayerHand hand = new PlayerHand();
		List<Constants.Card> flushCards = new List<Constants.Card>(cards);
		flushCards.Reverse();

		if(numSpades >= 5){
			hand = setFlushVariables(flushCards, Constants.Suit.SPADES);
		}
		if(numClubs >= 5){
			hand = setFlushVariables(flushCards, Constants.Suit.CLUBS);
		} 
		if(numDiamonds >= 5){
			hand = setFlushVariables(flushCards, Constants.Suit.DIAMONDS);
		}
		if(numHearts >= 5){
			hand = setFlushVariables(flushCards, Constants.Suit.HEARTS);
		} 
		if(hand.getHand() != Constants.Hand.FLUSH){
			return null;
		}
		return hand;
	}

	public static PlayerHand setFlushVariables(List<Constants.Card> cards, Constants.Suit suit){
		PlayerHand hand = new PlayerHand();
		hand.setHand(Constants.Hand.FLUSH);
		foreach(Constants.Card card in cards){
			if(card.getSuit() == suit){
				if(hand.getFirstHigh() == -1){
					hand.setFirstHigh((int)card.getValue());
					continue;
				}
				if(hand.getSecondHigh() == -1){
					hand.setSecondHigh((int)card.getValue());
					continue;
				}
				if(hand.getThirdHigh() == -1){
					hand.setThirdHigh((int)card.getValue());
					continue;
				}
				if(hand.getFourthHigh() == -1){
					hand.setFourthHigh((int)card.getValue());
					continue;
				}
				if(hand.getFifthHigh() == -1){
					hand.setFifthHigh((int)card.getValue());
				}
			}
		}
		return hand;
	}

	public static PlayerHand getFour(List<Constants.Card> cards){
		PlayerHand hand = new PlayerHand();
		int value = getSetOf(cards, -1, 4);
		if(0 < value){
			hand.setHand(Constants.Hand.FOUR);
			hand.setPrimaryValue(value);

			List<Constants.Card> otherCards = new List<Constants.Card>();
			foreach(Constants.Card card in cards){
				if((int)card.getValue() != value){
					otherCards.Add(card);
				}
			}
			otherCards.Reverse();
			hand.setFirstHigh((int)otherCards[0].getValue());

		}else{
			return null;
		}

		return hand;
	}

	public static PlayerHand getThree(List<Constants.Card> cards){
		PlayerHand hand = new PlayerHand();
		int value = getSetOf(cards, -1, 3);
		if(0 < value){
			hand.setHand(Constants.Hand.THREE);
			hand.setPrimaryValue(value);
			
			List<Constants.Card> otherCards = new List<Constants.Card>();
			foreach(Constants.Card card in cards){
				if((int)card.getValue() != value){
					otherCards.Add(card);
				}
			}
			otherCards.Reverse();
			hand.setFirstHigh((int)otherCards[0].getValue());
			hand.setSecondHigh((int)otherCards[1].getValue());
			
		}else{
			return null;
		}
		
		return hand;
	}

	public static PlayerHand getPair(List<Constants.Card> cards){
		PlayerHand hand = new PlayerHand();
		int value = getSetOf(cards, -1, 2);
		if(0 < value){
			hand.setHand(Constants.Hand.PAIR);
			hand.setPrimaryValue(value);
			
			List<Constants.Card> otherCards = new List<Constants.Card>();
			foreach(Constants.Card card in cards){
				if((int)card.getValue() != value){
					otherCards.Add(card);
				}
			}
			otherCards.Reverse();
			hand.setFirstHigh((int)otherCards[0].getValue());
			hand.setSecondHigh((int)otherCards[1].getValue());
			hand.setThirdHigh((int)otherCards[2].getValue());
			
		}else{
			return null;
		}
		
		return hand;
	}

	public static PlayerHand getTwoPair(List<Constants.Card>cards){
		PlayerHand hand = new PlayerHand();
		int setOf2 = getSetOf(cards, -1, 2);
		if(-1 < setOf2){
			int setOf2_2 = getSetOf(cards, setOf2, 2);
			if(-1 < setOf2_2){
				hand.setHand(Constants.Hand.TWOPAIR);
				hand.setPrimaryValue(setOf2);
				hand.setSecondaryValue(setOf2_2);

				List<Constants.Card> otherCards = new List<Constants.Card>();
				foreach(Constants.Card card in cards){
					if((int)card.getValue() != setOf2 && (int)card.getValue() != setOf2_2){
						otherCards.Add(card);
					}
				}
				otherCards.Reverse();
				hand.setFirstHigh((int)otherCards[0].getValue());
			}
		}
		if(hand.getHand() != Constants.Hand.TWOPAIR){
			return null;
		}
		return hand;
	}

	public static PlayerHand getFullHouse(List<Constants.Card>cards){
		PlayerHand hand = new PlayerHand();
		int setOf3 = getSetOf(cards, -1, 3);
		if(-1 < setOf3){
			int setOf2 = getSetOf(cards, setOf3, 2);
			if(-1 < setOf2){
				hand.setHand(Constants.Hand.FULLHOUSE);
				hand.setPrimaryValue(setOf3);
				hand.setSecondaryValue(setOf2);
			}
		}
		if(hand.getHand() != Constants.Hand.FULLHOUSE){
			return null;
		}
		return hand;
	}
	
	
	public static PlayerHand getStraight(List<Constants.Card> cards){
		
		PlayerHand hand = new PlayerHand();

		int numConcurrent = 0;
		int lastVal = 0;
		foreach(Constants.Card card in cards){
			int cardValue = (int)card.getValue();
			if(numConcurrent == 0){
				lastVal = cardValue;
				numConcurrent++;
			}else{
				if(lastVal == cardValue){
					continue;
				}else if(lastVal + 1 == cardValue){
					numConcurrent++;
				}else{
					numConcurrent = 1;
				}
				lastVal = cardValue;
			}
			if(numConcurrent >=  5){
				hand.setHand(Constants.Hand.STRAIGHT);
				hand.setPrimaryValue(cardValue);
			}
		}

		if(hand.getHand() != Constants.Hand.STRAIGHT){
			return null;
		}

		return hand;
	}

	public static int getSetOf(List<Constants.Card> cards, int exclude, int amount){

		int numTwos = 0;
		int numThrees = 0;
		int numFours = 0;
		int numFives = 0;
		int numSixes = 0;
		int numSevens = 0;
		int numEights = 0;
		int numNines = 0;
		int numTens = 0;
		int numJacks = 0;
		int numQueens = 0;
		int numKings = 0;
		int numAces = 0;
		
		foreach(Constants.Card card in cards){
			if(card.getValue() == Constants.Value.ACE){
				numAces++;
			}
			if(card.getValue() == Constants.Value.KING){
				numKings++;
				continue;
			}
			if(card.getValue() == Constants.Value.QUEEN){
				numQueens++;
				continue;
			}
			if(card.getValue() == Constants.Value.JACK){
				numJacks++;
				continue;
			}
			if(card.getValue() == Constants.Value.TEN){
				numTens++;
				continue;
			}
			if(card.getValue() == Constants.Value.NINE){
				numNines++;
				continue;
			}
			if(card.getValue() == Constants.Value.EIGHT){
				numEights++;
				continue;
			}
			if(card.getValue() == Constants.Value.SEVEN){
				numSevens++;
				continue;
			}
			if(card.getValue() == Constants.Value.SIX){
				numSixes++;
				continue;
			}
			if(card.getValue() == Constants.Value.FIVE){
				numFives++;
				continue;
			}
			if(card.getValue() == Constants.Value.FOUR){
				numFours++;
				continue;
			}
			if(card.getValue() == Constants.Value.THREE){
				numThrees++;
				continue;
			}
			if(card.getValue() == Constants.Value.TWO){
				numTwos++;
				continue;
			}
		}

		if(numAces >= amount && exclude != (int)Constants.Value.ACE){
			return (int)Constants.Value.ACE;
		}
		if(numKings >= amount && exclude != (int)Constants.Value.KING){
			return (int)Constants.Value.KING;
		}
		if(numQueens >= amount && exclude != (int)Constants.Value.QUEEN){
			return (int)Constants.Value.QUEEN;
		}
		if(numJacks >= amount && exclude != (int)Constants.Value.JACK){
			return (int)Constants.Value.JACK;
		}
		if(numTens >= amount && exclude != (int)Constants.Value.TEN){
			return (int)Constants.Value.TEN;
		}
		if( numNines >= amount && exclude != (int)Constants.Value.NINE){
			return (int)Constants.Value.NINE;
		}
		if(numEights >= amount && exclude != (int)Constants.Value.EIGHT){
			return (int)Constants.Value.EIGHT;
		}
		if(numSevens >= amount && exclude != (int)Constants.Value.SEVEN){
			return (int)Constants.Value.SEVEN;
		}
		if(numSixes >= amount && exclude != (int)Constants.Value.SIX){
			return (int)Constants.Value.SIX;
		}
		if(numFives >= amount && exclude != (int)Constants.Value.FIVE){
			return (int)Constants.Value.FIVE;
		}
		if(numFours >= amount && exclude != (int)Constants.Value.FOUR){
			return (int)Constants.Value.FOUR;
		}
		if(numThrees >= amount && exclude != (int)Constants.Value.THREE){
			return (int)Constants.Value.THREE;
		}
		if(numTwos >= amount && exclude != (int)Constants.Value.TWO){
			return (int)Constants.Value.TWO;
		}
		return -1;
	}

	public class PlayerHand : System.IComparable<PlayerHand>{
		Constants.Hand hand;
		int primaryValue;
		int secondaryValue;
		int firstHigh;
		int secondHigh;
		int thirdHigh;
		int fourthHigh;
		int fifthHigh;
		
		public PlayerHand(){
			hand = Constants.Hand.HIGHCARD;
			primaryValue = -1;
			secondaryValue = -1;
			firstHigh = -1;
			secondHigh = -1;
			thirdHigh = -1;
			fourthHigh = -1;
			fifthHigh = -1;
		}
		public int CompareTo(PlayerHand playerHand) {
			if(this.hand == playerHand.getHand()){
				if(this.primaryValue == playerHand.getPrimaryValue()){
					if(this.secondaryValue == playerHand.getSecondaryValue()){
						if(this.firstHigh == playerHand.getFirstHigh()){
							if(this.secondHigh == playerHand.getSecondHigh()){
								if(this.thirdHigh == playerHand.getThirdHigh()){
									if(this.fourthHigh == playerHand.getFourthHigh()){
										if(this.fifthHigh == playerHand.getFifthHigh()){
											return 0;
										}else{
											return this.fifthHigh.CompareTo(playerHand.getFifthHigh());
										}
									}else{
										return this.fourthHigh.CompareTo(playerHand.getFourthHigh());
									}
								}else{
									return this.thirdHigh.CompareTo(playerHand.getThirdHigh());
								}
							}else{
								return this.secondHigh.CompareTo(playerHand.getSecondHigh());
							}
						}else{
							return this.firstHigh.CompareTo(playerHand.getFirstHigh());
						}
					}else{
						return this.secondaryValue.CompareTo(playerHand.getSecondaryValue());
					}
				}else{
					return this.primaryValue.CompareTo(playerHand.getPrimaryValue());
				}
			}else{
				return this.hand.CompareTo(playerHand.getHand());
			}
		}
		
		public Constants.Hand getHand(){
			return hand;
		}
		public void setHand(Constants.Hand hand){
			this.hand = hand;
		}
		public int getPrimaryValue (){
			return primaryValue;
		}
		public void setPrimaryValue(int primaryValue){
			this.primaryValue = primaryValue;
		}
		public int getSecondaryValue (){
			return secondaryValue;
		}
		public void setSecondaryValue(int secondaryValue){
			this.secondaryValue = secondaryValue;
		}
		public int getFirstHigh (){
			return firstHigh;
		}
		public void setFirstHigh(int firstHigh){
			this.firstHigh = firstHigh;
		}
		public int getSecondHigh (){
			return secondHigh;
		}
		public void setSecondHigh(int secondHigh){
			this.secondHigh = secondHigh;
		}
		public int getThirdHigh (){
			return thirdHigh;
		}
		public void setThirdHigh(int thirdHigh){
			this.thirdHigh = thirdHigh;
		}
		public int getFourthHigh (){
			return fourthHigh;
		}
		public void setFourthHigh(int fourthHigh){
			this.fourthHigh = fourthHigh;
		}
		public int getFifthHigh (){
			return fifthHigh;
		}
		public void setFifthHigh(int fifthHigh){
			this.fifthHigh = fifthHigh;
		}
	}

}
