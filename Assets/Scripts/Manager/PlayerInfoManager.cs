using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour {

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

    public GameObject player1 { get; set; }
    // public GameObject player2;

	void Start () {
       
	}

	void Update () {
        displayPlayerInfo(this.player1);
	}

    private void displayPlayerInfo(GameObject player){
        if (player == null) {
            return;
        }
        // deck
        Text deckNumText = deckNumObject.GetComponent<Text>();
        deckNumText.text = "Deck : " + player.GetComponent<Player>().getDeckNum();
        // hand
        Text handNumText = handNumObject.GetComponent<Text>();
        handNumText.text = "Hand : " + player.GetComponent<Player>().getHandNum();
        // cemetery
        Text cemeteryNumText = cemeteryNumObject.GetComponent<Text>();
        cemeteryNumText.text = "Cemetery : " + player.GetComponent<Player>().getCemeteryNum();
        // pp
        Text ppNumText = ppNumObject.GetComponent<Text>();
        ppNumText.text = "PP : " + player.GetComponent<Player>().pp + " / " + player.GetComponent<Player>().maxPP;
        // life とりあえずここで
        Text lifeNumText = lifeNumObject.GetComponent<Text>();
        lifeNumText.text = "Life : " + player.GetComponent<Player>().life;
    }
}
