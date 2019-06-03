using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HidenSpike: Spike
{
    [SerializeField] Transform hide, unhide;

    private bool isHide = true;
    private new Rigidbody2D rigidbody;

    public bool IsHide { get => isHide; set { isHide = value; Move(); } }//establece si esta escondido o no el Spìke y lo mueve a su logar

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        Move();
    }
    public void Move()//mueve los pinchos a su puesto corespondiente teniendo en cuenta si esta oculto o no
    {
        rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        if (isHide)
            rigidbody.MovePosition(hide.position);
        else
            rigidbody.MovePosition(unhide.position);
    }

}
