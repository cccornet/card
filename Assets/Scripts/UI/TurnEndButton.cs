using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndButton : MonoBehaviour {

    private GameObject battleController;

	void Start() {
        this.battleController = GameObject.FindGameObjectsWithTag("BattleController")[0];
	}

	public void OnClick() {
        
        this.battleController.GetComponent<BattleController>().endTurn();

        // TODO setActive 親要素から
    }
}
