using Main.Gameplay.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BodyUnitSelectionUI : MonoBehaviour
{
    [Header("Store Parameters")]
    [Space(6)]
    [SerializeField] private int playerMoney = 100;
    [SerializeField] private List<BodyUnitStoreItem> availableBodyParts;

    [Header("Player Money")]
    [Space(6)]

    [Header("References")]
    [Space(6)]
    [SerializeField] private Transform selectionContainer;
    [SerializeField] private GameObject unitButtonPrefab;
    [SerializeField] private BodyUnitsUIManager unitsUIManager;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private PlayerController playerController;

    private List<BodyUnitStoreItem> currentSelection = new List<BodyUnitStoreItem>();

    [System.Serializable]
    public class BodyUnitStoreItem
    {
        public BodyPartSO bodyPart;
        public int price;
    }

    private void OnEnable()
    {
        GenerateRandomSelection();
        UpdateMoneyUI();
    }

    private void GenerateRandomSelection()
    {
        foreach (Transform child in selectionContainer)
        {
            Destroy(child.gameObject);
        }
        currentSelection.Clear();

        for (int i = 0; i < 3; i++)
        {
            BodyUnitStoreItem randomItem = availableBodyParts[Random.Range(0, availableBodyParts.Count)];
            currentSelection.Add(randomItem);

            GameObject unitButton = Instantiate(unitButtonPrefab, selectionContainer);
            TextMeshProUGUI buttonText = unitButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = $"{randomItem.bodyPart.name} - {randomItem.price}$";

            Button button = unitButton.GetComponent<Button>();
            button.onClick.AddListener(() => TrySelectBodyPart(randomItem));
        }
    }

    private void TrySelectBodyPart(BodyUnitStoreItem selectedItem)
    {
        if (playerMoney >= selectedItem.price) 
        {
            playerMoney -= selectedItem.price; 
            playerController.AddBodyUnit(selectedItem.bodyPart);
            unitsUIManager.UpdateUI(); 
            UpdateMoneyUI(); 
            Debug.Log($"Você comprou {selectedItem.bodyPart.name}!");
        }
        else
        {
            Debug.Log("Dinheiro insuficiente para comprar esta unidade.");
        }
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = playerMoney.ToString();  
    }
}
