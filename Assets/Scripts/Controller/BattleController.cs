﻿using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    [SerializeField]
    private GameObject playerInfoManager;

    private GameObject player1;
    private GameObject player2;

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

        this.player1 = makePlayer("Player1", deck1, true, ownHandZone, ownBattleZone);
        this.player2 = makePlayer("Player2", deck2, false, opponentHandZone, opponentBattleZone);
        // コンポーネントも別の変数に入れとく？

        this.playerInfoManager.GetComponent<PlayerInfoManager>().player1 = this.player1;
        this.playerInfoManager.GetComponent<PlayerInfoManager>().player2 = this.player2;

        this.player1.GetComponent<PlayerManager>().initBattle();
        this.player2.GetComponent<PlayerManager>().initBattle();

        // FIXME Start()のタイミングに注意
        //this.player2.GetComponent<PlayerManager>().changeHandBackSprite();

        this.player1.GetComponent<PlayerManager>().startMyTurn();

	}
	
	void Update () {
        
	}

    private GameObject makePlayer(string playerName, List<GameObject> deck, bool firstPlay, GameObject ownHandZone, GameObject ownBattleZone){
        // FIXME
        GameObject player = new GameObject(playerName);
        player.AddComponent<PlayerManager>();

        player.GetComponent<PlayerManager>().deck = deck;
        player.GetComponent<PlayerManager>().playFirst = firstPlay;
        player.GetComponent<PlayerManager>().ownHandZone = ownHandZone;
        player.GetComponent<PlayerManager>().ownBattleZone = ownBattleZone;
        return player;
    }


    // 直接Playerの関数呼ぶよりこっちの方が良さげ もう少しうまくアクセスしたい
    public bool myTurn(){
        return this.player1.GetComponent<PlayerManager>().myTurn;
    }

    public bool enableCard(Card targetCard) {
        return this.player1.GetComponent<PlayerManager>().enableCard(targetCard);
    }

    public void useCard(Card targetCard) {
        this.player1.GetComponent<PlayerManager>().useCard(targetCard);
    }

    public void removeHand(GameObject removeCard){
        this.player1.GetComponent<PlayerManager>().removeHand(removeCard);
    }

    public void addBattleZone(GameObject addCard){
        this.player1.GetComponent<PlayerManager>().addBattleZone(addCard);
    }

    public void endTurn(){
        Debug.Log("Turn end");
        this.player1.GetComponent<PlayerManager>().endMyTurn();

        // TODO player入れ替え
        this.player1.GetComponent<PlayerManager>().startMyTurn();
    }

    public void attackOpponentPlayer(int attack){
        this.player2.GetComponent<PlayerManager>().life -= attack;
    }
}
