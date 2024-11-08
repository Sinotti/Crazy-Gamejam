using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [Space(6)]
    [SerializeField] private GameObject _store;
    
    public static UIManager Instance;

    private bool isPaused = false;

    [Space(6)]
    public UnityEvent OnPaused;

    public bool IsPaused { get => isPaused; set => isPaused = value; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(Instance);
    }
    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleStore();
        }
    }

    public void ToggleStore()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ToggleMenus();
            OnPaused?.Invoke();
            Time.timeScale = 0f;
        }
        else
        {
            ToggleMenus();
            Time.timeScale = 1f;
        }
    }
    
    private void ToggleMenus()
    {
        _store.SetActive(!_store.activeSelf);
    }
}
