using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpikeController : MonoBehaviour
{
    public float attackRange;
    public enum enemystates {attack}
    public enemystates currentState = enemystates.attack;
    protected Rigidbody2D enemyRigidbody;

    public LayerMask wallLayer;
    public float rayLength;


    protected SpriteRenderer rend;

    protected float distance;
    protected PlayerController player;

    public float timeBetweenAttacks;
    protected float attackCools;

    protected Animator anim;

    // Start is called before the first frame update
    private void OnEnable()
    {
        attackCools = timeBetweenAttacks;
    }



    void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
    }


    public virtual void Attack() {}


    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case enemystates.attack:
            Attack();
            break;
        }

        if (attackCools > 0) attackCools -= Time.deltaTime;
    }
}
