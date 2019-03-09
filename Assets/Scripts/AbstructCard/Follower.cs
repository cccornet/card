using UnityEngine;
using UnityEngine.EventSystems;

abstract public class Follower : Card {
    protected int attack;
    protected int health;

    public bool canAttack { get; protected set; }

    protected abstract void fanfare();
    protected abstract void lastWord();
    protected abstract void effect();

	//private void addHandDrag(){
	//    base.addHandDrag();
	//}

	//private void Update() {
	//    changeCanAttackEffect();
	//}

    // FIXME 保留
	//protected override void Start() {
	//	base.Start();
	//}

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

    protected override void addBattleZoneDrag() {
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
        // FIXME
        base.beginPos = this.transform.position;
    }

    // FIXME In or On
    public void OnDragInBattleZone() {
        // 自分のターンでないと動かせない
        if (!(this.own.GetComponent<PlayerManager>().myTurn)) {
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
        if (!(this.own.GetComponent<PlayerManager>().myTurn)) {
            return;
        }

        // 攻撃可能でないと動かせない
        if (!(this.canAttack)) {
            return;
        }

        if(this.attackOpponent/*攻撃対象が選択された時*/){
            // GetComponent<BattleController>().attackOpponentPlayer(this.attack);
            // ゲームオーバー判定
            this.canAttack = false;

            // TODO フォロワーの場合
            // 破壊処理
        }else/* 選択されなかった時 */{
            this.transform.position = this.beginPos;
        }

    }
}
