using UnityEngine;

[CreateAssetMenu(fileName = "New BodyPartSO", menuName = "ScriptableObjects/BodyPartSO", order = 1)]
public class BodyPartSO : ScriptableObject
{
    [SerializeField] private GameObject _unitUIPrefab;
    [SerializeField] private GameObject _unitInGamePrefab;
    [SerializeField] private int _price;

    public GameObject UnitUIPrefab { get => _unitUIPrefab; }
    public GameObject UnitInGamePrefab { get => _unitInGamePrefab; }
    public int Price { get => _price; set => _price = value; }
}
