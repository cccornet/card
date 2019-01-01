using UnityEngine;

public class Goblin : Follower {

    // DBから持ってきた方がいい？
    private string CARDNAME = "Goblin";
    private int COST = 1;
    private int ATTACK = 1;
    private int HEALTH = 2;

    public Goblin(){
        
    }

	void Start () {
        this.cardName = this.CARDNAME;
        this.cost = this.COST;
        this.attack = this.ATTACK;
        this.health = this.HEALTH;
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
