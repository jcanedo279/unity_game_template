using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MultiCriteriaAction", menuName = "Event System/MultiCriteria Action")]
public class MultiCriteriaAction : ScriptableObject {
    [SerializeField] public string multiCriteriaName = "";
    public HashSet<string> criteriaNames;
    [SerializeField] public StringEventChannel onMultiCriteriaMetEventChannel;
    [SerializeField] public MultiCriteria.MultiCriteriaEnum multiCriteriaEnum;
    public bool isMultiCriteriaMet = false;
}
