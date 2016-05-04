using UnityEngine;
using System.Collections;

public class ListElementIndex : MonoBehaviour
{
    public int index;


    public void RemoveElement()
    {
        c_MainMenuManager.singletone.RemoveListElement(index, transform.parent.gameObject);
    }
}
