using UnityEngine;

abstract public class Follower : Card {
    protected int attack;
    protected int health;

    public bool canAttack { get; set; }

    protected abstract void fanfare();
    protected abstract void lastWord();
    protected abstract void effect();
}
