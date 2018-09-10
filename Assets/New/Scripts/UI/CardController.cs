using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{

    //public
    public Text CardName;
    public Text CardDescription;
    public Card Card;
    public Button RemoveFromPlayerBtn;
    public Button PickBtn;

    public void Remove() {
        POEStatics.CardManager.RemoveCardFromPlayer(Card);
    }

    public void Pick() {
        POEStatics.CardManager.AddCardToPlayer(Card.CardData);
    }

}