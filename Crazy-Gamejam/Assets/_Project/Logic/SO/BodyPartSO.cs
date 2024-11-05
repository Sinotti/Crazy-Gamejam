using UnityEngine;

[CreateAssetMenu(fileName = "New BodyPartSO", menuName = "ScriptableObjects/BodyPartSO", order = 1)]
public class BodyPartSO : ScriptableObject
{
    [SerializeField] private GameObject _UnitUIPrefab;
    [SerializeField] private GameObject _UnitInGamePrefab;

    public GameObject Icon { get => _UnitUIPrefab; }
    public GameObject Prefab { get => _UnitInGamePrefab; }
}
