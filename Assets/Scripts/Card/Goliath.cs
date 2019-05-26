using UnityEngine;

public class Goliath : Follower {

    protected override string CARDNAME { get { return "Goliath"; } }
    protected override int COST { get { return 4; } }
    protected override int ATTACK { get { return 3; } }
    protected override int HEALTH { get { return 4; } }

    protected override void fanfare() {
        // 能力なし
    }

    protected override void lastWord() {
        // 能力なし
    }

    protected override void effect() {
        // 能力なし
    }

}
