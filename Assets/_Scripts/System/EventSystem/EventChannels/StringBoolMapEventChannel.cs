using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "StringBoolMapEventChannel", menuName = "Event System/Channels/String Bool Map Event Channel", order = 2)]
public class StringBoolMapEventChannel : ParameterizedEventChannel<Dictionary<string,bool>> {}
