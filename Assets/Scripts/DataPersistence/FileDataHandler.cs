using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFilename = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "¶«¹ÏÎ÷¹Ï¹þÃÜ¹Ï£¬¹Ø×¢ÄËÁÕÐ»Ð»¸Â";
    public FileDataHandler(string dataDirPath, string dataFilename, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFilename = dataFilename;
        this.useEncryption = useEncryption;
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
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
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
            File.Delete(fullPath);
            string dataToStore = JsonUtility.ToJson(gameData, true);
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
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
        if (profileId == null)
        {
            return;
        }
        string fullPath = Path.Combine(dataDirPath, profileId, dataFilename);
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
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

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
