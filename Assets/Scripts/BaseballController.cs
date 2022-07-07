using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballController : MonoBehaviour
{
    public float speed;
    Rigidbody2D baseballRigidbody;
    public float damage;
    

    void Awake()
    {
        baseballRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        baseballRigidbody.AddForce(transform.up * speed);
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Damage(damage);
            Invoke("Disable", 0.001f);
        }

        if (collision.gameObject.CompareTag("Wall"))
            Invoke("Disable", 0.001f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
