using UnityEngine;
using UnityEngine.EventSystems;

abstract public class Card : MonoBehaviour {
    public string cardName { get; protected set; }
    public int cost { get; set; }
    protected Sprite sprite;
    protected string text;

    private Vector3 beginPos;

    protected GameObject own;

	public void OnBeginDrag() {
        this.beginPos = this.transform.position;

        // cost チェック
        // OnDrag()を呼ばない
    }

    public void OnDrag() {
        if(this.own.GetComponent<Player>().enableCard(this) /*  and inMyTurn */){
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePos.z = 0;/* 2D座標に合わせる */
            this.transform.position = worldMousePos;
        }else{
            return;// TODO ポップアップ ppが不足しています
        }
    }

    public void OnEndDrag(){
        //if(in ownHandZone){
        //    this.transform.position = this.beginPos;
        //}else{
        // fieldに出す handから削除
        //}
        this.transform.position = this.beginPos;
    }
}