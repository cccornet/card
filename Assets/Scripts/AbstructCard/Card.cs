using UnityEngine;
using UnityEngine.EventSystems;

abstract public class Card : MonoBehaviour {
    public string cardName { get; protected set; }
    public int cost { get; set; }
    protected Sprite sprite;
    protected string text;

    private Vector3 beginPos;

    protected GameObject own;

    private bool inHandZone;

	public void OnBeginDrag() {
        this.beginPos = this.transform.position;
    }

    public void OnDrag() {
        // 自分のターンでないと動かせない
        if(!(this.own.GetComponent<Player>().myTurn)){
            return;
        }

        // 手札にないと動かせない
        // FIXME ドラッグの動きが見えない, 手札以外でも動かせる
        if (!(this.inHandZone)) {
            return;
        }

        if(this.own.GetComponent<Player>().enableCard(this)){
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePos.z = 0;/* 2D座標に合わせる */
            this.transform.position = worldMousePos;
        }else{
            return;// TODO ポップアップ ppが不足しています
        }
    }

    public void OnEndDrag(){
        if(this.inHandZone){
            this.transform.position = this.beginPos;
        }else{
            this.own.GetComponent<Player>().removeHand(this.gameObject);
            this.own.GetComponent<Player>().addBattleZone(this.gameObject);
         // TODO pp減らす
        }
    }

	private void OnCollisionEnter2D(Collision2D collision) {
        // FIXME 当たり判定がうまくいっていない

        // とりあえずレイヤー判定で
        this.inHandZone = true;
	}

	private void OnCollisionExit(Collision collision) {
        this.inHandZone = false;
	}
}