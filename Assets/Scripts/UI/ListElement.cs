using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListElement : MonoBehaviour
{
    public Text PlayerNameText;
    public Text PlayerRateText;
    public Button RemoveButton;

    public string PlayerName;
    public string PlayerRate;

    public void Start()
    {
        //PlayerNameText = GameObject.Find("PlayerName").GetComponent<Text>();
        //PlayerRateText = GameObject.Find("PlayerRate").GetComponent<Text>();
    }

    public void Update()
    {
        PlayerNameText.text = PlayerName;
        PlayerRateText.text = PlayerRate;
    }

    public void SetIndexOfCurrentList(int ID)
    {
        //RemoveButton.gameObject.AddComponent<ListElementIndex>();
        RemoveButton.GetComponent<ListElementIndex>().index = ID;
    }
}
