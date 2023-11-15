using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    #region Singleton
    // Singleton
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            
            return _instance;
        }
    }
    // End Singleton
    #endregion

    public string GameDataFileName = ".json";   // file name of game data save

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }
    public void LoadGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, GameDataFileName);
        if (File.Exists(filePath))
        {
            Debug.Log("Loading game data from: " + filePath);

            string dataAsJson = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            Debug.Log("No game data found at: " + filePath);

            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Path.Combine(Application.persistentDataPath, GameDataFileName);
        File.WriteAllText(filePath, dataAsJson);

        Debug.Log("Saved game data to: " + filePath);
    }


    


}
