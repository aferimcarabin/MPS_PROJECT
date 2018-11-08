using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Move
{

    public readonly int CardId;
    public readonly int Position;

    public Move(int cardId, int position)
    {
        CardId = cardId;
        Position = position;
        Debug.Log("Id: " + CardId + "  pos " + Position);
    }

    public override string ToString()
    {
        return "(CardId:" + CardId + "  ,Pos:" + Position + ")";
    }
}

[Serializable]
public class Moves
{
    public int GameId;
    public int PlayerId;
    public int Score;
    public List<Move> MovesList;

    private static Moves instance = null;

    private Moves()
    {
        MovesList = new List<Move>();
    }

    public static Moves Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Moves();
            }

            return instance;
        }
    }

    public override string ToString()
    {
        string str = "";

        foreach (var move in MovesList)
        {
            str += move.ToString() + "\n";
        }

        return str;
    }

    public void Refresh()
    {
        GameId = PlayerProfile.Instance.GameId;
        PlayerId = PlayerProfile.Instance.PlayerID;
        Score = PlayerProfile.Instance.Score;
    }
}
