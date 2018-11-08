using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server
{
    private static int reqNumber = 0;

    public static object AnswerObj(string path, string type)
    {
        reqNumber++;
        Debug.Log("reqNumber = " + reqNumber);
        Debug.Log("Request:"+path);

        if (path.Contains("startgame"))
        {
            return new Models.StartGame(1, 2, "Name");
        }

        if (path.Contains("gameready"))
        {
            return new Models.Response("yes");
        }

        if (path.Contains("moves"))
        {
            Models.EnemyCards enemyCards = new Models.EnemyCards();
            
            enemyCards.Moves.Add(new Models.Move(new Move(1 * reqNumber, 2)));
            enemyCards.Moves.Add(new Models.Move(new Move(2 * reqNumber, 2)));
            enemyCards.Moves.Add(new Models.Move(new Move(3 * reqNumber, 3)));
            enemyCards.Moves.Add(new Models.Move(new Move(4 * reqNumber, 4)));

            enemyCards.Cards.Add(new Models.CardModel(1 * reqNumber,"Nume1", 1 * reqNumber, 1 * reqNumber,1));
            enemyCards.Cards.Add(new Models.CardModel(2 * reqNumber, "Nume2", 2 * reqNumber, 1 * reqNumber, 1));
            enemyCards.Cards.Add(new Models.CardModel(3 * reqNumber, "Nume3", 3 * reqNumber, 1 * reqNumber, 1));
            enemyCards.Cards.Add(new Models.CardModel(4 * reqNumber, "Nume4", 4 * reqNumber, 1 * reqNumber, 1));

            return enemyCards;
        }

        if (path.Contains("myturn"))
        {
            for (int i = 0; i < 1000000; i++)
            {
                i += 0;
            }

            if (reqNumber % 5 == 0)
            {
                return new Models.Response("yes");
            }
            else
            {
                return new Models.Response("yes");
            }
        }

        if (path.Contains("whowins"))
        {
            return new Models.Response("0");
        }

        if (path.Contains("cards"))
        {
            Models.CardsContainer cardsContainer = new Models.CardsContainer();

            for (int i = 1; i <= 30; i++)
            {
                cardsContainer.Cards.Add(new Models.CardModel(i*reqNumber,"Name" + i,i,i,1));
            }

            return cardsContainer;
        }

        return null;
    }

    public static string AnswerReq(string path, string type)
    {
        string ans =  JsonUtility.ToJson(AnswerObj(path, type));
        Debug.Log("ans = " + ans);
        return ans;
    }
}
