﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class intro_scene : MonoBehaviour {

	public GameObject continueText;

	// Use this for initialization
	void Start () {
		StartCoroutine (DisplayContinue ());
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Return))
        {
            AudioManager.Instance.GameStart();
            StatsManager.deltaPorcentajeAudienciaOficialismo = 0.0f;
            StatsManager.deltaPorcentajePoblacionOficialismo = 0.0f;
            StatsManager.deltaRating = 0.0f;

            SceneManager.LoadScene ("Test_01", LoadSceneMode.Single);
		}
	}
	
	IEnumerator DisplayContinue () {
		yield return new WaitForSeconds (10);
		continueText.SetActive (true);
	}
}
