using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [Space(6)]
    [SerializeField] private GameObject _store;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _textPanel1;
    [SerializeField] private GameObject _textPanel2;
    [SerializeField] private TextMeshProUGUI _textMeshPro1;
    [SerializeField] private TextMeshProUGUI _textMeshPro2;

    public static UIManager Instance;

    private bool isPaused = false;

    [Space(6)]
    public UnityEvent OnPaused;

    public bool IsPaused { get => isPaused; set => isPaused = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
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

    public void ShowTextSequence(int currentWave, int nextWave)
    {
        StartCoroutine(PauseAndShowTextSequence(currentWave, nextWave));
    }

    private IEnumerator PauseAndShowTextSequence(int currentWave, int nextWave)
    {
        isPaused = true;
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);

        // Exibe o texto da próxima wave no segundo painel
        _textMeshPro2.text = currentWave.ToString();
        _textPanel1.SetActive(false);
        _textMeshPro1.gameObject.SetActive(false);
        _textPanel2.SetActive(true);
        _textMeshPro2.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3f); // Exibe o primeiro texto por 5 segundos

        // Exibe o texto da wave atual no primeiro painel
        _textMeshPro1.text = nextWave.ToString();
        _textPanel1.SetActive(true);
        _textMeshPro1.gameObject.SetActive(true);
        _textPanel2.SetActive(false);
        _textMeshPro2.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(3f); // Exibe o segundo texto por mais 5 segundos

        // Desativa tudo e retoma o jogo
        _pausePanel.SetActive(false);
        _textPanel2.SetActive(false);
        _textMeshPro2.gameObject.SetActive(false);

        ToggleMenus();
    }
}
