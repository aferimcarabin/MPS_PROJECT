using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerProfile
{

    private static PlayerProfile instance = null;
    public int Id;
    public int GameId;
    public string Name;
    public Hand PlayerHand;
    public bool MyTurnFast;
    public Models.EnemyCards Enemy;
    public int Round;
    private int cnt = 0;
    public bool ClearTable;
    public int Score;
    public bool LastTurn;
    public bool ReadyToPlay;
    public int PlayerID;

    private PlayerProfile()
    {
        string serverAns = Utils.httpSend(Utils.SERVERURL + "startgame", "GET", "");
        Debug.Log(serverAns);
        var player = JsonUtility.FromJson<Models.StartGame>(serverAns);
        cnt = 0;
        Round = 0;
        Score = 0;
        LastTurn = false;
        ClearTable = false;

        Id = player.Playerid % 2 + 1;
        PlayerID = player.Playerid;
        GameId = player.Gameid;
        Name = player.Name;
        ReadyToPlay = false;

        Debug.Log(player.ToString());

        bool isGameReady = false;
        ulong time = 1;

        while (!isGameReady)
        {
            if (time % 2000000 == 0)
            {
                serverAns = Utils.httpSend(Utils.SERVERURL + "gameready?game_id=" + player.Gameid, "GET", "");
                Debug.Log(serverAns);
                var res = JsonUtility.FromJson<Models.Response>(serverAns);
                isGameReady = res.Answer.ToLower() == "yes";
            }
            time++;
        }

    }

    public static PlayerProfile Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerProfile();
            }

            return instance;
        }
    }

    public void GetMoves()
    {
        string str =
            Utils.httpSend(
                Utils.SERVERURL + "moves?game_id=" + PlayerProfile.instance.GameId + "&player_id=" +
                PlayerProfile.instance.PlayerID, "GET", "");
        Enemy = JsonUtility.FromJson<Models.EnemyCards>(str);
        //TODO
        Debug.Log("str = " + str + "\nenemy:" + Enemy);

    }
}
