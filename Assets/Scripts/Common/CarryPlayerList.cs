using UnityEngine;
using System.Collections.Generic;

public class CarryPlayerList : MonoBehaviour
{
    private static CarryPlayerList instance;
    public static CarryPlayerList singletone
    {
        get
        {
            return instance;
        }
    }

    public List<Player> players = new List<Player>();

    public void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
