using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    public List<IEndGameObserver> observers = new List<IEndGameObserver>();

    private CinemachineFreeLook followCamera;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RigisterPlayer(CharacterStats player)
    {
        this.playerStats = player;

        // ÒòÇÐ»»³¡¾°»á¶ªÊ§¸úËæ£¬ÒªÉèÖÃÉãÏñÍ·¸úËæ
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(2);
            followCamera.LookAt = playerStats.transform.GetChild(2);

        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        var list = FindObjectsOfType<TransitionDestination>();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].destinationTag == TransitionDestination.DestinationTag.Enter)
            {
                return list[i].transform;
            }
        }
        return null;
    }
}
