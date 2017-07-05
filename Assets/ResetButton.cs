using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Switch"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main");
        }
    }
}
