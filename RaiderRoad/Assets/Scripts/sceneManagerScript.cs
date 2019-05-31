using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManagerScript : MonoBehaviour {

    public static sceneManagerScript Instance;
    public const string GAME_SCENE = "EnemyAI";
    public const string LOBBY_SCENE = "playerLobby";
    public const string MAIN_MENU_SCENE = "MainMenu";
    public const string LOAD_SCENE = "LoadingScreen";

    public List<string> NeedPlaySelScene; // a list of scenes that require the Player Select Scene before playing
    public List<string> NeedLoadScreen = new List<string> { GAME_SCENE, LOBBY_SCENE, MAIN_MENU_SCENE };

    
    private static string nextScene;
    private static int[] char1Players;
    private static int[] char2Players;
    private static int[] char3Players;
    private static int[] char4Players;
    private Transform[] playerPos = new Transform[4];

    public Transform character1; //FOR MOMENT, MENU MANAGER WILL HANDLE SPAWNING PLAYERS
    public Transform character2;
    public Transform character3;
    public Transform character4;
    public Material[] charaMatArray = new Material[4];
    public Texture[] charaTexArray = new Texture[4];
    private Transform rv; //rv is reference to RV obj in scene

    public List<Transform> playersInScene;
    [SerializeField] private int sharedWallCount = 12;

    void Awake() {
		// Have playlist persist across scenes.
		if (Instance == null) {
			DontDestroyOnLoad(gameObject); // Don't destroy this object
			Instance = this;
		}
		else {
			Destroy(this);
		}
    }

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(LOBBY_SCENE)) 
        {   
            nextScene = null;
        }
        else    //check in case someone tests game starting in the player select screen, if so default to first scene
        {
            nextScene = NeedPlaySelScene[0];
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("escape"))     //Set all variables back to null and send player to Scene menu
        {
            /*if(GameManager.GameManagerInstance != null) // make sure we have a game manager
            {
                GameManager g = GameManager.GameManagerInstance;
                g.clearMenu();
            }*/

            nextScene = NeedPlaySelScene[0];
            char1Players = null;
            char2Players = null;
            char3Players = null;
            char4Players = null;
            LoadLobby(); //was PlayerSelect
        }
	}

    public void LoadGame()
    {
        Load(GAME_SCENE);
    }

    public void LoadMainMenu()
    {
        ResetPlayerList();
        Load(MAIN_MENU_SCENE);
    }

    public void LoadLobby()
    {
        Load(LOBBY_SCENE, LoadSceneMode.Single);
    }

    public void LoadScene(string myScene) {
        if (NeedPlaySelScene.Contains(myScene))     //if scene needs player select, go to that and set it to be next scene
        {
            nextScene = myScene;
            Load(LOBBY_SCENE, LoadSceneMode.Single);
        } else
        {
            Load(myScene, LoadSceneMode.Single);
        }
    }

    public void OverrideNextScene(string myScene)
    {
        nextScene = myScene;
    }

    public void PlaySelDone(int[] c1Array, int[] c2Array, int[] c3Array, int[] c4Array) {
        char1Players = c1Array;     //Pull all the arrays from the player select menu (so we know how many of each character we have)
        char2Players = c2Array;
        char3Players = c3Array;
        char4Players = c4Array;

        Load(nextScene, LoadSceneMode.Single);
    }
    
    private void Load(string scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (isLoading)
        {
            Debug.LogError("Cannot load two scenes at once");
        }
        else if (NeedLoadScreen.Contains(scene))
        {
            StartCoroutine(LoadScreen(scene, mode));
        }
        else
        {
            SceneManager.LoadScene(scene, mode);
        }
    }

    private bool isLoading = false;
    private IEnumerator LoadScreen(string scene, LoadSceneMode mode)
    {
        isLoading = true;
        AsyncOperation loadLS = SceneManager.LoadSceneAsync(LOAD_SCENE, LoadSceneMode.Single);
        loadLS.allowSceneActivation = true;
        while (!loadLS.isDone)
        {
            yield return null;
        }
        
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(scene, mode);
        loadScene.allowSceneActivation = true;
        while (!loadScene.isDone)
        {
            yield return null;
        }
        isLoading = false;
    }

    //Load Main Game Scene Functions
    public void SpawnPlayers(GameObject myRv, Transform[] SpawnPoints)
    {
        GameManager g = GameManager.GameManagerInstance;
        playersInScene = new List<Transform>(); //Empty players list
        //Debug.Log("Players are Spawned");
        //finding RV
        rv = myRv.transform;
        //Debug.Log(rv.name);
        playerPos = SpawnPoints;
        //Debug.Log(char1Players.Length);
        int dividedWallCount = sharedWallCount / (char1Players.Length + char2Players.Length + char3Players.Length + char4Players.Length); // shared wall count divided equally amoung players

        spawnChar1(dividedWallCount);
        spawnChar2(dividedWallCount);
        spawnChar3(dividedWallCount);
        spawnChar4(dividedWallCount);

        //gameObject.SetActive(false);
        g.SetPlayers(playersInScene);
        g.restartMenu();
    }

    public void ResetPlayerList()
    {
        playersInScene.Clear();
    }

    void spawnChar1(int wallCount)
    {
        if (char1Players.Length > 0)
        {
            Transform[] player = new Transform[char1Players.Length];
            for (int i = 0; i < char1Players.Length; i++)       //for loop in case multiples of character
            {
                int playId = char1Players[i];
                Debug.Log("HEY" + playId);
                player[i] = Instantiate(character1, playerPos[playId].position, character1.rotation, rv);    //create character, set them to player spawn position
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(playId);
                PlayerPlacement_Rewired myPlayPlacement = player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>();
                myPlayPlacement.SetId(playId);
                myPlayPlacement.wallInventory = wallCount;

                AssignPlayMat(player[i].gameObject, playId, 0);  //passing player gameObject and player id
                //Debug.Log(player[i]);
                playersInScene.Add(player[i]);
            }
        }
    }

    void spawnChar2(int wallCount)
    {
        if (char2Players.Length > 0)
        {
            Transform[] player = new Transform[char2Players.Length];
            for (int i = 0; i < char2Players.Length; i++)
            {
                int playId = char2Players[i];
                player[i] = Instantiate(character2, playerPos[playId].position, character2.rotation, rv);
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(playId);
                PlayerPlacement_Rewired myPlayPlacement = player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>();
                myPlayPlacement.SetId(playId);
                myPlayPlacement.wallInventory = wallCount;

                AssignPlayMat(player[i].gameObject, playId, 1);

                playersInScene.Add(player[i]);
            }
        }
    }

    void spawnChar3(int wallCount)
    {
        if (char3Players.Length > 0)
        {
            Transform[] player = new Transform[char3Players.Length];
            for (int i = 0; i < char3Players.Length; i++)
            {
                int playId = char3Players[i];
                player[i] = Instantiate(character3, playerPos[playId].position, character3.rotation, rv);
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(playId);
                PlayerPlacement_Rewired myPlayPlacement = player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>();
                myPlayPlacement.SetId(playId);
                myPlayPlacement.wallInventory = wallCount;

                AssignPlayMat(player[i].gameObject, playId, 2);

                playersInScene.Add(player[i]);
            }
        }
    }

    void spawnChar4(int wallCount)
    {
        if (char4Players.Length > 0)
        {
            Transform[] player = new Transform[char4Players.Length];
            for (int i = 0; i < char4Players.Length; i++)
            {
                int playId = char4Players[i];
                player[i] = Instantiate(character4, playerPos[playId].position, character4.rotation, rv);
                player[i].gameObject.GetComponent<PlayerController_Rewired>().SetId(playId);
                PlayerPlacement_Rewired myPlayPlacement = player[i].Find("View").gameObject.GetComponent<PlayerPlacement_Rewired>();
                myPlayPlacement.SetId(playId);
                myPlayPlacement.wallInventory = wallCount;

                AssignPlayMat(player[i].gameObject, playId, 3);

                playersInScene.Add(player[i]);
            }
        }
    }

    public void AssignPlayMat(GameObject PlayerParent, int playerNum, int charNum)
    {
        Renderer[] myMats = PlayerParent.GetComponentsInChildren<Renderer>();   //assign materials of child objects

        Material InstMat = Instantiate(charaMatArray[playerNum]);
        //switch materialto correct character's mat (right now mats determine color, not character stuff, may change)
        InstMat.SetTexture("_MainTex", charaTexArray[charNum]);

        for (int j = 0; j < myMats.Length; j++)   //assign the same matterial to all the child objects
        {
            if (myMats[j].gameObject.name != "TempAttack" && myMats[j].gameObject.name != "tempGroundIndicator" && myMats[j].gameObject.tag != "CharaWeapon")  //Make sure not assigning attack visual object
            {

                Material[] tempMats = new Material[myMats[j].materials.Length]; //Check that an object doesn't have multiple mats (Vasilios!)
                for (int k = 0; k < tempMats.Length; k++)
                {
                    //Debug.Log(charaMatArray[playerNum]);
                    tempMats[k] = InstMat; //playerNum used to assign correct
                    //Debug.Log(tempMats[k]);
                }
                myMats[j].materials = tempMats;
            }
        }
        PlayerParent.GetComponent<PlayerController_Rewired>().myMat = InstMat;
    }
}
