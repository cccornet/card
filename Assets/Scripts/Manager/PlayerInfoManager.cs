using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour {

    // 自分
    [SerializeField]
    private GameObject deckNumObject;
    [SerializeField]
    private GameObject handNumObject;
    [SerializeField]
    private GameObject cemeteryNumObject;
    [SerializeField]
    private GameObject ppNumObject;
    [SerializeField]
    private GameObject lifeNumObject;

    // 相手
    [SerializeField]
    private GameObject opponentDeckNumObject;
    [SerializeField]
    private GameObject opponentHandNumObject;
    [SerializeField]
    private GameObject opponentCemeteryNumObject;
    [SerializeField]
    private GameObject opponentPpNumObject;
    [SerializeField]
    private GameObject opponentLifeNumObject;

    public GameObject player1 { get; set; }
    public GameObject player2 { get; set; }

	void Start () {
       
	}

	void Update () {
        displayPlayerInfo(this.player1, true);
        displayPlayerInfo(this.player2, false);
	}

    private void displayPlayerInfo(GameObject player, bool own){
        if (player == null) {
            return;
        }
        if(own){
            // deck
            Text deckNumText = deckNumObject.GetComponent<Text>();
            deckNumText.text = "Deck : " + player.GetComponent<PlayerManager>().getDeckNum();
            // hand
            Text handNumText = handNumObject.GetComponent<Text>();
            handNumText.text = "Hand : " + player.GetComponent<PlayerManager>().getHandNum();
            // cemetery
            Text cemeteryNumText = cemeteryNumObject.GetComponent<Text>();
            cemeteryNumText.text = "Cemetery : " + player.GetComponent<PlayerManager>().getCemeteryNum();
            // pp
            Text ppNumText = ppNumObject.GetComponent<Text>();
            ppNumText.text = "PP : " + player.GetComponent<PlayerManager>().pp + " / " + player.GetComponent<PlayerManager>().maxPP;
            // life とりあえずここで
            Text lifeNumText = lifeNumObject.GetComponent<Text>();
            lifeNumText.text = "Life : " + player.GetComponent<PlayerManager>().life;
        }else{
            // deck
            Text opponentDeckNumText = opponentDeckNumObject.GetComponent<Text>();
            opponentDeckNumText.text = "Deck : " + player.GetComponent<PlayerManager>().getDeckNum();
            // hand
            Text opponentHandNumText = opponentHandNumObject.GetComponent<Text>();
            opponentHandNumText.text = "Hand : " + player.GetComponent<PlayerManager>().getHandNum();
            // cemetery
            Text opponentCemeteryNumText = opponentCemeteryNumObject.GetComponent<Text>();
            opponentCemeteryNumText.text = "Cemetery : " + player.GetComponent<PlayerManager>().getCemeteryNum();
            // pp
            Text opponentPpNumText = opponentPpNumObject.GetComponent<Text>();
            opponentPpNumText.text = "PP : " + player.GetComponent<PlayerManager>().pp + " / " + player.GetComponent<PlayerManager>().maxPP;
            // life とりあえずここで
            Text opponentLifeNumText = opponentLifeNumObject.GetComponent<Text>();
            opponentLifeNumText.text = "Life : " + player.GetComponent<PlayerManager>().life;
        }

    }
}
