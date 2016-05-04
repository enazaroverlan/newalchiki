using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class c_StepByStepLogic : MonoBehaviour
{
    private static c_StepByStepLogic instance;
    public static c_StepByStepLogic singletone
    {
        get
        {
            return instance;
        }
    }

    public GameObject AlchiksPosition;
    public GameObject alchik;
    public Quaternion rotationAlchik;

    public Camera plCam;

    public CurrentStep curStep = CurrentStep.choosingAPostion;
    public List<Player> allPlayers = new List<Player>();

    [Header("Cam Position Vars")]
    public GameObject CameraPositionAxes;
    public Transform DefPlace;
    public Transform choosingApositionPlace;

    c_LounchSwipe manager;
    [HideInInspector]
    public int indexOfCurrentPlayer = 0;
    bool isGameStarted = false;

    [Header("Other")]
    public GameObject PlaceSelected;


    public GameObject CurrentChukko
    {
        get; set;
    }
    
    public void Start()
    {
        instance = this;
        manager = GameObject.FindObjectOfType<c_LounchSwipe>();
        if (allPlayers.Count != 0)
        {
            curStep = CurrentStep.choosingAPostion;
            isGameStarted = true;
        }

        //allPlayers = CarryPlayerList.singletone.players;

        foreach (Player pl in allPlayers)
        {
            for (int i = 0; i < pl.Rate; i++)
            {
                GameObject inst_rate = Instantiate(alchik) as GameObject;
                inst_rate.transform.position = new Vector3((AlchiksPosition.transform.position.x - (allPlayers.Count * pl.Rate)) + (i), AlchiksPosition.transform.position.y + 0.1f, AlchiksPosition.transform.position.z);
                inst_rate.transform.rotation = rotationAlchik;
                inst_rate.AddComponent<Rigidbody>();
                inst_rate.tag = "Alchik";
            }
        }
    }

    public void FixedUpdate()
    {

        if(curStep != CurrentStep.choosingAPostion && curStep != CurrentStep.positionChosen)
            plCam.transform.LookAt(new Vector3(0, 0, choosingApositionPlace.transform.position.z));

        if (curStep == CurrentStep.actionDid)
        {
            if (indexOfCurrentPlayer == allPlayers.Count)
            {
                curStep = CurrentStep.getResults;
                manager.action = curStep.ToString();
            }
            else
            {
                curStep = CurrentStep.doAction;
                manager.action = curStep.ToString();
            }


        }

        if (curStep == CurrentStep.doAction)
        {
            ShootAlchikk();

            if (c_LounchSwipe.singletone.isLauched)
            {
                if (CurrentChukko.GetComponent<Rigidbody>().velocity.magnitude == 0)
                    StartCoroutine(AlchikkShooted(CurrentChukko.GetComponent<CollisionListener>().collided? true : false));
            }
        }



        if (curStep == CurrentStep.choosingAPostion)
            ChoosingAPostion();


        if (curStep == CurrentStep.choosingAPostion || curStep == CurrentStep.positionChosen)
        {
            if (c_LounchSwipe.singletone.isLauched)
            {
                if (CurrentChukko.GetComponent<Rigidbody>().velocity.magnitude == 0)
                    PositionChoosen(CurrentChukko.transform.position);
            }
        }


        if (curStep == CurrentStep.getResults)
        {
            GetResults();
        }

    }

    public void ChoosingAPostion()
    {
        if (isGameStarted) //?
        {
            //Yes
            CameraPositionAxes.transform.position = Vector3.Lerp(CameraPositionAxes.transform.position, choosingApositionPlace.transform.position, 3.5f * Time.deltaTime);
            CameraPositionAxes.transform.rotation = Quaternion.Lerp(CameraPositionAxes.transform.rotation, choosingApositionPlace.transform.rotation, 3.5f * Time.deltaTime);
        }
    }
        
    int isDone = 0;
    public void PositionChoosen(Vector3 position)
    {
        Debug.Log("PositionChoosen: " + position);
        curStep = CurrentStep.positionChosen;
        manager.action = curStep.ToString();
        allPlayers[indexOfCurrentPlayer].ShootPlace = position;
        GameObject inst_place =  (GameObject)Instantiate(PlaceSelected);
        inst_place.transform.position = position;
        //inst_place.transform.rotation = new Quaternion(270.0198f, 0, 0, 0);
        Destroy(CurrentChukko);
        isDone = 0;
        foreach (Player pl in allPlayers)
        {
            if (pl.ShootPlace == Vector3.zero)
            {
                isDone++;
            }
        }

        if (isDone <= 0)
        {
            indexOfCurrentPlayer = 00;
            curStep = CurrentStep.doAction;
            manager.action = curStep.ToString();
        }
        else
        {
            indexOfCurrentPlayer++;
        }

        CurrentChukko = null;
        c_LounchSwipe.singletone.isLauched = false;

    }

    public void ShootAlchikk()
    {

        Debug.Log("Position choosen and ready to shoot!!!!");

        CameraPositionAxes.transform.position = Vector3.Lerp(CameraPositionAxes.transform.position, new Vector3(allPlayers[indexOfCurrentPlayer].ShootPlace.x, CameraPositionAxes.transform.position.y, allPlayers[indexOfCurrentPlayer].ShootPlace.z), 3.5f * Time.deltaTime);
        CameraPositionAxes.transform.rotation = Quaternion.Lerp(CameraPositionAxes.transform.rotation, DefPlace.rotation, 3.5f * Time.deltaTime);
    }

    int b = 0;
    public IEnumerator AlchikkShooted(bool collided)
    {
        Debug.Log("Alchik shooted!!!");
        if (!collided)
        {
            Debug.Log("Collided");
            allPlayers[indexOfCurrentPlayer].shooted = true;
            indexOfCurrentPlayer++;
        }
        else
        {
            allPlayers[indexOfCurrentPlayer].ShootPlace = CurrentChukko.transform.position;
        }
        curStep = CurrentStep.actionDid;
        manager.action = curStep.ToString();

        CurrentChukko = null;
        c_LounchSwipe.singletone.isLauched = false;

        yield return new WaitForSeconds(0.5f);

        

        b = 0;
        foreach (Player pl in allPlayers)
        {
            if (pl.shooted)
            {
                b++;
            }
        }

        if (b >= 3)
        {
            curStep = CurrentStep.getResults;
            manager.action = curStep.ToString();
        }
        else
        {
            curStep = CurrentStep.doAction;
            manager.action = curStep.ToString();
        }
    }
    
    public void OnGUI()
    {
        GUI.Box(new Rect(0,0, 200, 30), curStep.ToString());
        GUI.Box(new Rect(0,40, 200, 30), indexOfCurrentPlayer+"");
        GUI.Box(new Rect(Screen.width - 200, 0, 190, 30), "Current player: " + allPlayers[indexOfCurrentPlayer].Name);
    }
    
    public void GetResults()
    {
        CameraPositionAxes.transform.position = Vector3.Lerp(CameraPositionAxes.transform.position, DefPlace.position, 3.5f * Time.deltaTime);
        CameraPositionAxes.transform.rotation = Quaternion.Lerp(CameraPositionAxes.transform.rotation, DefPlace.rotation, 3.5f * Time.deltaTime);
    }

}
public enum CurrentStep
{
    choosingAPostion = 0,
    positionChosen = 1,
    doAction = 2,
    actionDid = 3,
    getResults = 4
}
[System.Serializable]
public class Player
{
    public string Name = "Player";
    public int Rate = 0;
    public int Score;
    public Vector3 ShootPlace;
    public bool shooted = false;
    public List<GameObject> CollidedObjects = new List<GameObject>();
}
