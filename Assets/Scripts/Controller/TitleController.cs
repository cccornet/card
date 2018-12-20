using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

	//void Start () {
		
	//}
	
	void Update () {
        TouchInfo info = AppUtil.GetTouch();
        if (info == TouchInfo.Began) {
            Debug.Log("Touch !");
            SceneManager.LoadScene("Home");
        }
	}
}
