﻿using UnityEngine;
using System.Collections;

public class ClickEventHandler : MonoBehaviour {

	bool clicked = false;
    bool clickStarted = false;
	bool singleClick = false;
	bool doubleClick = false;
	double timePassed = double.MaxValue;
	double doubleClickDelay = 0.2;

	// Use this for initialization
	void Start () {
	
	}

	public bool SingleClick(){
		return singleClick;
	}

	public bool DoubleClick(){
		return doubleClick;
	}

	// Update is called once per frame
	void Update () {
		//no clicks have been created yet for this frame
		doubleClick = false;
		singleClick = false;

        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            clickStarted = true;
        }

		if (clickStarted && Input.GetMouseButtonUp (0))
        {
            clickStarted = false;

						//initial click handling
						if (!clicked) {
								clicked = true;
								timePassed = Time.time;
						}
			            //it is now officially a double click
			            else {
								clicked = false;
								doubleClick = true;
						}
		}

		if(clicked){
				if((Time.time - timePassed) > doubleClickDelay){
					clicked = false;
					singleClick = true;
					//timePassed = double.MaxValue;
				}
		}



	}
}
