﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {

    [SerializeField]
    private GameObject playerInfoManager;

    private GameObject player1;
    private GameObject player2;

    private GameObject nowPlayer;

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject resultUIPrefab;
    private GameObject resultUIInstance;

    // FIXME
    [SerializeField]
    private Sprite backSprite;

	void Start () {
        // エリア生成
        GameObject ownHandZone = (GameObject)Resources.Load("Prefabs/Zone/OwnHandZone");
        Instantiate(ownHandZone, new Vector2(0, -4), Quaternion.identity);
        GameObject ownBattleZone = (GameObject)Resources.Load("Prefabs/Zone/OwnBattleZone");
        Instantiate(ownBattleZone, new Vector2(0, -1), Quaternion.identity);
        GameObject opponentHandZone = (GameObject)Resources.Load("Prefabs/Zone/OpponentHandZone");
        Instantiate(opponentHandZone, new Vector2(-4, 4), Quaternion.identity);
        GameObject opponentBattleZone = (GameObject)Resources.Load("Prefabs/Zone/OpponentBattleZone");
        Instantiate(opponentBattleZone, new Vector2(0, 1), Quaternion.identity);

        // TODO スクリプト
        Instantiate((GameObject)Resources.Load("Prefabs/Player/Player"), new Vector2(0, 4f), Quaternion.identity);

        List<GameObject> deck1 = new List<GameObject>();
        List<GameObject> deck2 = new List<GameObject>();

        // TODO deck 読み込み
        int DECKMAX = 10;
        for (int i = 0; i < DECKMAX; i++){
            deck1.Add((GameObject)Resources.Load("Prefabs/Card/" + "Goblin"));
            deck2.Add((GameObject)Resources.Load("Prefabs/Card/" + "Goblin"));
            // deckエリアの子要素？
        }

        // 先攻後攻決め
        // bool firstPlay = Random.Range(0, 2) == 0 ? true : false;

        this.player1 = makePlayer("Player1", deck1, this.backSprite, true, ownHandZone, ownBattleZone, false);
        this.player2 = makePlayer("Player2", deck2, this.backSprite, false, opponentHandZone, opponentBattleZone, true);
        // コンポーネントも別の変数に入れとく？

        this.nowPlayer = player1;

        this.playerInfoManager.GetComponent<PlayerInfoManager>().player1 = this.player1;
        this.playerInfoManager.GetComponent<PlayerInfoManager>().player2 = this.player2;

        this.player1.GetComponent<PlayerManager>().initBattle();
        this.player2.GetComponent<PlayerManager>().initBattle();

        this.player1.GetComponent<PlayerManager>().startMyTurn();

	}
	
	void Update () {
        
	}

    private GameObject makePlayer(string playerName, List<GameObject> deck, Sprite cardBack,bool firstPlay, GameObject ownHandZone, GameObject ownBattleZone, bool auto){
        // FIXME
        GameObject player = new GameObject(playerName);
        player.AddComponent<PlayerManager>();
        PlayerManager playerManager = player.GetComponent<PlayerManager>();

        playerManager.deck = deck;
        playerManager.backSprite = cardBack;
        playerManager.playFirst = firstPlay;
        playerManager.ownHandZone = ownHandZone;
        playerManager.ownBattleZone = ownBattleZone;
        playerManager.battleController = this;

        if(auto){
            playerManager.auto = auto;
        }

        return player;
    }


    // 直接Playerの関数呼ぶよりこっちの方が良さげ もう少しうまくアクセスしたい
    public bool myTurn(GameObject player){
        return player.GetComponent<PlayerManager>().myTurn;
    }

    public bool enableCard(GameObject player, Card targetCard) {
        return player.GetComponent<PlayerManager>().enableCard(targetCard);
    }

    public void useCard(GameObject player, Card targetCard) {
        player.GetComponent<PlayerManager>().useCard(targetCard);
        player.GetComponent<PlayerManager>().removeHand(targetCard.gameObject);

        if(targetCard is Follower){
            player.GetComponent<PlayerManager>().addBattleZone(targetCard.gameObject);
            targetCard.addBattleZoneDrag();
        }else{
            // TODO スペル、アミュレット
        }
    }

    public void removeHand(GameObject player, GameObject removeCard){
        player.GetComponent<PlayerManager>().removeHand(removeCard);
    }

    public void addBattleZone(GameObject player, GameObject addCard){
        player.GetComponent<PlayerManager>().addBattleZone(addCard);
    }

    public void displayZone(GameObject player, string zone){
        player.GetComponent<PlayerManager>().displayZone(zone);
    }

    public void endTurn(){
        Debug.Log("Turn end");
        nowPlayer.GetComponent<PlayerManager>().endMyTurn();

        if(nowPlayer == this.player2){
            nowPlayer = this.player1;
            this.player1.GetComponent<PlayerManager>().startMyTurn();
        }else{
            nowPlayer = this.player2;
            this.player2.GetComponent<PlayerManager>().startMyTurn();
        }

    }

    public void attackOpponentPlayer(int attack){
        this.player2.GetComponent<PlayerManager>().life -= attack;
        checkGameOver(player2);
    }

    public void checkGameOver(GameObject player){

        // デッキ枚数による勝敗は引くタイミングなので、ここではタイミングがずれる
        // this.player1.GetComponent<PlayerManager>().getDeckNum() < 0
        if(this.player1.GetComponent<PlayerManager>().life < 1){
            displayResult(player1);
        }else if(this.player2.GetComponent<PlayerManager>().life < 1){
            displayResult(player2);
        }

    }

    public void displayResult(GameObject player){
        string message = "You ";
        if(player == player1){
            message += "Lose";
        }else{
            message += "Win!";
        }
        this.resultUIInstance = Instantiate(resultUIPrefab) as GameObject;
        this.resultUIInstance.transform.SetParent(this.canvas.transform, false);
        Text UIText = this.resultUIInstance.transform.Find("Text").gameObject.GetComponent<Text>();
        UIText.text = message;

        //Time.timeScale = 0f; // Update()は止まらない
        // メモ
        //void Update() {
        //    if (Mathf.Approximately(Time.timeScale, 0f)) {
        //        return;
        //    }
        //}
    }

    public GameObject instantiateCard(GameObject card, PlayerManager player) {
        GameObject newCard = Instantiate(card, new Vector2(0, player.ownHandZone.transform.position.y), Quaternion.identity);
        newCard.GetComponent<Card>().battleController = this;
        newCard.GetComponent <Card>().owner = player.gameObject;
        newCard.GetComponent<Card>().backSprite = player.backSprite;
        newCard.GetComponent<Card>().addEventTrigger(); /* 先にownerを登録する必要がある */
        return newCard;
    }

    public bool checkHandBack(PlayerManager player){
        if(player.gameObject == this.player1){
            return false;
        }else{
            return true;
        }
    }
}
