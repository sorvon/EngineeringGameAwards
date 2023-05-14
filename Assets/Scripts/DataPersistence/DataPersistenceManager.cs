using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    //[Header("File Storage Config")]
    public static DataPersistenceManager instance { get; private set; }
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler fileDataHandler;
    [Header("File Storage Config")]
    [SerializeField] private string selectedProfileId = "data.game";
    [SerializeField] bool useEncryption = false;
    public void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more then one DataPersistenceManager in the scene.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        fileDataHandler = new(Application.persistentDataPath, selectedProfileId, useEncryption);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    private void Update()
    {
        if(gameData != null)
        {
            gameData.durationTime += Time.deltaTime;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        };
    }
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load(selectedProfileId);
        if (gameData == null)
        {
            Debug.Log("No data was found");
            return;
        }
        foreach (var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogWarning("No game data was found.");
            return ;
        }
        foreach (var dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(gameData);
        }
        gameData.lastTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        fileDataHandler.Save(gameData, selectedProfileId);
    }
    public void DeleteData(string profileId)
    {
        fileDataHandler.Delete(profileId);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public bool HasGameData()
    {
        return gameData != null;
    }
    public Dictionary<string, GameData> GatAllProfilesGameData()
    {
        return fileDataHandler.LoadAllProfiles();
    }

    public void ChangeSelectedProfileId(string value)
    {
        selectedProfileId = value;
    }

    public GameData GetCurrentGameData()
    {
        return gameData;
    }
}
