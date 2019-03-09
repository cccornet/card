using UnityEngine;

abstract public class Spell : Card {
    protected abstract void effect();

    protected override void addBattleZoneDrag(){}
}
