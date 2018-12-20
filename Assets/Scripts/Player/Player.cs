using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int pp;
    private int life;
    private List<Card> deck; // 0を一番上とする
    private bool playFirst;

    public Player(List<Card> deck,bool playFirst){
        this.pp = 0;
        this.life = 20;
        this.deck = deck;
        this.playFirst = playFirst;
        // this.hand
    }

	private void Start () {
		
	}
	
    private void initTurn(){
        incrementPP();
        drowCard(1);
    }

    private void incrementPP(){
        if(this.pp == 10){
            return;
        }else{
            this.pp++;
        }
    }

    private void drowCard(int n){
        for (int i = 0; i < n; i++){
            Card drowedCard = this.deck[0];
            this.deck.RemoveAt(0);
            // hand.Add(drowCard);   
        }
    }

    private void shuffleDeck(){
        
    }
}
