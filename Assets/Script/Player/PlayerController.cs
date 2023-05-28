using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public int Score = 0;
    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void Create()
    {
        Score = 0;

        isJump = false;
        isSlide = false;

        this.transform.position = Vector3.up;
        this.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        Score = (int)(this.transform.position.z / 10);
        OnUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name == "Dead")
        {
            GameController.Instance.OnDead();
        }
    }
}
