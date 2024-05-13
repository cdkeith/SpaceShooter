using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Projectile" || other.gameObject.tag == "PlayerProjectile")
        {
            other.gameObject.SetActive(false);
        }
    }
}
