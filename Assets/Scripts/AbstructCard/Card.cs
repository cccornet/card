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
	//private bool inBattleZone;

    protected void addEventTrigger(){
        this.gameObject.AddComponent<EventTrigger>();

        // FIXME 中身を動的に変更することで処理を分岐できる
        this.addHandDrag();
    }

    protected void addHandDrag(){
        
        EventTrigger eventTrigger = this.gameObject.GetComponent<EventTrigger>();
        eventTrigger.triggers.Clear();

        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener(data => this.OnDragInHand() );
        eventTrigger.triggers.Add(dragEntry);

        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener(data => this.OnBeginDragInHand());
        eventTrigger.triggers.Add(beginDragEntry);

        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener(data => this.OnEndDragInHand());
        eventTrigger.triggers.Add(endDragEntry);

    }

	public void OnBeginDragInHand() {
        this.beginPos = this.transform.position;

        if(!(this.own.GetComponent<Player>().inBattleZone(this.gameObject))){
            // TODO 攻撃対象の矢印を出したい
        }

    }

    public void OnDragInHand() {

        // 自分のターンでないと動かせない
        if (!(this.own.GetComponent<Player>().myTurn)) {
            return;
        }

        // FIXME 手札にないと動かせない
        if (!(this.own.GetComponent<Player>().inHand(this.gameObject))) {
            return;
        }

        if (this.own.GetComponent<Player>().enableCard(this)) {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePos.z = 0;/* 2D座標に合わせる */
            this.transform.position = worldMousePos;
        }
        else {
            return;// TODO ポップアップ ppが不足しています
        }
    }

    public void OnEndDragInHand(){

        // 自分のターンでないと動かせない
        if (!(this.own.GetComponent<Player>().myTurn)) {
            return;
        }

        // FIXME 手札にないと動かせない
        if (!(this.own.GetComponent<Player>().inHand(this.gameObject))) {
            return;
        }

        // ppが不足していると動かせない
        if (!(this.own.GetComponent<Player>().enableCard(this))) {
            return;
        }

        if(this.inHandZone/*手札に戻した時*/){
            this.transform.position = this.beginPos;
        }else/*場に出す時*/{
            // TODO スペル

            this.own.GetComponent<Player>().useCard(this);
            this.own.GetComponent<Player>().removeHand(this.gameObject);
            this.own.GetComponent<Player>().addBattleZone(this.gameObject);

            // TODO ドラッグイベント切り替え
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