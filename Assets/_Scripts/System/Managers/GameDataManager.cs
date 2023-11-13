using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A SO wrapper for some game data which is related to itself in some way.
public class GameData : ScriptableObject {
    private Dictionary<string,object>  gameData;
}

// A SO wrapper which contains all the gameDataManager's data.
public class GameDataManagerData : ScriptableObject {
    private Dictionary<string,object> gameDataManagerData = new Dictionary<string,object>();
}

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameDataManagerData gameDataManagerData;
}
