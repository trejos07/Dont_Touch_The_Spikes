using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Game : MonoBehaviour
{
    public static Game Instance;
    private bool endRace = false;
    int score = 0;

    public delegate void Action();
    public delegate void IntAction(int i);
    public event Action OnStartGame;
    public event Action OnLobby;
    public event Action OnEndGame;
    public event IntAction OnScoreChange;

    //listas de los posibles Spikes por cada lado
    [SerializeField] List<HidenSpike> lefthSpikes;
    [SerializeField] List<HidenSpike> rigthSpikes;


    #region Accesores
    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            if(score != value)
            {
                score = value;
                if (OnScoreChange != null)
                {
                    OnScoreChange(value);
                }
            }
        }
    }
    public bool EndRace
    {
        get
        {
            return endRace;
        }

        set
        {
            endRace = value;
        }
    }
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        SetUpEvents();
    }

    public void GoLobby()//Cuando voy al lobby
    {
        OnLobby?.Invoke();
        HideSpikeWall(lefthSpikes);
        HideSpikeWall(rigthSpikes);
    }
    public void StartGame()//cuando el juego comienza
    {
        Score = 0;
        EndRace = false;
        HideSpikeWall(lefthSpikes);
        HideSpikeWall(rigthSpikes);
        OnStartGame?.Invoke();
        SoundManager.instance.Play("Start");
    }
    public void EndGame()//cuando el juego acaba 
    {
        EndRace = true;
        OnEndGame?.Invoke();
    }

    public void SetUpEvents()//suscribe los eventos iniciales
    {
        Spike.OnDeathTrigger += EndGame;//cuando se pincha el juego se acaba 
        Wall.OnPlayerHitWall += UpScore;// cada vez que toca una pared aumenta el score
        Wall.OnPlayerHitWall += SetSpikeWalls;//  cada vez que toca una pared salen pinchos en la otra
    }
    public void UpScore()//aumenta el score
    {
        if (!endRace)
            Score++;
    }
    public void SetSpikeWalls()//establece los pinchos en las paredes teniendo en cuenta el puntaje
    {
        if(!endRace)
        {
            int ranBase = 3 + (int)(score / 20);
            int nSpikes = Random.Range(ranBase - 1, ranBase + 2);

            if (Player.Instance.Dir == 1)
            {
                HideSpikeWall(lefthSpikes);
                SpawnRandomSpikes(rigthSpikes, nSpikes);
            }
            else
            {
                HideSpikeWall(rigthSpikes);
                SpawnRandomSpikes(lefthSpikes, nSpikes);
            }
        }
    }
    public void SpawnRandomSpikes(List<HidenSpike> spikes, int n )//seleciona Spikes de forma aleatoria en una lista y los activa
    {
        while(n>0)
        {
            int i = Random.Range(0, spikes.Count);
            spikes[i].IsHide = false;
            n--;

        }

    }
    public void HideSpikeWall(List<HidenSpike> spikes)//oculta todos los Spikes de una lista
    {
        for (int i = 0; i < spikes.Count; i++)
        {
            spikes[i].IsHide = true;
        }
    }

}
