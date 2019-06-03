using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleGameMenu : MonoBehaviour
{
    public static SimpleGameMenu Instance;

    [SerializeField] Button play_button;
    [SerializeField] Button restart_button;
    [SerializeField] Button lobby_button;
    [SerializeField] TextMeshProUGUI score_text;
    [SerializeField] GameObject lobby_Panel;
    [SerializeField] GameObject game_Panel;
    [SerializeField] GameObject endGame_Panel;

    #region Accesores
    public Button Play_button { get => play_button; set => play_button = value; }
    public Button Restart_button { get => restart_button; set => restart_button = value; }
    public Button Lobby_button1 { get => lobby_button; set => lobby_button = value; }
    public TextMeshProUGUI Score_text { get => score_text; set => score_text = value; }
    public GameObject Lobby_Panel1 { get => lobby_Panel; set => lobby_Panel = value; }
    public GameObject Game_Panel { get => game_Panel; set => game_Panel = value; }
    public GameObject EndGame_Panel { get => endGame_Panel; set => endGame_Panel = value; } 
    #endregion

    private void Awake()
    {
        if (Instance == null)//establece el singelton
            Instance = this;
        
        //prende y apaga los paneles necesarios 
        game_Panel.SetActive(false);
        lobby_Panel.SetActive(true);
        endGame_Panel.SetActive(false);

        SetUpEvents();

    }
    void SetUpEvents()//establece los eventos de los bonotenes de UI
    {
        //cuando hago clik en el boton de play
        play_button.onClick.AddListener(() => {
            lobby_Panel.SetActive(false);
            game_Panel.SetActive(true);
            endGame_Panel.SetActive(false);
            DoButtonSound();
            Game.Instance.StartGame();
        });

        //cuando hago clik en el boton de Restart
        restart_button.onClick.AddListener(() => {
            endGame_Panel.SetActive(false);
            game_Panel.SetActive(true);
            lobby_Panel.SetActive(false);
            DoButtonSound();
            Game.Instance.StartGame();
        });

        //cuando hago clik en el boton de Go Lobby
        lobby_button.onClick.AddListener(() => {
            lobby_Panel.SetActive(true);
            endGame_Panel.SetActive(false);
            game_Panel.SetActive(false);
            DoButtonSound();
            Game.Instance.GoLobby();
        });
    }
    void DoButtonSound()//Reproduce sonido de boton
    {
        SoundManager.instance.Play("Button");
    }
    private void Start()
    {
        Spike.OnDeathTrigger += () => { endGame_Panel.SetActive(true); game_Panel.SetActive(true); };
        Game.Instance.OnScoreChange += SetScore;
        Game.Instance.OnStartGame += () => game_Panel.SetActive(true); 
    }
    public void SetScore(int s)//actualiza el texto que muestra el score
    {
        score_text.text = s.ToString();
    }

}
