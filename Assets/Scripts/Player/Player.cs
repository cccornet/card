using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int maxPP { get; private set; } // ppブースト系はメソッド作成
    public int pp { get; private set; } // 回復はメソッド作成
    public int life { get; set; }
    public bool playFirst { get; set; }

    public List<GameObject> deck { get; set; }// 0を一番上とする
    private List<GameObject> hand;
    private List<GameObject> battleZone;
    private List<GameObject> cemetery;

    public GameObject ownHandZone { get; set; }
    public GameObject ownBattleZone { get; set; }

    private int MAXHAND = 9;

    public bool myTurn { get; private set; }

    public Player(){
        this.maxPP = 0;
        this.pp = this.maxPP;
        this.life = 20;
        // this.playFirst = playFirst;
        // this.deck = deck;
        this.hand = new List<GameObject>();
        this.battleZone = new List<GameObject>();
        this.cemetery = new List<GameObject>();
        this.myTurn = false;
    }

	private void Start () {
        
	}

    public void initBattle(){
        drowCard(3);
        // TODO マリガン
        if (playFirst == false) {
            drowCard(1);
        }
    }
	
    public void startMyTurn(){
        incrementPP();
        this.pp = this.maxPP;
        drowCard(1);
        this.myTurn = true;
        wakeUpFollowers();
    }

    public void endMyTurn(){
        this.myTurn = false;
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
            
            if(this.deck.Count == 0){
                // TODO 敗北
                return;
            }

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

    public bool useCard(Card targetCard){
        if(enableCard(targetCard)){
            this.pp = this.pp - targetCard.cost;
            return true;
        }else{
            return false;
        }
    }

    public bool enableCard(Card targetCard){
        
        if (targetCard.cost > this.pp) {
            return false;
        }else if(targetCard is Follower && this.battleZone.Count >= 5) {
            // 場が埋まっている時、フォロワーはプレイできない
            // TODO アミュレット
            return false;
        }else{
            return true; 
        }

    }

    public int getDeckNum(){
        return this.deck.Count;
    }

    public int getCemeteryNum(){
        return this.cemetery.Count;
    }

    public int getBattleFiledNum() {
        return this.battleZone.Count;
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

        // 子要素にする必要ない？
        // card.transform.parent = this.ownHandZone.transform;
        // TODO デッキ以外の挙動
        GameObject card = Instantiate(addCard, new Vector2(0, this.ownHandZone.transform.position.y), Quaternion.identity);
        this.hand.Add(card);
        displayZone("hand");
    }

    public void removeHand(GameObject removeCard){
        this.hand.Remove(removeCard);
        displayZone("hand");
    }

    public bool inHand(GameObject targetCard){
        if(this.hand.IndexOf(targetCard) > -1){
            return true;
        }else{
            return false;
        }
    }

    public void addBattleZone(GameObject addCard) {
        // TODO アニメーション再生

        this.battleZone.Add(addCard);
        displayZone("battleZone");
    }

    public void removeBattleZone(GameObject removeCard) {
        this.battleZone.Remove(removeCard);
        displayZone("battleZone");
    }

    public bool inBattleZone(GameObject targetCard) {
        if (this.battleZone.IndexOf(targetCard) > -1) {
            return true;
        }
        else {
            return false;
        }
    }

    public void setFirstPlay(){
        // TODO 初回呼び出し時のみ変更可能な実装にする
    }

    public void displayZone(string zone){
        List<GameObject> target = new List<GameObject>();
        GameObject targetZone = null;
        if(zone == "hand"){
            target = this.hand;
            targetZone = this.ownHandZone;
        }else if(zone == "battleZone"){
            target = this.battleZone;
            targetZone = this.ownBattleZone;
        }

        if (target == null){
            return;
        }else {
            int halfTargetNum = target.Count / 2;

            // FIXME 変数名
            float cardSize = targetZone.transform.localScale.x / 6;
            float center = targetZone.transform.position.x;
            // 相手のカードサイズ変更する？

            if (target.Count % 2 == 0 /*偶数*/) {
                float targetCursor = center + -cardSize / 2 + -cardSize * (halfTargetNum - 1);
                for (int i = 0; i < target.Count; i++) {
                    target[i].transform.position = new Vector3(targetCursor, targetZone.transform.position.y, 0);
                    targetCursor += cardSize;
                }
            }
            else/*奇数*/{
                float targetCursor = center + -cardSize * halfTargetNum;
                for (int i = 0; i < target.Count; i++) {
                    target[i].transform.position = new Vector3(targetCursor, targetZone.transform.position.y, 0);
                    targetCursor += cardSize;
                }
            }

        }

    }

    public void wakeUpFollowers(){
        // 場のフォロワーを攻撃可能にする
        foreach(GameObject follower in this.battleZone){
            follower.GetComponent<Follower>().enableAttack();
        }
    }

}
