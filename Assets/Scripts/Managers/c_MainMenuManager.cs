using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class c_MainMenuManager : MonoBehaviour
{
    private static c_MainMenuManager instance;
    public static c_MainMenuManager singletone
    {
        get
        {
            return instance;
        }
    }

    public GameObject MainCanvas;
    //public Texture2D BackGorund;

    public InputField PlayerNameText;
    public InputField PlayerRateText;
    public GameObject ListElementPrefab;
    public GameObject[] AllCanvases;
    public GameObject ListContent;

    public List<Player> players = new List<Player>();

    [Header("Settings Vars")]
    public Toggle onOffMusic;
    public Scrollbar sBar;
    public Text sBarValue;
    public Slider hSlider;
    public Text GraphicsText;

    AsyncOperation async;

    public void Start()
    {
        instance = this;
    }

    bool isMusic = true;
    [HideInInspector]
    public float volume;
    [HideInInspector]
    public float graphicIndex;


    public void Update()
    {
        volume = sBar.value;

        sBarValue.text = Mathf.Round(AudioListener.volume * 100).ToString() + "%";

        if (isMusic)
            AudioListener.volume = volume;
        else
            AudioListener.volume = 0;



        graphicIndex = hSlider.value;

        int gIndex = Mathf.RoundToInt(graphicIndex);

        switch (gIndex)
        {
            case 0: QualitySettings.currentLevel = QualityLevel.Fastest; break;
            case 1: QualitySettings.currentLevel = QualityLevel.Fast; break;
            case 2: QualitySettings.currentLevel = QualityLevel.Simple; break;
            case 3: QualitySettings.currentLevel = QualityLevel.Good; break;
            case 4: QualitySettings.currentLevel = QualityLevel.Beautiful; break;
            case 5: QualitySettings.currentLevel = QualityLevel.Fantastic; break;
        }
        GraphicsText.text = QualitySettings.currentLevel.ToString();




    }
    

    //===================== UI button's voids ==================
    public void GoToMenu(string NextMenu)
    {
        for (int i = 0; i < AllCanvases.Length; i++)
        {
            if (AllCanvases[i].name == NextMenu)
                AllCanvases[i].SetActive(true);
            else
                AllCanvases[i].SetActive(false);
        }
    }

    public void GoToMainCanvas()
    {
        GoToMenu("MainPanel");
    }

    public void GoToOptionCanvas()
    {
        GoToMenu("OptionsPanel");
    }

    public void GoToPlayerSettingsCanvas()
    {
        GoToMenu("PlayerSettings");
    }

    public void GoToQuitGame()
    {
        Application.Quit();
    }

    public void AddNewPlayer()
    {
        Player pl = new Player();
        pl.Name = PlayerNameText.text;
        pl.Rate = StrToInt(PlayerRateText.text);
        players.Add(pl);

        GameObject inst_el = Instantiate(ListElementPrefab);
        inst_el.GetComponent<ListElement>().PlayerName = PlayerNameText.text;
        inst_el.GetComponent<ListElement>().PlayerRate = PlayerRateText.text;
        inst_el.transform.SetParent(ListContent.transform);
        inst_el.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        inst_el.GetComponent<ListElement>().SetIndexOfCurrentList(players.IndexOf(pl));

        PlayerNameText.text = "";
        PlayerRateText.text = "";

    }

    public void StartGame()
    {
        GameObject carryer = new GameObject();
        carryer.name = "Carryer!!";
        carryer.AddComponent<CarryPlayerList>();
        carryer.GetComponent<CarryPlayerList>().players = players;
        if (players.Count > 1)
            StartCoroutine(LoadLevel("NewGamePlay"));
        else
            Debug.Log("no players added!!");
    }
    bool isLevelLoading;
    public IEnumerator LoadLevel(string nextLevel)
    {
        async = SceneManager.LoadSceneAsync(nextLevel);
        isLevelLoading = true;
        MainCanvas.SetActive(false);
        yield return async;
        isLevelLoading = false;
        Debug.Log("Level Loaded!!");
        SceneManager.LoadScene(nextLevel);
    }
    
    public void OnGUI()
    {
        if (isLevelLoading)
        {
            if (!async.isDone)
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "Loading... " + async.progress.ToString() + "%");
            else
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "Done!");
        }
    }

    public void RemoveListElement(int index, GameObject parent)
    {
        players.Remove(players[index]);
        Destroy(parent);
    }

    public int StrToInt(string value)
    {
        int k = new int();
        for (int i = 0; i < value.Length; i++)
        {
            switch (value[i])
            {
                case '0': if (k == 0) { k = 0; } else { k *= 10; } break;
                case '1': if (k == 0) { k = 1; } else { k *= 10; k += 1; } break;
                case '2': if (k == 0) { k = 2; } else { k *= 10; k += 2; } break;
                case '3': if (k == 0) { k = 3; } else { k *= 10; k += 3; } break;
                case '4': if (k == 0) { k = 4; } else { k *= 10; k += 4; } break;
                case '5': if (k == 0) { k = 5; } else { k *= 10; k += 5; } break;
                case '6': if (k == 0) { k = 6; } else { k *= 10; k += 6; } break;
                case '7': if (k == 0) { k = 7; } else { k *= 10; k += 7; } break;
                case '8': if (k == 0) { k = 8; } else { k *= 10; k += 8; } break;
                case '9': if (k == 0) { k = 9; } else { k *= 10; k += 9; } break;
            }
        }
        return k;
    }

    public void OnToggleValueChanged()
    {
        if (isMusic)
            isMusic = !isMusic;
        else
            isMusic = true;
    }

    public void GoToMatchmakingCunvas()
    {
        GoToMenu("InternetMatchMaking");
    }
}
