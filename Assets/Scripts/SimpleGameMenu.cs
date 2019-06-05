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
    [SerializeField] TextMeshProUGUI bestScore_text;
    [SerializeField] TextMeshProUGUI lobbybestScore_text;
    [SerializeField] GameObject lobby_Panel;
    [SerializeField] GameObject game_Panel;
    [SerializeField] GameObject endGame_Panel;
    [SerializeField] new Camera camera;


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
        Game.Instance.OnEndGame += SetBestScore;
    }
    public void SetScore(int s)//actualiza el texto que muestra el score
    {
        score_text.text = s.ToString();
        SetColor();
    }
    public void SetColor()
    {
        Color color_1 = Color.Lerp(Game.Instance.Color, Color.white, 0.3f);
        Color color_2 = Color.Lerp(Game.Instance.Color, Color.black, 0.3f);

        camera.backgroundColor = color_1;
        game_Panel.transform.Find("Image").GetComponent<Image>().color = color_2;
        score_text.color = color_1;
    }
    public void SetBestScore()
    {
        bestScore_text.text = Game.Instance.BestScore.ToString();
        lobbybestScore_text.text = Game.Instance.BestScore.ToString();
    }
}
