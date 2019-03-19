using UnityEngine;

abstract public class Spell : Card {
    protected abstract void effect();

    public override void addBattleZoneDrag(){}
}
