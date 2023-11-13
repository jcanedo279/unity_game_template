using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MultiCriteriaActionListener : StringBoolMapEventChannelListener {
    [SerializeField] List<MultiCriteriaAction> multiCriteriaActions;
    private Dictionary<string,List<int>> criteriaNameToMultiCriteriaIndex;

    public void resetMultiCriteriaAction(MultiCriteriaAction multiCriteriaAction) {
        multiCriteriaAction.isMultiCriteriaMet = false;
        multiCriteriaAction.criteriaNames
            = new HashSet<string>(
                MultiCriteria.MultiCriteriaFromEnum(multiCriteriaAction.multiCriteriaEnum));
    }

    void Awake() {
        // Initialize multiCriteriaDictionary on startup.
        criteriaNameToMultiCriteriaIndex = new Dictionary<string, List<int>>();
        for (int multiCriteriaIndex=0; multiCriteriaIndex<multiCriteriaActions.Count; multiCriteriaIndex++) {
            MultiCriteriaAction multiCriteriaAction = multiCriteriaActions[multiCriteriaIndex];
            resetMultiCriteriaAction(multiCriteriaAction);
            foreach (string criteriaName in multiCriteriaAction.criteriaNames) {
                if (criteriaNameToMultiCriteriaIndex.ContainsKey(criteriaName)) {
                    criteriaNameToMultiCriteriaIndex[criteriaName].Add(multiCriteriaIndex);
                } else {
                    criteriaNameToMultiCriteriaIndex.Add(criteriaName, new List<int>() {multiCriteriaIndex});
                }
            }
        }
    }

    // We intercept multiCriteria event updates from the listener base class and use this,
    // to check for multiCriteria status change before calling base implementation.
    protected override void InvokeUnityEventResponse(Dictionary<string,bool> multiCriteria) {
        // We loop over each criteriaName in the event and loop over all multiCriteria which
        // we know have the corresponding criteriaName (either met or unmet).
        // In the future we would like to change this to either add in criteriaName for certain multiCriteria
        // through events or through a method call. Like AddMultiCriteriaToMultiCriteria(multiCriteriaName,multiCriteria).
        foreach ((string criteriaName, bool isMet) in multiCriteria) {
            if (!criteriaNameToMultiCriteriaIndex.ContainsKey(criteriaName)) {
                // Trying to remove a non registered criteria.
                Debug.Log($"criteriaName: {criteriaName} is not registered to any multiCriteria on this channel.");
                continue;
            }
            if (isMet) {
                // Remove criteriaName from all multiCriteria where criteriaName is not met.
                foreach (int multiCriteriaIndex in criteriaNameToMultiCriteriaIndex[criteriaName]) {
                    MultiCriteriaAction multiCriteriaAction = multiCriteriaActions[multiCriteriaIndex];
                    if (multiCriteriaAction.criteriaNames.Contains(criteriaName)) {
                        multiCriteriaAction.criteriaNames.Remove(criteriaName);
                        // Flip criteriaStatus is no criteria left.
                        if (multiCriteriaAction.criteriaNames.Count==0) {
                            multiCriteriaAction.onMultiCriteriaMetEventChannel.RaiseEvent(multiCriteriaAction.multiCriteriaName);
                            multiCriteriaAction.isMultiCriteriaMet = true;
                        }
                    }
                }
            } else {
                // Add criteriaName to all multiCriteria.
                foreach (int multiCriteriaIndex in criteriaNameToMultiCriteriaIndex[criteriaName]) {
                    MultiCriteriaAction multiCriteriaAction = multiCriteriaActions[multiCriteriaIndex];
                    multiCriteriaAction.criteriaNames.Add(criteriaName);
                    // Flip criteriaStatus if criteria was previously met.
                    if (multiCriteriaAction.isMultiCriteriaMet) {
                        multiCriteriaAction.isMultiCriteriaMet = false;
                    }
                }
            }
        }
        base.InvokeUnityEventResponse(multiCriteria);
   }
}


public class MultiCriteria {
    public static Dictionary<MultiCriteriaEnum,HashSet<string>> multiCriteriaEnumToMultiCriteria
            = new Dictionary<MultiCriteriaEnum, HashSet<string>>() {
        {MultiCriteriaEnum.MULTI_CRITERIA_ENUM_GAME_STATE_TRANSITION_IS_READY, new HashSet<string>(){
            {"isCriteriaMetSceneLoad"},
            {"isCriteriaMetDataLoad"}
        }},
        {MultiCriteriaEnum.MULTI_CRITERIA_ENUM_GAME_STATE_TRANSITION_TO_AQUARIUM, new HashSet<string>() {
            {"isCriteriaMetAquariumRequest"}
        }}

    };

    public enum MultiCriteriaEnum {
        MULTI_CRITERIA_ENUM_GAME_STATE_TRANSITION_IS_READY,
        MULTI_CRITERIA_ENUM_GAME_STATE_TRANSITION_TO_AQUARIUM,
    }

    public static HashSet<string> MultiCriteriaFromEnum(MultiCriteriaEnum multiCriteriaEnum) {
        if (!MultiCriteria.multiCriteriaEnumToMultiCriteria.ContainsKey(multiCriteriaEnum)) {
            throw new System.ArgumentOutOfRangeException(
                $"MultiCriteriaEnum: {multiCriteriaEnum} does not have a registered multiCriteria name set.");
        }
        return MultiCriteria.multiCriteriaEnumToMultiCriteria[multiCriteriaEnum];
    }
}
