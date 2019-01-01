using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int maxPP;
    private int pp;
    private int life;
    public bool playFirst { get; set; }

    // 候補 カード名(String), Card, GameObject
    // カード名が一番軽い コスト検索などが手間
    // Card生成 -> GameObject生成 だとCardを2回インスタンス化してる
    // デッキ全てを初めからGameObjectにするのは重そう
    public List<GameObject> deck { get; set; }// 0を一番上とする
    private List<GameObject> hand;
    private List<GameObject> cemetery;

    public GameObject ownHandZone { get; set; }

    private int MAXHAND = 9;

    private float handCursour;

    public Player(){
        
    }

	private void Start () {
        this.maxPP = 0;
        this.pp = this.maxPP;
        this.life = 20;
        // this.playFirst = playFirst;
        // this.deck = deck;
        this.hand = new List<GameObject>();
        this.cemetery = new List<GameObject>();

        this.handCursour = -2.0f;
	}

    public void initBattle(){
        drowCard(3);
        // TODO マリガン
        if (playFirst == false) {
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
            GameObject drowedCard = this.deck[0];
            this.deck.RemoveAt(0);
            if(this.getHandNum() < MAXHAND){
                this.addHand(drowedCard);
            }else/*手札の最大値を超える*/{
                this.cemetery.Add(drowedCard);
            }

        }
    }

    private void shuffleDeck(){
        var deckNum = this.deck.Count;
        for (int i = 0; i < deckNum; i++) {
            GameObject temp = this.deck[i];
            int randomIndex = Random.Range(0, deckNum);
            this.deck[i] = this.deck[randomIndex];
            this.deck[randomIndex] = temp;
        }
    }

    public bool useCard(GameObject target){
        int targetCost = target.GetComponent<Card>().cost;
        if(targetCost <= this.pp){
            this.pp = this.pp - targetCost;
            return true;
        }else{
            return false;
        }
    }

    public int getDeckNum(){
        return this.deck.Count;
    }

    public int getCemeteryNum(){
        return this.cemetery.Count;
    }

    public int getHandNum() {
        if(this.hand == null){
            return 0;
        }else{
            return this.hand.Count;   
        }
    }

    public void damaged(int num){
        this.life = this.life - num;
        if(life < 1){
            // TODO 勝敗
        }
    }

    public void addHand(GameObject addCard){
        // TODO アニメーション再生

        //GameObject cardPrefab = (GameObject)Resources.Load("Prefabs/Card/" + addCard.cardName);

        // Vector2 pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        GameObject card = Instantiate(addCard, new Vector2(handCursour, -3.0f), Quaternion.identity);
        this.handCursour += 2.0f;
        // card.transform.parent = transform;

        // deckをstringにする場合
        //GameObject newCard = new GameObject("");
        //newCard.AddComponent<Goblin>();
    }

    public void setFirstPlay(){
        // TODO 初回呼び出し時のみ変更可能な実装にする
    }

    public void displayHand(){
        if (this.hand == null) {
            return;
        }else{
            // TODO
        }
    }
}
