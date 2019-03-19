using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

abstract public class Follower : Card {
    protected int attack;
    public int health { get; set; }

    public bool canAttack { get; protected set; }

    protected abstract string CARDNAME{ get; }
    protected abstract int COST { get; }
    protected abstract int ATTACK { get; }
    protected abstract int HEALTH { get; }

    protected abstract void fanfare();
    protected abstract void lastWord();
    protected abstract void effect();

    protected GameObject attackObj;
    protected GameObject healthObj;
    protected Text attackText;
    protected Text healthText;

	//private void addHandDrag(){
	//    base.addHandDrag();
	//}

	//private void Update() {
	//    changeCanAttackEffect();
	//}

    protected override void setParameters() {
        this.cardName = this.CARDNAME;
        this.cost = this.COST;
        this.attack = this.ATTACK;
        this.health = this.HEALTH;

        this.costObj = this.transform.Find("cost").gameObject;
        this.attackObj = this.transform.Find("attack").gameObject;
        this.healthObj = this.transform.Find("health").gameObject;

        this.costText = this.costObj.transform.Find("Canvas").transform.Find("Text").gameObject.GetComponent<Text>();
        this.attackText = this.attackObj.transform.Find("Canvas").transform.Find("Text").gameObject.GetComponent<Text>();
        this.healthText = this.healthObj.transform.Find("Canvas").transform.Find("Text").gameObject.GetComponent<Text>();

        this.costText.text = this.cost.ToString();
        this.attackText.text = this.attack.ToString();
        this.healthText.text = this.health.ToString();

        this.canAttack = false;
    }

	public void enableAttack(){
        this.canAttack = true;
        this.changeCanAttackEffect();
    }

    public void disableAttack() {
        this.canAttack = false;
        this.changeCanAttackEffect();
    }

    private void changeCanAttackEffect(){
        if(canAttack){
            base.onEffect();
        }else{
            base.offEffect();
        }
    }

    public override void addBattleZoneDrag() {
        EventTrigger eventTrigger = this.gameObject.GetComponent<EventTrigger>();
        eventTrigger.triggers.Clear();

        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener(data => this.OnDragInBattleZone());
        eventTrigger.triggers.Add(dragEntry);

        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener(data => this.OnBeginDragInBattleZone());
        eventTrigger.triggers.Add(beginDragEntry);

        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener(data => this.OnEndDragInBattleZone());
        eventTrigger.triggers.Add(endDragEntry);
    }

    // FIXME In or On
    public void OnBeginDragInBattleZone() {
        
    }

    // FIXME In or On
    public void OnDragInBattleZone() {
        // 自分のターンでないと動かせない
        if (!(battleController.myTurn(this.owner))) {
            return;
        }

        if (!(this.canAttack)) {
            return;
        }

        // FIXME 矢印を出すように変更
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0;/* 2D座標に合わせる */
        this.transform.position = worldMousePos;
    }

    // FIXME In or On
    public void OnEndDragInBattleZone() {
        // 自分のターンでないと動かせない
        if (!(battleController.myTurn(this.owner))) {
            return;
        }

        // 攻撃可能でないと動かせない
        if (!(this.canAttack)) {
            return;
        }

        if(this.attackOpponent/*相手プレイヤーが選択された時*/){
            this.battleController.GetComponent<BattleController>().attackOpponentPlayer(this.attack);
            this.canAttack = false;
        }else if (false/* 相手フォロワーが選択された時 */){
            // 1.互いにダメージを与えあう
            // 2.破壊処理

            // 複数選択不可にする方法
            // とりあえず複数だったら弾くか
        }else/* 選択されなかった時 */{
            
        }

        // FIXME
        battleController.GetComponent<BattleController>().displayZone(this.owner, "battleZone");
    }

    public override void lowStatePosiotion(){
        base.lowStatePosiotion();

        this.attackObj.SetActive(true);
        this.healthObj.SetActive(true);

        Vector3 attackLocalPos = this.attackObj.transform.localPosition;/* positionはアクセス修飾詞の設定上、直接変更できない */
        attackLocalPos.y = -4.0f;
        this.attackObj.transform.localPosition = attackLocalPos;

        Vector3 healthLocalPos = this.healthObj.transform.localPosition;/* positionはアクセス修飾詞の設定上、直接変更できない */
        healthLocalPos.x = 3.0f;
        healthLocalPos.y = -4.0f;
        this.healthObj.transform.localPosition = healthLocalPos;

    }

    public override void setActiveState(bool states) {
        base.setActiveState(states);

        this.attackObj.SetActive(states);
        this.healthObj.SetActive(states);
    }
}
