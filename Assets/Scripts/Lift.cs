using UnityEngine;

public class Lift : MonoBehaviour
{
    public const string TAG = "Lift";

    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Started()
    {
        anim.SetBool("isMoving", true);
    }

    void Stopped()
    {
        anim.SetBool("isMoving", false);
    }
}
