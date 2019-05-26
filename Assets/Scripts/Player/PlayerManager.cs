using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

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

    public BattleController battleController{ get; set; }

    public Sprite backSprite { get; set; }

    public bool auto { get; set; }

    public PlayerManager(){
        this.maxPP = 0;
        this.pp = this.maxPP;
        this.life = 20;
        // this.playFirst = playFirst;
        // this.deck = deck;
        this.hand = new List<GameObject>();
        this.battleZone = new List<GameObject>();
        this.cemetery = new List<GameObject>();
        this.myTurn = false;
        this.auto = false;
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

        if(auto){
            StartCoroutine(this.autoPlay());
            //this.autoPlay();
        }
    }

    public void endMyTurn(){
        this.myTurn = false;
        // this.sleepFollowers();/* 相手ターンに動かせないように */
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
                battleController.displayResult(this.gameObject);
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

    public void shuffleDeck(){
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

    public GameObject getMaxCostCard(int pp){
        /* pp以下で最大の手札のカードを返す、該当するカードがない場合はnull */
        if(this.hand == null){
            return null;// FIXME null返すのはどうなのか
        }

        GameObject maxCostCard = null;
        for (int i = 0; i < getHandNum(); i++) {
            int cost = this.hand[i].GetComponent<Card>().cost;

            if (cost > pp) {
                continue;
            }else if(maxCostCard == null) {
                maxCostCard = this.hand[i];
            }else if (maxCostCard.GetComponent<Card>().cost < cost) {
                maxCostCard = this.hand[i];
            }

        }

        return maxCostCard;
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
        GameObject card = battleController.instantiateCard(addCard, this);
        if(battleController.checkHandBack(this)) {
            card.GetComponent<Card>().changeBackSprite();
            card.GetComponent<Card>().setActiveState(false);
        }

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
        if(auto/* 別のフラグ立てる？ */){
            addCard.GetComponent<Card>().changeMainSprite();
            addCard.GetComponent<Card>().setOpponentCardTag();
        }
        this.battleZone.Add(addCard);
        addCard.GetComponent<Card>().lowStatePosiotion();
        displayZone("battleZone");
    }

    public void removeBattleZone(GameObject removeCard) {
        this.battleZone.Remove(removeCard);
        displayZone("battleZone");
    }

    public void addCemetery(GameObject addCard){
        this.cemetery.Add(addCard);
    }

    //public bool inBattleZone(GameObject targetCard) {
    //    if (this.battleZone.IndexOf(targetCard) > -1) {
    //        return true;
    //    }
    //    else {
    //        return false;
    //    }
    //}

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
        int i = 0;
        foreach(GameObject follower in this.battleZone){
            i++;
            follower.GetComponent<Follower>().enableAttack();
        }
    }

    public void sleepFollowers(){
        // 場のフォロワーを攻撃不可にする
        foreach (GameObject follower in this.battleZone) {
            follower.GetComponent<Follower>().disableAttack();
        }
    }

    //public void changeHandBackSprite(){
    //    foreach (GameObject card in this.hand) {
    //        card.GetComponent<Card>().changeBackSprite();
    //    }
    //}

    public void checkFollowersHealth() {

        /* foreachでは、中で対象のリストの要素を削除できない */
        for (int i = 0; i < this.battleZone.Count; i++){
            GameObject card = this.battleZone[i];
            if (card.GetComponent<Card>() is Follower && ((Follower)card.GetComponent<Card>()).health <= 0) {
                removeBattleZone(card);
                addCemetery(card);
                Destroy(card);
            }
        }

    }

    public IEnumerator autoPlay(){
        
        /* 使えるカードがなくなるまでコストが大きい順に使う */
        while(true){
            
            /* 場が埋まっている時 */
            if(this.battleZone.Count >= 5){
                // TODO スペルは使える
                break;
            }

            GameObject maxCostCard = this.getMaxCostCard(this.pp);
            if(maxCostCard == null){
                break;
            }else{
                yield return battleController.autoUseCard(this.gameObject, maxCostCard.GetComponent<Card>());
            }
        }

        // TODO フォロワーへの攻撃
        foreach(GameObject cardObj in this.battleZone){
            Card card = cardObj.GetComponent<Card>();
            if (card is Follower && ((Follower)card).canAttack == true) {
                yield return battleController.autoAttackOpponentPlayer((Follower)card);
            }
        }

        this.battleController.endTurn();

        yield return null;
    }
}
