using UnityEngine;

public class Goblin : Follower {

    // DBから持ってきた方がいい？
    private int COST = 1;
    private int ATTACK = 1;
    private int HEALTH = 2;

	void Start () {
        this.cost = COST;
        this.attack = ATTACK;
        this.health = HEALTH;
	}
	
    protected override void fanfare() {
        // 能力なし
    }

    protected override void lastWord() {
        // 能力なし
    }

    protected override void effect () {
		// 能力なし
	}
}
