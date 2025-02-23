using UnityEngine;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{
    // UI Elements this script will modify the behavior of
    private UIDocument _document;
    private Button _waveButton;

    private Label health;
    private Label coins;
    private Label wave;


    // Scripts
    private GameManager _gameManager;


    // Sounds
    private AudioSource _buttonClick;
    void Start()
    {
        _gameManager = GetComponent<GameManager>();

        EventBus.Instance.OnUpdateHealthUI += updateHealthUI;
        EventBus.Instance.OnUpdateCoinsUI += updateCoinsUI;
        EventBus.Instance.OnWaveStart += updateWaveUI;

        updateHealthUI();
        updateCoinsUI();
    }

    private void OnEnable()
    {
        _waveButton.RegisterCallback<ClickEvent>(OnWaveButtonClick);

    }

    private void OnDisable()
    {
        _waveButton.UnregisterCallback<ClickEvent>(OnWaveButtonClick);

        EventBus.Instance.OnUpdateHealthUI -= updateHealthUI;
        EventBus.Instance.OnUpdateCoinsUI -= updateCoinsUI;
    }


    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _waveButton = _document.rootVisualElement.Q<Button>("WaveButton");
        health = _document.rootVisualElement.Q<Label>("HealthText");
        coins = _document.rootVisualElement.Q<Label>("CoinsText");
        wave = _document.rootVisualElement.Q<Label>("WaveText");

        _buttonClick = GetComponent<AudioSource>();

        // _waveButton.RegisterCallback<ClickEvent>(OnWaveButtonClick);
    }

    private void updateHealthUI()
    {
        health.text = "Health: " + _gameManager.GetHealth();
    }

    private void updateCoinsUI()
    {
        coins.text = "Coins: " + _gameManager.GetCoins();
    }

    private void updateWaveUI()
    {
        wave.text = "Wave: " + _gameManager.GetWaveNumber();
    }
    private void OnWaveButtonClick(ClickEvent evt)
    {
        _buttonClick.Play();
        _gameManager.StartWave();
    }

}
