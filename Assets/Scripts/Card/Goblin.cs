using UnityEngine;

public class Goblin : Follower {

    // DBから持ってきた方がいい？
    protected override string CARDNAME { get{ return "Goblin"; } }
    protected override int COST { get { return 1; } }
    protected override int ATTACK { get { return 1; } }
    protected override int HEALTH { get { return 2; } }

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
