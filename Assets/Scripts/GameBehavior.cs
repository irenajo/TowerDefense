using UnityEngine;
using UnityEngine.UIElements;

public class GameBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private UIDocument _document;
    private Button _waveButton;

    private AudioSource _buttonClick;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _waveButton = _document.rootVisualElement.Q<Button>("WaveButton");
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
    }
}
