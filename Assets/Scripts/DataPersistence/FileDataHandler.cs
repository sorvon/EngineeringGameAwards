using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFilename = "";
    public FileDataHandler(string dataDirPath, string dataFilename)
    {
        this.dataDirPath = dataDirPath;
        this.dataFilename = dataFilename;
    }

    public GameData Load(string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
        GameData gameData = null;
        if (File.Exists(fullPath))
        {
            Debug.Log(fullPath);
            try
            {
                string dataToLoad = "";
                using FileStream fs = File.OpenRead(fullPath);
                using StreamReader sr = new(fs);
                dataToLoad = sr.ReadToEnd();
                gameData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        return gameData;
    }

    public void Save(GameData gameData, string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);
            using FileStream fs = File.OpenWrite(fullPath);
            using StreamWriter sw = new(fs);
            sw.Write(dataToStore);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void Delete(string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
        try
        {
            File.Delete(fullPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDirctionary = new Dictionary<string, GameData>();
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (var dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;
            string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
            if (File.Exists(fullPath))
            {
                GameData gameData = Load(profileId);
                if (gameData != null)
                {
                    profileDirctionary.Add(profileId, gameData);
                }
                else
                {
                    Debug.LogError("Tried to load profile but something went wrong.ProfileId: " + profileId);
                }
            }
        }
        return profileDirctionary;
    }
}
