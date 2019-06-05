using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private static Player instance;

    [SerializeField] private float jumForce = 2;
    [SerializeField] private float sideSpeed =2;
    [SerializeField] private float wallKnockBack = 2;
    [SerializeField] private Vector2 maxSpeed;

    private int dir = 1;
    private bool tap = false;
    private bool alive = true;

    private new Rigidbody2D rigidbody;
    private new SpriteRenderer renderer;
    private EchoEffect echo;
    private Color basic;

    public event Action OnChangeDir;

    #region Accesores
    public static Player Instance { get => instance; set => instance = value; }
    public bool Alive { get => alive; set { alive = value; renderer.color = value ? basic : Color.black; } }
    public int Dir { get => dir; set {
            dir = value;
            OnChangeDir?.Invoke();
        } }
    #endregion

    private void Awake()
    {
        if (Instance == null)//establesco el Singelton
            Instance = this;
        else
            Destroy(gameObject);

        //Obtengo componentes 
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        echo = GetComponent<EchoEffect>();
        basic = renderer.color;
        OnLobby();// el jugador comienza en el lobby
    }
    private void Start()
    {
        Game.Instance.OnStartGame += OnStartGame;//me susbribo al evento de comienzo del juego
        Game.Instance.OnLobby += OnLobby;// me suscribo al evento cuando el jugador quiere ir al lobby 
    }
    private void FixedUpdate()
    {
        if(Alive)//si estoy vivo
        {
            
            if (tap) // si hice tap
            {
                SoundManager.instance.Play("Tap");//reproducir sonido de tap
                Jump(jumForce); //salto y reinicio la variable
                tap = false;
            }
            ClampVelocity();// controlo la maxima velocidad del jugador
        }
    }
    private void Update()
    {
        if (Alive)
            GetImputs();// cheque los inputs
    }
    private void OnCollisionEnter2D(Collision2D collision)//si choque
    {
        if(Alive)//si estoy vivo
        {
            rigidbody.velocity = Vector2.zero;
            ChangeDir();//cambio de direcion al chocar
            Jump(jumForce / 2);
            rigidbody.AddForce(Vector3.right * Dir * wallKnockBack * Time.deltaTime, ForceMode2D.Impulse);//agrego un impulso extra
            SoundManager.instance.Play("Wall"); //reprodusco sonido de golpe contra la pared
        }
        
    }


    public void OnLobby()//cuando el jugador va al lobby
    {
        echo.Active = false;
        Alive = true;
        Dir = 1;
        rigidbody.simulated = false;
        transform.position = Vector2.zero;
    }
    public void OnStartGame()//cuando el juego comienza
    {
        if(!Alive)
            Alive = true;

        Dir = 1;
        rigidbody.simulated = true;
        rigidbody.position = Vector2.zero;
        Spike.OnDeathTrigger += Death;
        Jump(jumForce / 2);
        echo.Active = true;
    }
    public void GetImputs()//obtiene los inputs dependiendo del dispositivo
    {
        #region PcInputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            //starTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            tap = false;
        }
        #endregion
        #region MobileInputs
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
            }
            //else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            //{
            //    //tap = false;
            //}
        }
        #endregion
    }
    public void ChangeDir()//cambia de direcion
    {
        Dir *= -1;
    }
    public void Jump(float f)//agrega una fuerza hacia ariiba y hacia la direcion que va el jugador
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(Vector2.up * f * Time.fixedDeltaTime, ForceMode2D.Impulse);
        rigidbody.AddForce(Vector2.right * Dir * sideSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    public void ClampVelocity()//controla la velocidad maxima y minima del jugador
    {
        Vector2 v = rigidbody.velocity;
        v.x = Mathf.Clamp(v.x,-maxSpeed.x,maxSpeed.x);
        v.y = Mathf.Clamp(v.y,-Mathf.Infinity,maxSpeed.y);
        rigidbody.velocity = v;
    }
    public void Death()//cuando el jugador muere
    {
        Jump(jumForce / 2);
        Alive = false;
        echo.Active = false;
        Spike.OnDeathTrigger -= Death;
        SoundManager.instance.Play("Death");
    }
}
