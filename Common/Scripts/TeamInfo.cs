﻿using UnityEngine;

public class TeamInfo
{
    private IGameObject gameObject;

    private int id;
    public int ID
    {
        get
        {
            return id;
        }
    }

    private string teamName;
    public string Name
    {
        get
        {
            return teamName;
        }
    }

    private string imageTarget;
    public string ImageTarget
    {
        get
        {
            return imageTarget;
        }
    }

    public TeamInfo(IGameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    /// <summary>
    /// When the Team object to which this TeamInfo component belongs is instantiated, it should 
    /// be grouped under the Teams object.
    /// </summary>
    public void OnNetworkInstantiate()
    {
        gameObject.transform.parent = gameObject.Find("Teams").transform;
    }

    public void SetInfo(int id, string name, string imageTarget)
    {
        gameObject.networkView.RPC("SetTeamInfo", RPCMode.AllBuffered, id, name, imageTarget);
    }


    /// <summary>
    /// Determine if the team represented by this TeamInfo object belongs to the local player. 
    /// This requires a GameObject named Player to be present with a PlayerInfo script.
    /// </summary>
    public bool IsMine()
    {
        IGameObject player = gameObject.Find("Player");
        if (player != null)
        {
            PlayerInfo info = player.GetComponent<PlayerInfo>();
            if (info != null)
            {
                return info.Team == ID;
            }
        }

        return false;
    }

    public void RPC_SetTeamInfo(int id, string name, string imageTarget)
    {
        this.id = id;
        this.teamName = name;
        this.imageTarget = imageTarget;

        IGameObject target = gameObject.Find(imageTarget);
        target.transform.parent = gameObject.transform;
    }
}