using System;
using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject projectile;
    public GameObject explosion;
    public int health = 2;
    public int scoreAmount = 1;
    public float projectileMoveSpeed;
    public int burstCount;
    public int projectilesPerBurst;
    [Range(0, 359)] public float angleSpread;
    public float startingDistance = 0.1f;
    public float timeBetweenBursts;
    public float restTime = 1.0f;
    public bool followThroughBursts = false;
    public bool stagger;
    public bool oscillate;
    private bool isShooting = false;
    void Update()
    {   
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }
    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0.0f;
        ConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;
        }

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate)
            {
                if(followThroughBursts)
                {
                    ConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
                }
            }

            if (oscillate && i % 2 != 1 && followThroughBursts) 
            {
                ConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1.0f;
            }
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindProjectileSpawnPos(currentAngle);
                GameObject newProjectile = ObjectPoolManager.SpawnObject(projectile, pos, Quaternion.identity);
                newProjectile.transform.right = newProjectile.transform.position - transform.position;

                if (newProjectile.TryGetComponent(out EnemyProjectile enemyProjectile))
                {
                    enemyProjectile.UpdateMoveSpeed(projectileMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) 
                { 
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }

            currentAngle = startAngle;
            yield return new WaitForSeconds(timeBetweenBursts);
        }
        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void ConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = this.transform.InverseTransformPoint(GameObject.Find("Player").transform.position);
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0.0f;
        angleStep = 0.0f;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindProjectileSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector2 pos = new Vector2(x, y);
        return pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerProjectile")
        {
            health -= 1;
            other.gameObject.SetActive(false);
            if (health <= 0)
            {
                Instantiate (explosion, transform.position, transform.rotation);
                gameObject.SetActive(false);
                GameManager.gm.enemyHit(scoreAmount);
            }
        }
    }
}
