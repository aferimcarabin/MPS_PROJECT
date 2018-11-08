using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    Card card;
    public new Text Name;
    public Image ImageImage;
    public Text Attack;
    public Text Defense;
    public Text CountryText;
    public Text ClubText;

    // Use this for initialization
    void Start()
    {
        Name.text = card.Name;
        CountryText.text = card.Country;
        ClubText.text = card.Club;

        ImageImage.sprite = card.Image;

        Attack.text = card.Attack.ToString();
        Defense.text = card.Defense.ToString();
    }

}