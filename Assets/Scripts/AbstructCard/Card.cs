using UnityEngine;
using UnityEngine.EventSystems;

abstract public class Card : MonoBehaviour {
    public string cardName { get; protected set; }
    public int cost { get; set; }
    protected Sprite sprite;
    protected string text;

    private Vector3 beginPos;

    protected GameObject own;
    // 静的に決める方法考え中
    // protected GameObject battleController;

    private bool inHandZone;
    private bool inBattleZone;

    private bool canDrag;

	public void OnBeginDrag() {
        this.beginPos = this.transform.position;

        if(this.inBattleZone){
            // TODO 攻撃対象の矢印を出したい
        }

        // 手札にないと動かせない
        if(this.inHandZone){
            canDrag = true;
        }else{
            canDrag = false;
        }
    }

    public void OnDrag() {
        // 自分のターンでないと動かせない

        if(!(this.own.GetComponent<Player>().myTurn)){
            return;
        }

        if (!(canDrag)) {
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

        if (!(canDrag)) {
            return;
        }

        if(this.inHandZone/*手札に戻した時*/){
            this.transform.position = this.beginPos;
        }else/*場に出す時*/{
            this.own.GetComponent<Player>().useCard(this);
            this.own.GetComponent<Player>().removeHand(this.gameObject);
            this.own.GetComponent<Player>().addBattleZone(this.gameObject);

            this.inHandZone = true;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "ownHandZone") {
            // 手札エリアに入っている
            this.inHandZone = true;
        }

	}

    private void OnTriggerExit2D(Collider2D collision){

        if (collision.tag == "ownHandZone") {
            this.inHandZone = false;
        }

    }

}