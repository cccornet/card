﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

abstract public class Card : MonoBehaviour {
    public string cardName { get; protected set; }
    public int cost { get; set; }
    protected Sprite sprite;
    protected string text;

    public GameObject owner{ get; set; }
    public BattleController battleController{ get; set; }

    private bool inHandZone;
    //private bool inBattleZone;
    protected bool attackOpponent;
    protected GameObject attackedFollower; /* 衝突判定がCard.csにあるのでとりあえずここ */

    private SpriteRenderer mainSpriteRenderer;
    private Sprite mainSprite;
    public Sprite backSprite{ get; set; }

    protected GameObject costObj;
    protected Text costText;

    protected virtual void Awake(){
        this.mainSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.mainSprite = mainSpriteRenderer.sprite;

        setParameters();

    }

    protected abstract void setParameters();

    public void addEventTrigger(){
        this.gameObject.AddComponent<EventTrigger>();

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

    public abstract void addBattleZoneDrag();

	public void OnBeginDragInHand() {
        
    }

    public void OnDragInHand() {

        // 自分のターンでないと動かせない
        if (!(battleController.myTurn(this.owner))) {
            return;
        }

        if (battleController.enableCard(this.owner, this)) {
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
        if (!(battleController.myTurn(this.owner))) {
            return;
        }

        // ppが不足していると動かせない
        // OnDragInHandと条件が逆なのでどうするか
        if (!(battleController.enableCard(this.owner, this))) {
            return;
        }

        if(this.inHandZone/*手札に戻した時*/){
            battleController.displayZone(this.owner, "hand");
        }else/*場に出す時*/{
            battleController.useCard(this.owner, this);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.tag == "OwnHandZone") {
            // 手札エリアに入っている
            this.inHandZone = true;
            return;
        }

        // FIXME 自プレイヤーと区別する
        if (collision.tag == "Player") {
            // 相手プレイヤーを選択している
            this.attackOpponent = true;
            return;
        }

        if (collision.tag == "OpponentFollower") {
            this.attackedFollower = collision.gameObject;
            return;
        }
	}

    private void OnTriggerExit2D(Collider2D collision){
        
        if (collision.tag == "OwnHandZone") {
            this.inHandZone = false;
            return;
        }

        if (collision.tag == "Player") {
            this.attackOpponent = false;
            return;
        }

        if (collision.gameObject.tag == "OpponentFollower") {
            if (collision.gameObject.tag == "OpponentFollower") {
                this.attackedFollower = null;
                return;
            }
        }
    }

	protected void onEffect(){
        // TODO
    }

    protected void offEffect(){
        // TODO
    }

    public void changeBackSprite(){
        this.mainSpriteRenderer.sprite = backSprite;/* インスタンス化してないとmainSpriteRendererがnullなので注意 */
        // TODO コスト等を非表示にする 子のメソッド呼べるか確認
    }

    public void changeMainSprite() {
        this.mainSpriteRenderer.sprite = mainSprite;/* インスタンス化してないとmainSpriteRendererがnullなので注意 */
    }

    public virtual void lowStatePosiotion() {
        this.costObj.SetActive(false);
    }

    public virtual void setActiveState(bool states){
        this.costObj.SetActive(states);
    }

    public void setOpponentCardTag(){
        this.gameObject.tag = "OpponentFollower";
    }

}