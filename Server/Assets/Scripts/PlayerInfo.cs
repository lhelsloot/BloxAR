﻿using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour
{
    public static bool IsSpectator = false;
    public static int Team;

    public static bool HasFullBlock { get; private set; }

    private static GameObject teamObject;

    public void SendInfo(IPlayer player, int? teamId)
    {
        networkView.RPC("SetPlayerInfo", player.NetworkPlayer.NetworkPlayer, teamId ?? player.Team.ID);
    }

	public void SendInfo(IPlayer player){
		networkView.RPC("SetPlayerInfo", player.NetworkPlayer.NetworkPlayer, player.Team.ID);
	}

    [RPC]
    void SetPlayerInfo(int team)
    {
        Team = team;

        GameObject goalStructure = GameObject.Find("GoalStructure");

        foreach (TeamInfoLoader teamInfoLoader in GameObject.Find("Teams").GetComponentsInChildren<TeamInfoLoader>())
        {
            TeamInfo teamInfo = teamInfoLoader.TeamInfo;
            if (teamInfo.IsMine)
            {
                teamObject = teamInfoLoader.gameObject;
                goalStructure.transform.parent = GameObject.Find(teamInfo.ImageTarget).transform;
                goalStructure.transform.localPosition = Vector3.zero;
            }

            else
            {
                GameObject goalClone = GameObject.Instantiate(goalStructure) as GameObject;
                goalClone.transform.parent = GameObject.Find(teamInfo.ImageTarget).transform;
                goalClone.transform.localPosition = Vector3.zero;
            }
        }
    }

    [RPC]
    public void SetHalfBlockColor(Vector3 color)
    {
        GameObject rotatingBlock = GameObject.Find("RotatingBlock");
        rotatingBlock.renderer.material.color = ColorModel.ConvertToUnityColor(color);
    }

    [RPC]
    public void SetBlockHalf()
    {
        HasFullBlock = false;
        GameObject rotatingBlock = GameObject.Find("RotatingBlock");
        GameObject halfBlock = Resources.Load("HalfBlock") as GameObject;
        rotatingBlock.GetComponent<MeshFilter>().mesh = halfBlock.GetComponent<MeshFilter>().mesh;
    }

    [RPC]
    public void SetBlockFull()
    {
        HasFullBlock = true;
        GameObject rotatingBlock = GameObject.Find("RotatingBlock");
        GameObject block = Resources.Load("GoalCube") as GameObject;
        rotatingBlock.GetComponent<MeshFilter>().mesh = block.GetComponent<MeshFilter>().mesh;
    }

    [RPC]
    public void ThrowAwayBlock(NetworkMessageInfo message)
    {
        INetworkPlayer nPlayer = new NetworkPlayerWrapper(message.sender);
        IPlayer player = TeamLoader.TeamManager.GetPlayer(nPlayer);
        player.GiveNewInventoryBlock();
    }
}