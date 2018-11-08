using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour {

    public Text scoreText1;
    public Text scoreText2;

    public void modifyEnemyScore(int score,int playerID)
    {
        if (playerID == 1)
            scoreText1.text = "Score:" + score;
        else
            scoreText2.text = "Score:" + score;
    }

    public int calculateScore(int playerID)
    {
        var cards = PlayerProfile.Instance.PlayerHand.Cards;
        int score = 0;
        foreach (Card card in cards)
        {
            if (card.CardObject.transform.parent.name != "Hand" && card.CardObject.transform.parent.name != "Hand (1)")
            {
                if (card.Type == 2 || card.Type == 3)
                {
                    score += cards.Count * (card.Attack + card.Defense);
                }
                else
                {
                    score += card.Attack;
                    score += card.Defense;
                }
            }
        }

        if (playerID == 1)
            scoreText1.text = "Score:" + score;
        else
            scoreText2.text = "Score:" + score;

        return score;
    }
}
