using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    public GameObject projectile;
    public float power = 50.0f;
    public float activateScore = 60;
    SpriteRenderer sprite;
    private bool delay = false;
    private float timer = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.score >= activateScore)
        {
            sprite.enabled = true;
            if (Input.GetButton("Fire1") && delay == false)
            {
                if (projectile)
                {
                    GameObject newProjectile = ObjectPoolManager.SpawnObject(projectile, transform.position, transform.rotation);

                    if (!newProjectile.GetComponent<Rigidbody2D>()) 
                    {
                        newProjectile.AddComponent<Rigidbody2D>();
                    }

                    newProjectile.GetComponent<Rigidbody2D>().AddForce(transform.up * power, ForceMode2D.Impulse);
                    delay = true;
                    timer = 0.1f;
                }
            }
            timer -= Time.deltaTime;
            if(timer <= 0.0f)
            {
                delay = false;
            }
        }
        if (GameManager.gm.gameIsOver == true)
        {
            sprite.enabled = false;
            timer = 200.0f;
        }
    }
}
