﻿using UnityEngine;
using UnityEngine.UI;

public class OfficeRutineManager : MonoBehaviour
{

    public delegate void OnNewDaySignature();
    public event OnNewDaySignature OnNewDay;
	public Button onAirButton;
	public GameObject panelEndGame;
	public Text endGameText;

    private static OfficeRutineManager instance = null;
    public static OfficeRutineManager Instance { get { return instance; } }

    [SerializeField] private uint startingDay = 1;

    private uint currentDay = 1;
    public uint CurrentDay { get { return currentDay; } }

    private uint daysElapsed = 0;
    public uint DaysElapsed { get { return daysElapsed; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentDay = startingDay;
        daysElapsed = 0;

        NewsManager.Instance.RefreshAvailableNews();
    }


	public void OnEndGame (StatsManager.EndGameType endGameInfo)
    {
		Debug.LogWarning (endGameInfo.ToString());
		onAirButton.interactable = false;
		panelEndGame.SetActive (true);
		endGameText.text = endGameInfo.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AdvanceDay();
        }
        if (Input.GetKey(KeyCode.F8))
        {
            AdvanceDay();
        }
    }

	public void OnAirButton()
	{
        AdvanceDay();
	}

    public void AdvanceDay()
    {
        currentDay++;
        daysElapsed++;

		OnNewDay ();
    }
}
