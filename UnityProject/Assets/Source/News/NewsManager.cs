﻿using System.Collections.Generic;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    public bool debugLog = true;

    public delegate void AvailableNewsRefreshedSignature();
    public event AvailableNewsRefreshedSignature OnAvailableNewsRefreshed;

    public delegate void NewsChosenSignature(News chosenNews);
    public event NewsChosenSignature OnNewsChosen;

    private static NewsManager instance = null;
    public static NewsManager Instance { get { return instance; } }

    private List<News> newsPool = new List<News>();
    public List<News> NewsPool { get { return NewsPool; } }

    [SerializeField] private uint newsCopies = 2;

    private List<News> availableNews = new List<News>();
    public List<News> AvailableNews { get { return availableNews; } }

    private Queue<News> chosenNews = new Queue<News>();
    public Queue<News> ChosenNews { get { return ChosenNews; } }
    
    private int availableNewsCount = 3;

	public void SetTime( int news, float time )
	{
		availableNews [news].NewsValues.timeAssigned = time;
	}

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OfficeRutineManager.Instance.OnNewDay += OnNewDay;

        for (int i = 0; i < newsCopies; i++)
        {
            newsPool.AddRange(new List<News>(NewsDefinitions.GetNewsDatabase()));
        }
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChoiceFromAvailableNews(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChoiceFromAvailableNews(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChoiceFromAvailableNews(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChoiceFromAvailableNews(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChoiceFromAvailableNews(4);
        }
        */
    }

    public bool RefreshAvailableNews()
    {
        if (newsPool.Count < availableNewsCount)
        {
            Debug.LogWarning("not enough news in the news pool");
            return false;
        }

        News[] newAvailableNews = new News[availableNewsCount];

        //Select random news from news database
        for (int i = 0; i < newAvailableNews.Length; i++)
        {
            News randomNew = newsPool[Random.Range(0, newsPool.Count)];
            newAvailableNews[i] = randomNew;
            newsPool.Remove(randomNew);
        }

        //Assign the results.
        availableNews = new List<News>(newAvailableNews);

        if(debugLog) Debug.Log("Available news refreshed.");

        PrintAvailableNews();

        if (OnAvailableNewsRefreshed != null)
        {
            OnAvailableNewsRefreshed();
        }

        return true;
    }

    private void OnNewDay()
    {
        RefreshAvailableNews();
    }

    private void ChoiceFromAvailableNews(int index)
    {
        if (index >= availableNews.Count)
        {
            if (debugLog) Debug.Log("No news exists at index " + index + ".");
            return;
        }

        ChoiseFromAvailableNews(availableNews[index]);
    }

    private void ChoiseFromAvailableNews(News news)
    {
        if (!availableNews.Contains(news))
        {
            if (debugLog) Debug.Log("\"" + news.Title + "\" new is not available.");
            return;
        }

        availableNews.Remove(news);
        if (debugLog) Debug.Log("\"" + news.Title + "\" removed from available news.");
        chosenNews.Enqueue(news);
        if (debugLog) Debug.Log("\"" + news.Title + "\" enqueued to chosen news.");

        if (OnNewsChosen != null)
        {
            OnNewsChosen(news);
        }

        PrintAvailableNews();
    }

    public void PrintAvailableNews()
    {
        if (debugLog) Debug.Log("Available news: ");
        for (int i = 0; i < availableNews.Count; i++)
        {
            if (debugLog) Debug.Log(availableNews[i].Title + " (" + i + ")");
        }
    }
}
