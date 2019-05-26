using UnityEngine;

public class ExileOfMercenaries : Follower {

    protected override string CARDNAME { get { return "ExileOfMercenaries"; } }
    protected override int COST { get { return 3; } }
    protected override int ATTACK { get { return 3; } }
    protected override int HEALTH { get { return 2; } }

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
