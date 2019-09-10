using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Button TuichuButton;
	void Start () {
		TuichuButton.onClick.AddListener(Quit1);
	}
	

    private void Quit1()
    {
       
        Application.Quit();
    }
}
