using UnityEngine;
using UnityEngine.EventSystems;

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

    private SpriteRenderer mainSpriteRenderer;
    private Sprite mainSprite;
    [SerializeField]
    private Sprite backSprite;

    protected virtual void Start(){
        this.mainSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.mainSprite = mainSpriteRenderer.sprite;

        setParameters();

        //addEventTrigger();

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

    protected abstract void addBattleZoneDrag();

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
            // TODO スペル、アミュレット

            battleController.useCard(this.owner, this);
            battleController.removeHand(this.owner, this.gameObject);
            battleController.addBattleZone(this.owner, this.gameObject);

            addBattleZoneDrag();
        }
    }

	private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "ownHandZone") {
            // 手札エリアに入っている
            this.inHandZone = true;
            return;
        }

        // TODO
        if (collision.tag == "Player") {
            // 相手プレイヤーを選択している
            this.attackOpponent = true;
            return;
        }

	}

    private void OnTriggerExit2D(Collider2D collision){
        
        if (collision.tag == "ownHandZone") {
            this.inHandZone = false;
            return;
        }

        if (collision.tag == "Player") {
            this.attackOpponent = false;
            return;
        }

    }

    protected void onEffect(){
        // TODO
    }

    protected void offEffect(){
        // TODO
    }

    public void changeBackSprite(){
        mainSpriteRenderer.sprite = backSprite;
    }

    public void changeMainSprite() {
        mainSpriteRenderer.sprite = mainSprite;
    }
}