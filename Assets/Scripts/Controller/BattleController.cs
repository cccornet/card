using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    private GameObject player1;
    // private GameObject player2;

	void Start () {
        List<Card> deck1 = new List<Card>();
        //List<Card> deck2 = new List<Card>();

        // TODO deck 読み込み
        deck1.Add(new Goblin());

        bool firstPlay = Random.Range(0, 2) == 0 ? true : false;

        // this.player1 = makePlayer("Player1", deck1, firstPlay);
        this.player1 = makePlayer("Player1", deck1, true);
        // コンポーネントも別の変数に入れとく？
        this.player1.GetComponent<Player>().initBattle();

        //this.player1 = new Player(deck1, firstPlay);
        //this.player2 = new Player(deck2, !(firstPlay));

	}
	
	void Update () {
        setPlayerInfo(player1);
        //setPlayerInfo(player2);
	}

    private void setPlayerInfo(GameObject player){
        // TODO set
        // player.getDeckNum();
        // player.getCemeteryNum();
        // player.getHandNum();
    }

    private GameObject makePlayer(string playerName, List<Card> deck, bool firstPlay){
        GameObject player = new GameObject("Player1");
        player.AddComponent<Player>();
        player.GetComponent<Player>().deck = deck;
        player.GetComponent<Player>().playFirst = firstPlay;
        return player;
    }
}
