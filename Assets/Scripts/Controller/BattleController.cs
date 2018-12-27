using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    private Player player1;
    private Player player2;

	void Start () {
        List<Card> deck1 = new List<Card>();
        //List<Card> deck2 = new List<Card>();
        // TODO deck 読み込み
        bool firstPlay = Random.Range(0, 2) == 0 ? true : false;
        this.player1 = new Player(deck1, firstPlay);
        //this.player2 = new Player(deck2, !(firstPlay));
        player1.displayHand();
	}
	
	void Update () {
        setPlayerInfo(player1);
        //setPlayerInfo(player2);

        //if(player1.changedHand){
        //    player1.displayHand();
        //}
	}

    private void setPlayerInfo(Player player){
        // TODO set
        // player.getDeckNum();
        // player.getCemeteryNum();
        // player.getHandNum();
    }
}
