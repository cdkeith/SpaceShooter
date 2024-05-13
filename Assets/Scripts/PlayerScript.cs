using System.Collections;
using UnityEngine;

namespace DesignPatterns.ObjectPool
{
    public class PlayerScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public float speed = 5f;
        public int playerHealth = 3;
        public bool invincible = false;
        private bool death = false;
        public GameObject projectile;
        public GameObject sparks;
        public GameObject explosion;
        public float power = 50.0f;
        private bool delay = false;
        private float timer = 0.1f;
        private Vector2 movementDirection;
        public bool collectionCheck = true;
        public int defaultCapacity = 15;
        public int maxSize = 50;


        // Update is called once per frame
        void Update()
        {
            if (!death)
            {
                movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                movementDirection.Normalize();
                if (Input.GetButton("Fire1") && delay == false)
                {
                    if (projectile)
                    {
                        GameObject newProjectile = ObjectPoolManager.SpawnObject(projectile, transform.position, transform.rotation);


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
        }
        void FixedUpdate()
        {
            if (!death)
            {
                transform.Translate(movementDirection * speed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Projectile")
            {
                if (!invincible)
                {
                    StartCoroutine (ApplyDamage());
                }
            }
        }

        public IEnumerator ApplyDamage () 
        {
            
                playerHealth -= 1;
                invincible = true;
                Instantiate (sparks, transform.position, transform.rotation);
                GameManager.gm.playerHit(playerHealth);
                if (playerHealth <= 0) 
                { 
                    
                    gameOver();
                }
                else
                {

                }
                yield return new WaitForSeconds(2.0f);
                invincible = false;
        }

        
        void gameOver()
        {

            Instantiate (explosion, transform.position, transform.rotation);
            death = true;
            if (GameManager.gm) 
                GameManager.gm.EndGame();
            Destroy (gameObject);
        }
    }
}