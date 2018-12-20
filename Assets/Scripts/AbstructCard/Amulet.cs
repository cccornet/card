using UnityEngine;

abstract public class Amulet : Card {
    protected int countdown;

    protected abstract void fanfare();
    protected abstract void lastWord();
    protected abstract void effect();
}
