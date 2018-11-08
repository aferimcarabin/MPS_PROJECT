using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Moves queueMoves = Moves.Instance;

    public GameObject scoreUpdater;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.
            GetComponent<Draggable>();

        if (d != null)
        {
            d.placeholderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    private void AddMove(GameObject cardObject, string terName,int val)
    {
        Debug.Log(gameObject.name);
        if (gameObject.name.ToLower().Contains(terName.ToLower()))
        {
            queueMoves.MovesList.Add(new Move(PlayerProfile.Instance.PlayerHand.GetCard(cardObject).Id, val));
            Debug.Log("Q:" + queueMoves);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        Debug.Log("x: " + transform.localPosition.x + "  , id = " + PlayerProfile.Instance.Id + "  , " + PlayerProfile.Instance.MyTurnFast);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && ((PlayerProfile.Instance.Id == 1 && this.transform.localPosition.x < 0) || (PlayerProfile.Instance.Id == 2 && this.transform.localPosition.x > 0)) && PlayerProfile.Instance.MyTurnFast)
        {
            d.parentToReturnTo = this.transform;

            AddMove(eventData.pointerDrag, "goalkeeper", 1);
            AddMove(eventData.pointerDrag, "defense", 2);
            AddMove(eventData.pointerDrag, "midfield", 3);
            AddMove(eventData.pointerDrag, "forwards", 4);
        }

        PlayerProfile.Instance.Score = scoreUpdater.GetComponent<ScoreUpdater>().calculateScore(PlayerProfile.Instance.Id);
    }
}
