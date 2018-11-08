using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

class ContainerCards
{
    public List<Card> list;
}

public class Hand
{
    public static readonly int NumberOfCards = 25;

    public readonly int PlayerId;
    public List<Card> Cards;

    public Hand(int playerId)
    {
        PlayerId = playerId;
        Cards = new List<Card>();
    }

    // get cards from server
    public void GetCards()
    {
        Cards = JsonUtility.FromJson<ContainerCards>(Utils.httpSend(Utils.SERVERURL, "GET", "")).list;
    }

    public Card GetCard(GameObject gameObject)
    {
        foreach (var card in Cards)
        {
            if (card.CardObject == gameObject)
            {
                return card;
            }
        }

        return null;
    }
}
