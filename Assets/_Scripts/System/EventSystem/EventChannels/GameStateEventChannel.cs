using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "GameStateEventChannel", menuName = "Event System/Channels/GameState Event Channel")]
public class GameStateEventChannel : ParameterizedEventChannel<GameState> {}