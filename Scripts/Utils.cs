using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Policy;
using UnityEngine;
using System.Threading;

public class Utils
{

    public static readonly int EXIL = -3000;
    public static readonly Vector3 EXILVEC = new Vector3(EXIL,EXIL);
    public static readonly string SERVERURL = "http://172.19.10.243:3000/";

    static IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            //Print server response
            Debug.Log(www.text);
        }
        else
        {
            //Something goes wrong, print the error response
            Debug.Log(www.error);
        }
    }

    /*
     Send a http request (post or get) to a server
         */
    public static string httpSend(string serverUrl, string method, string json)
    {
        //return Server.AnswerReq(serverUrl, method);

        

        var client = new WebClient();
        switch (method.ToLower())
        {
            case "post":
                WWW www;
                Dictionary<string, string> postHeader = new Dictionary<string, string>();
                postHeader.Add("Content-Type", "application/json");
                postHeader.Add("Authorization", "Basic bXktdHJ1c3RlZC1jbGllbnQ6c2VjcmV0");
                // convert json string to byte
                var formData = System.Text.Encoding.UTF8.GetBytes(json);

                www = new WWW(serverUrl, formData, postHeader);
                //StartCoroutine(WaitForRequest(www));
                WaitForRequest(www);
                while (!www.isDone) ;
                return www.text;
            case "get":
                return client.DownloadString(serverUrl);
            default:
                return "wrong method";
        }
        
    }

    // TODO Test this function
    public static void DisplayCards(List<Card> objects, int columns)
    {
        const int startX = 100, startY = 100, d = 300;

        int line = 0;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].CardObject.transform.localPosition = new Vector3(startX + (i % columns) * d, startY + line * d);
            if (i % columns == 0) line++;
        }
    }

    public static void LinkListToObject(List<Card> l, string tag)
    {
        Debug.Log(tag);
        foreach (var card in l)
        {
            Debug.Log(GameObject.FindGameObjectWithTag(tag));
            card.CardObject.transform.SetParent(GameObject.FindGameObjectWithTag(tag).transform);
        }
    }

    public static List<Card> GetCards(int type)
    {
        string serverAns = Utils.httpSend(Utils.SERVERURL + "cards?type=" + type, "GET", "");
        Debug.Log(serverAns);
        var c = JsonUtility.FromJson<Models.CardsContainer>(serverAns);
        List<Card> l = new List<Card>();

        foreach (var cardModel in c.Cards)
        {
            l.Add(new Card(cardModel));    
        }

        return l;
    }
}
