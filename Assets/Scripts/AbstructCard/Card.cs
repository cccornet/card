using UnityEngine;
using UnityEngine.EventSystems;

abstract public class Card : MonoBehaviour {
    public string cardName { get; protected set; }
    public int cost { get; set; }
    protected Sprite sprite;
    protected string text;

    private Vector3 beginPos;

    public void OnBeginDrag() {
        this.beginPos = this.transform.position;

        // cost チェック
        // OnDrag()を呼ばない
    }

    public void OnDrag() {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0;/* 2D座標に合わせる */
        this.transform.position = worldMousePos;
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