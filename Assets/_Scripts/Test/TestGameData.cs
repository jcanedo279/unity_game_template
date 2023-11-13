using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
public class TestGameData {
    [JsonProperty("testProperty")]
    public string testProperty;
}
