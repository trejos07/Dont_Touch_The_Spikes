using UnityEngine;

public class EchoEffect : MonoBehaviour
{

    [SerializeField] float timeBtwnSpawn;
    [SerializeField] float time;
    [SerializeField] GameObject echo;


    private bool active;

    public bool Active {
        get => active;
        set {
            active = value;
            if (value)
                InvokeRepeating("Spawn", timeBtwnSpawn, timeBtwnSpawn);
            else
                CancelInvoke("Spawn");
        } }

    public void Spawn()
    {
        GameObject e = Instantiate(echo, transform.position, Quaternion.identity);
        Destroy(e, time);
        
    }
    
}