using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour
{

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void OnMouseDown()
    {
        Application.LoadLevel("main");
    }

}
