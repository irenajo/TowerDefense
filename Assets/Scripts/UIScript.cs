using UnityEngine;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{
    // UI Elements this script will modify the behavior of
    private UIDocument _document;
    private Button _waveButton;

    private Label health;
    private Label coins;


    // Scripts
    private GameManager gameManager;


    // Sounds
    private AudioSource _buttonClick;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }


    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _waveButton = _document.rootVisualElement.Q<Button>("WaveButton");
        health = _document.rootVisualElement.Q<Label>("WaveText");
        coins = _document.rootVisualElement.Q<Label>("HealthText");

        _buttonClick = GetComponent<AudioSource>();

        _waveButton.RegisterCallback<ClickEvent>(OnAllButtonsClick);


    }

    private void OnDisable()
    {
        _waveButton.UnregisterCallback<ClickEvent>(OnAllButtonsClick);
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _buttonClick.Play();
        gameManager.StartWave();
    }
}
