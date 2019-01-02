using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    private GameObject player1;
    // private GameObject player2;

	void Start () {
        // エリア生成
        GameObject ownHandZone = (GameObject)Resources.Load("Prefabs/Zone/OwnHandZone");

        List<GameObject> deck1 = new List<GameObject>();
        //List<Card> deck2 = new List<Card>();

        // TODO deck 読み込み
        int DECKMAX = 10;
        for (int i = 0; i < DECKMAX; i++){
            deck1.Add((GameObject)Resources.Load("Prefabs/Card/" + "Goblin"));
            // deckエリアの子要素？
        }

        bool firstPlay = Random.Range(0, 2) == 0 ? true : false;

        this.player1 = makePlayer("Player1", deck1, true, ownHandZone);
        // コンポーネントも別の変数に入れとく？
        this.player1.GetComponent<Player>().initBattle();

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

    private GameObject makePlayer(string playerName, List<GameObject> deck, bool firstPlay, GameObject ownHandZone){
        GameObject player = new GameObject("Player1");
        player.AddComponent<Player>();
        player.GetComponent<Player>().deck = deck;
        player.GetComponent<Player>().playFirst = firstPlay;
        player.GetComponent<Player>().ownHandZone = ownHandZone;
        return player;
    }
}
