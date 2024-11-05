using Main.Gameplay.Player;
using UnityEngine;

public class BodyUnitsUIManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _unitSlotPrefab;
    [SerializeField] private Transform _slotsContainer;

    
    private void Awake()
    {
        InitializeUI();
    }

    private void Update()
    {
        
    }

    private void InitializeUI()
    {
        for(int i = 0; i <_playerController.BodyUnits.Count; i++)
        {
            //Instantiate(_unitSlotPrefab, _slotsContainer);
        }
    }
}
