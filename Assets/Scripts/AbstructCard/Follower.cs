using UnityEngine;

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
            // 光る
        }else{
            //　光らない
        }
    }

}
