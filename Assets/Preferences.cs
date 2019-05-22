using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences
{
    private const string MESSAGE_KEY = "MESSAGE";
    private const string SAVEDATA_KEY = "SD";
    private const string SAVEDATA_ENCRYPTION_METHOD_KEY = "SDEM";

    private int encryptionMethod = 1;

    // Preferences

    
    // Save game data
    public SaveData saveData = new SaveData();

    public Preferences()
    {
        if (!PlayerPrefs.HasKey(MESSAGE_KEY) || !PlayerPrefs.HasKey(SAVEDATA_ENCRYPTION_METHOD_KEY))
        {
            PlayerPrefs.SetString(MESSAGE_KEY, "Do NOT delete or edit these values manually!!");
            PlayerPrefs.SetInt(SAVEDATA_ENCRYPTION_METHOD_KEY, 1);
            PlayerPrefs.Save();
        }

        string encryptedSaveDataJson = PlayerPrefs.GetString(SAVEDATA_KEY, "");
        encryptionMethod = PlayerPrefs.GetInt(SAVEDATA_ENCRYPTION_METHOD_KEY, 1);
        if(encryptedSaveDataJson.Length > 0)
        {
            LoadSaveData(encryptedSaveDataJson, encryptionMethod);
        }
    }

    private void LoadSaveData(string encryptedSaveDataJson, int encryptionMethod)
    {
        string saveDataJson = DecryptSaveDataJson(encryptedSaveDataJson, encryptionMethod);
        saveData = JsonUtility.FromJson<SaveData>(saveDataJson);
    }

    public void SaveProgress()
    {
        string saveDataJson = JsonUtility.ToJson(saveData);
        string encryptedSaveDataJson = EncryptSaveDataJson(saveDataJson, encryptionMethod);
        PlayerPrefs.SetString(SAVEDATA_KEY, encryptedSaveDataJson);
        PlayerPrefs.Save();
    }

    private string DecryptSaveDataJson(string encryptedSaveDataJson, int encryptionMethod)
    {
        return encryptedSaveDataJson;//TODO implement
    }

    private string EncryptSaveDataJson(string rawSaveDataJson, int encryptionMethod)
    {
        return rawSaveDataJson;//TODO implement
    }
}
