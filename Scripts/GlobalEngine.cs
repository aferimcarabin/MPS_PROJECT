using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalEngine : MonoBehaviour
{
    private PlayerProfile playerProfile;
    private Hand hand;
    private List<Card> select1List;
    private List<Card> select2List;
    private List<Card> select3List;

    public GameObject ZonesGameObject;
    public GameObject Select1GameObject;
    public GameObject Select2GameObject;
    public GameObject Select3GameObject;
    public Text FinishText;
    public Text RoundsScoreText;

    private string ScoreText;
    private int roundsWonBy1;
    private int roundsWonBy2;
    private int enemyScore;
    private int enemyRound;
    private int myRound;
    public UnityEngine.UI.Text score;
    public GameObject scoreUpdater;
    private ulong frames = 0;
    private int _round;
    //private bool myTurn = false;

    // Use this for initialization
    void Start () {
		playerProfile = PlayerProfile.Instance;
        hand = new Hand(playerProfile.Id);
	    playerProfile.PlayerHand = hand;
        _round = 0;

        roundsWonBy1 = 0;
        roundsWonBy2 = 0;

	    select1List = Utils.GetCards(1);
	    select2List = Utils.GetCards(2);
	    select3List = Utils.GetCards(3);

        InstanAll(select1List,1);
        InstanAll(select2List,2);
        InstanAll(select3List,3);

        GoToSelect1();

        //if (PlayerProfile.Instance.MyTurn) ;
    }

    private void InstanAll(List<Card> l, int n)
    {
        foreach (var card in l)
        {
            card.CardObject = (GameObject)Instantiate(Resources.Load(@"Prefabs\Card1"));
            card.CardObject.transform.SetParent(GameObject.FindGameObjectWithTag("DropZone" + n).transform);
            card.CardObject.transform.localPosition = Utils.EXILVEC;
            card.CardObject.transform.localScale = Vector3.one;
            card.CardObject.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite =
                (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Pictures/Cards/" + Mathf.RoundToInt(UnityEngine.Random.Range(1, 25)) + ".jpg", typeof(Sprite)); ;
            card.CardObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = card.Name.Length > 12 ? card.Name.Substring(0, 12) : card.Name;
            card.CardObject.transform.GetChild(5).GetComponent<UnityEngine.UI.Text>().text = card.Attack.ToString();
            card.CardObject.transform.GetChild(6).GetComponent<UnityEngine.UI.Text>().text = card.Defense.ToString();
            card.CardObject.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = card.Country;
        }
    }

    private void insCard(Card card, Models.Move move)
    {
        string zone = "";
        switch (move.Position)
        {
            case 1:
                zone = "Goalkeeper";
                break;
            case 2:
                zone = "Defense";
                break;
            case 3:
                zone = "Midfield";
                break;
            case 4:
                zone = "Forwards";
                break;
            default:
                zone = "AmPlMare";
                break;
        }

        zone += PlayerProfile.Instance.Id;

        card.CardObject = (GameObject)Instantiate(Resources.Load(@"Prefabs\Card1"));
        card.CardObject.transform.SetParent(GameObject.FindGameObjectWithTag(zone).transform);
        card.CardObject.transform.localPosition = Utils.EXILVEC;
        card.CardObject.transform.localScale = Vector3.one;
        card.CardObject.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite =
            (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Pictures/Cards/" + Mathf.RoundToInt(UnityEngine.Random.Range(1, 25)) + ".jpg", typeof(Sprite)); ;
        card.CardObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = card.Name.Length > 12 ? card.Name.Substring(0, 12) : card.Name;
        card.CardObject.transform.GetChild(5).GetComponent<UnityEngine.UI.Text>().text = card.Attack.ToString();
        card.CardObject.transform.GetChild(6).GetComponent<UnityEngine.UI.Text>().text = card.Defense.ToString();
        card.CardObject.transform.GetChild(3).GetComponent<UnityEngine.UI.Text>().text = card.Country;
    }

    private void GetMoves()
    {
        playerProfile.ClearTable = false;
        playerProfile.Round++;
        int score = 0;
        // ClearTable();
        PlayerProfile.Instance.GetMoves();

        for (int i = 0; i < PlayerProfile.Instance.Enemy.Moves.Count; i++)
        {
            foreach (var mov in PlayerProfile.Instance.Enemy.Moves)
            {
                if (mov.CardId == PlayerProfile.Instance.Enemy.Cards[i].id)
                {
                    insCard(new Card(PlayerProfile.Instance.Enemy.Cards[i]), mov);
                    score += PlayerProfile.Instance.Enemy.Cards[i].attack + playerProfile.Enemy.Cards[i].defense;
                }
            }
        }

        PlayerProfile.Instance.Enemy.Moves.Clear();
        PlayerProfile.Instance.Enemy.Cards.Clear();

        scoreUpdater.GetComponent<ScoreUpdater>().modifyEnemyScore(score,playerProfile.Id % 2 + 1);

        enemyScore = score;
    }

    // Update is called once per frame
    void Update ()
    {
        frames++;
        
	    if (frames % 120 == 0 && PlayerProfile.Instance.ReadyToPlay)
	    {
	        string answer = Utils.httpSend(Utils.SERVERURL + "myturn?game_id=" + playerProfile.GameId + "&player_id=" + playerProfile.PlayerID,
	            "GET", "");
          
            Debug.Log("rasp = " + answer);
	        playerProfile.MyTurnFast = answer.ToLower().Contains("yes") || ! playerProfile.ReadyToPlay;
            // need to get moves
	        if (playerProfile.LastTurn != playerProfile.MyTurnFast)
	        {
	            _round++;
	            string str = Utils.httpSend(Utils.SERVERURL + "whowins?game_id=" + PlayerProfile.Instance.GameId, "GET", "");
	            if (str.Contains("1"))
	            {
	                roundsWonBy1++;
	                //TODO load scene where player 1 wins
	                FinishText.text = " Congrats player1 !!!!";
	            }

	            if (str.Contains("2"))
	            {
	                roundsWonBy2++;
	                FinishText.text = "Congrats player2 !!!!";
	                //TODO load scnee where player 2 wins
	            }

            }

            if (!playerProfile.LastTurn && playerProfile.MyTurnFast)
	        {
	            ClearTable();
                GetMoves();
	           
                // calculate enemy score
                //scoreUpdater.GetComponent<ScoreUpdater>().calculateScore(PlayerProfile.Instance.Id % 2 + 1);
            }
            // clear table
	        if (_round > 0 && _round % 3 == 0)
	        {
	            //ClearTable();
                
	            if (PlayerProfile.Instance.Score < enemyScore)
	            {
	                enemyScore = 0;
	                enemyRound++;
	            }
	            else
	            {
	                myRound++;
	            }
	            _round = 0;
	            Debug.Log("AAAAAAAAAAAAAAAAScore: " + PlayerProfile.Instance.Score + "  " + myRound);
            }

	        playerProfile.LastTurn = playerProfile.MyTurnFast;

	       
	           RoundsScoreText.text = roundsWonBy1 + " - " + roundsWonBy2;
	        
	    }
        
        /*
	    if (!myTurn && frames % 100 == 0)
	    {
	        if (true)
	        {
	            playerProfile.ClearTable = false;
	            playerProfile.Round++;
	            ClearTable();
	            PlayerProfile.Instance.GetMoves();
	            //if (PlayerProfile.Instance.MyTurn) ;

	            for (int i = 0; i < PlayerProfile.Instance.Enemy.Moves.Count; i++)
	            {
	                foreach (var mov in PlayerProfile.Instance.Enemy.Moves)
	                {
	                    if (mov.CardId == PlayerProfile.Instance.Enemy.Cards[i].id)
	                    {
	                        insCard(new Card(PlayerProfile.Instance.Enemy.Cards[i]), mov);
	                    }
	                }
	            }

	            PlayerProfile.Instance.Enemy.Moves.Clear();
	            PlayerProfile.Instance.Enemy.Cards.Clear();
	        }

	        ScoreText = roundsWonBy2 + " - " + roundsWonBy1;
	        score.text = ScoreText;

	        PlayerProfile.Instance.Score =
	            scoreUpdater.GetComponent<ScoreUpdater>().calculateScore(PlayerProfile.Instance.Id);
	    }
        */
        if(roundsWonBy1 == 2 || roundsWonBy2 == 2)
            SceneManager.LoadScene("Final", LoadSceneMode.Single);
    }

    public void ChooseCardsSelect1()
    {
        if(ChooseCardsSelectX(11,select1List))
            GoToSelect2();
    }

    public void ChooseCardsSelect2()
    {
        if(ChooseCardsSelectX(5, select2List))
            GoToSelect3();
    }

    public void ChooseCardsSelect3()
    {
        if(ChooseCardsSelectX(1, select3List))
            GoToZones();
    }

    public bool ChooseCardsSelectX(uint numberOfCards, List<Card> cards)
    {
        uint goodCards = 0;
        int playerTer = 1;

        if (PlayerProfile.Instance.Id == 2)
        {
            playerTer *= -1;
        }

        foreach (var card in cards)
        {
            if (card.CardObject.transform.localPosition.x < -140)
            {
                goodCards++;
            }
        }

        if (goodCards != numberOfCards)
        {
            Debug.Log("You must choose " + numberOfCards + " cards");
            return false;
        }

        foreach (var card in cards)
        {
            if (card.CardObject.transform.localPosition.x > -140)
            {
                card.CardObject.transform.localPosition = Utils.EXILVEC;
                Destroy(card.CardObject);
                //cards.Remove(card);
            }
            else
            {
                hand.Cards.Add(card);
            }
        }

        return true;
    }

    private void GoToNowWhere()
    {
        ZonesGameObject.SetActive(false);
        Select1GameObject.SetActive(false);
        Select2GameObject.SetActive(false);
        Select3GameObject.SetActive(false);
    }

    public void GoToZones()
    {
        GoToNowWhere();
        ZonesGameObject.SetActive(true);
        playerProfile.ReadyToPlay = true;
        Utils.LinkListToObject(hand.Cards,"Hand" + playerProfile.Id);
        
    }

    private void moveListToZero(List<Card> l)
    {
        foreach (var e in l)
        {
            e.CardObject.transform.localPosition = Vector3.zero;
        }
    }

    public void GoToSelect1()
    {
        GoToNowWhere();
        moveListToZero(select1List);
        Select1GameObject.SetActive(true);
    }

    public void GoToSelect2()
    {
        GoToNowWhere();
        moveListToZero(select2List);
        Select2GameObject.SetActive(true);
    }

    public void GoToSelect3()
    {
        GoToNowWhere();
        moveListToZero(select3List);
        Select3GameObject.SetActive(true);
    }

    public void EndTurn()
    {
        Moves.Instance.Refresh();
        Models.ContainerMoves containerMoves = new Models.ContainerMoves(Moves.Instance);
        Debug.Log(containerMoves);
        string json = JsonUtility.ToJson(containerMoves);

        Debug.Log("Json: " + json);
        Utils.httpSend(Utils.SERVERURL + "moves", "POST", json);
        Moves.Instance.MovesList.Clear();

        /*Debug.Log("whowins?game_id=" + PlayerProfile.Instance.GameId);
        string str = Utils.httpSend(Utils.SERVERURL + "whowins?game_id="+PlayerProfile.Instance.GameId, "GET", "");
        Debug.Log(str);
        if (str.Contains("1"))
        {
            roundsWonBy1++;
            //TODO load scene where player 1 wins
            FinishText.text = " Congrats player1 !!!!";
        }

        if (str.Contains("2"))
        {
            roundsWonBy2++;
            FinishText.text = "Congrats player2 !!!!";
            //TODO load scnee where player 2 wins
        }
        Debug.Log("a dat 0");*/
    }

    public void ClearTable()
    {
        DestroyChildrenByTag("Goalkeeper1");
        DestroyChildrenByTag("Defense1");
        DestroyChildrenByTag("Midfield1");
        DestroyChildrenByTag("Forwards1");

        DestroyChildrenByTag("Goalkeeper2");
        DestroyChildrenByTag("Defense2");
        DestroyChildrenByTag("Midfield2");
        DestroyChildrenByTag("Forwards2");
    }

    private void DestroyChildrenByTag(string tag)
    {
        var objectWithTag = GameObject.FindGameObjectWithTag(tag);

        foreach (Transform child in objectWithTag.transform)
        {
            Destroy(child.gameObject);
            for (int i = 0; i < PlayerProfile.Instance.PlayerHand.Cards.Count; i++)
            {
                if (PlayerProfile.Instance.PlayerHand.Cards[i].CardObject.transform == child)
                {
                    PlayerProfile.Instance.PlayerHand.Cards.RemoveAt(i);
                    break;
                }
            }
        }
    }
}

