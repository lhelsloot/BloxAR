﻿using System;
using System.Collections.Generic;

public class Team : ITeam
{
    private static int nextId = 1;

    private int id;
    public int ID
    {
        get
        {
            return id;
        }
    }

    public int Size
    {
        get
        {
            return players.Count;
        }
    }

    private string name;
    public string Name
    {
        get
        {
            return name;
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

    private BlockTracker tracker;
    public BlockTracker Tracker
    {
        get
        {
            return tracker;
        }
    }

    public float Progress
    {
        get
        {
            return tracker.Progress;
        }
    }

    private LinkedList<IPlayer> players;
    public IEnumerable<IPlayer> Players
    {
        get
        {
            return players;
        }
    }

    public Team(string name, string imageTarget)
    {
        UnityEngine.Color?[,,] goalStructure = new UnityEngine.Color?[3, 3, 3];
        goalStructure[1, 1, 1] = UnityEngine.Color.red;
        goalStructure[1, 2, 1] = UnityEngine.Color.green;

        this.id = nextId++;
        this.name = name;
        this.imageTarget = imageTarget;
        this.tracker = new BlockTracker(this, new NetworkWrapper(), 
            new Structure<UnityEngine.Color?>(goalStructure)
        );
        this.tracker.OnCompletion += () => UnityEngine.Debug.LogError("Progress complete!");
        this.players = new LinkedList<IPlayer>();
    }

    public void AddPlayer(IPlayer player)
    {
        player.Team = this;
        players.AddLast(player);
    }

    public void RemovePlayer(IPlayer player)
    {
        player.Team = null;
        players.Remove(player);
    }
}
