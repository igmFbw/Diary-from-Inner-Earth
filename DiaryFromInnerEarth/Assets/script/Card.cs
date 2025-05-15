using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : MonoBehaviour,IPointerClickHandler
{
    public int cardNum;
    [SerializeField] private Text numText;
    [SerializeField] private GameObject squareToChange;
    [SerializeField] private mouseClick mouseClickHandle;
    public void OnPointerClick(PointerEventData eventData)
    {
        mouseClick newEvent = Instantiate(mouseClickHandle);
        newEvent.setSquare(squareToChange);
    }
    public void updateNum()
    {
        numText.text = cardNum.ToString();
    }
}
