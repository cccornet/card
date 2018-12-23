using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int maxPP;
    private int pp;
    private int life;
    private bool playFirst;
    private List<Card> deck; // 0を一番上とする
    private List<Card> hand;
    private List<Card> cemetery;

    public Player(List<Card> deck,bool playFirst){
        this.maxPP = 0;
        this.pp = this.maxPP;
        this.life = 20;
        this.playFirst = playFirst;
        this.deck = deck;
        this.hand = new List<Card>();
        this.cemetery = new List<Card>();
    }

	private void Start () {
        drowCard(3);
        // TODO マリガン
        if(playFirst == false){
            drowCard(1);
        }
	}
	
    private void initTurn(){
        incrementPP();
        this.pp = this.maxPP;
        drowCard(1);
    }

    private void incrementPP(){
        if(this.maxPP == 10){
            return;
        }else{
            this.maxPP++;
        }
    }

    private void drowCard(int n){
        for (int i = 0; i < n; i++){
            Card drowedCard = this.deck[0];
            this.deck.RemoveAt(0);
            this.hand.Add(drowedCard);
        }
    }

    private void shuffleDeck(){
        var deckNum = this.deck.Count;
        for (int i = 0; i < deckNum; i++) {
            Card temp = this.deck[i];
            int randomIndex = Random.Range(0, deckNum);
            this.deck[i] = this.deck[randomIndex];
            this.deck[randomIndex] = temp;
        }
    }

    public bool useCard(Card target){
        if(target.cost <= this.pp){
            this.pp = this.pp - target.cost;
            return true;
        }else{
            return false;
        }
    }
}
