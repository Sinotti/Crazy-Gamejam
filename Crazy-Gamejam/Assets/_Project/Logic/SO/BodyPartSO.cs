using UnityEngine;

[CreateAssetMenu(fileName = "New BodyPartSO", menuName = "ScriptableObjects/BodyPartSO", order = 1)]
public class BodyPartSO : ScriptableObject
{
    [SerializeField] private GameObject _unitUIPrefab;
    [SerializeField] private GameObject _unitInGamePrefab;

    public GameObject UnitUIPrefab { get => _unitUIPrefab; }
    public GameObject UnitInGamePrefab { get => _unitInGamePrefab; }
}
