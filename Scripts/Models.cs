using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models {

    [Serializable]
    public class StartGame
    {
        public int Gameid;
        public int Playerid;
        public string Name;

        public StartGame(int gameid, int playerid, string name)
        {
            Gameid = gameid;
            Playerid = playerid;
            Name = name;
        }

        public override string ToString()
        {
            return "GameID:" + Gameid + "   Playerid:" + Playerid + "  Name:" + Name;
        }
    }

    public class Response
    {
        public string Answer;

        public Response()
        {
            Answer = "";
        }

        public Response(string answer)
        {
            Answer = answer;
        }
    }

    [Serializable]
    public class CardsContainer
    {
        public List<CardModel> Cards;

        public CardsContainer()
        {
            Cards = new List<CardModel>();
        }

        public override string ToString()
        {
            string str = "";

            foreach (var c in Cards)
            {
                str += c.ToString() + "\n";
            }

            return str;
        }
    }

    [Serializable]
    public class CardModel
    {
        public int id;
        public string name;
        public int attack;
        public int defense;
        public string country;
        public string club;
        public string description;
        public int card_type;

        public CardModel(int id, string name, int attack, int defense, int card_type)
        {
            this.id = id;
            this.name = name;
            this.attack = attack;
            this.defense = defense;
            this.card_type = card_type;
        }

        public override string ToString()
        {
            return "Id: " + id + "  Name:" + name + "  Attack:" + attack;
        }
    }

    [Serializable]
    public class Move
    {
        public int CardId;
        public int Position;

        public Move(global::Move mov)
        {
            CardId = mov.CardId;
            Position = mov.Position;
        }

        public override string ToString()
        {
            return "( " + CardId + ", " + Position + " )";
        }
    }

    [Serializable]
    public class EnemyCards
    {
        public int GameId;
        public int PlayerId;
        public int Score;
        public List<Move> Moves;
        public List<CardModel> Cards;

        public EnemyCards()
        {
            Moves = new List<Move>();
            Cards = new List<CardModel>();
        }

        public override string ToString()
        {
            string str = "";

            foreach (var mov in Moves)
            {
                str += mov;
            }

            foreach (var card in Cards)
            {
                str += card;
            }

            return str;
        }
    }

    [Serializable]
    public class ContainerMoves
    {
        public int GameId;
        public int PlayerId;
        public int Score;
        public List<Move> Moves;

        public ContainerMoves(Moves moves)
        {
            Moves = new List<Move>();
            GameId = moves.GameId;
            PlayerId = moves.PlayerId;
            Score = moves.Score;
            foreach (var mov in moves.MovesList)
            {
                if (mov.CardId != 0)
                {
                    Moves.Add(new Move(mov));
                }    
            }
        }

        public override string ToString()
        {
            string str = "";

            foreach (var move in Moves)
            {
                str += move.ToString();
            }

            return "GameId:" + GameId + "  PlayerId:" + PlayerId + "   L:" + str;
        }
    }
}
