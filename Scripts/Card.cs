using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.Animations;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{

    public int Id;
    public GameObject CardObject;
    public readonly string Name;
    public Sprite Image;
    public int Attack;
    public int Defense;
    public string Country;
    public string Club;
    public string Description;
    public int Type;

    public Card(Models.CardModel cardModel)
    {
        Name = cardModel.name;
        Id = cardModel.id;
        Attack = cardModel.attack;
        Defense = cardModel.defense;
        Country = cardModel.country;
        Club = cardModel.club;
        Description = cardModel.description;
        Type = cardModel.card_type;
    }

    public Card(string name, Sprite image, int attack, int defense, string country, string club, string description, int type)
    {
        Id = 0;
        Name = name;
        Image = image;
        Attack = attack;
        Defense = defense;
        Country = country;
        Club = club;
        Description = description;
        Type = type;
    }

    public Card(int id, string name, int attack, int defense, string country, string club, string description, int type, GameObject gameObject, string dropZoneTag)
    {
        Id = id;
        CardObject = gameObject;
        CardObject.transform.SetParent(GameObject.FindGameObjectWithTag(dropZoneTag).transform);
        CardObject.transform.localPosition = Utils.EXILVEC;
        CardObject.transform.localScale = Vector3.one;// CardObject.transform.localScale;
        Name = name;
        Attack = attack;
        Defense = defense;
        Country = country;
        Club = club;
        Description = description;
        Type = type;
    }

    public override string ToString()
    {
        return "Id:" + Id + "  Name:" + Name + "  Attack:" + Attack + "   Defense:" + Defense;
    }
}
