using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaiqi1 : MonoBehaviour
{

    public GameObject kaiqiwuti;
    public bool kongzhi;
    // Update is called once per frame
    void Update () {
	    if (kaiqiwuti.activeSelf&& kongzhi == false)
	    {
	        kongzhi = true;
        }
	    else if (kaiqiwuti.activeSelf==false&& kongzhi)
        {
            kongzhi = false;        
        }
	}
}
