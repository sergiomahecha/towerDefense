using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    //Este objeto luego representara al enemigo.
    private Transform target;

    //Velocidad con la que se movera la bala.
    public float speed = 70f;

    //Al golpear al enemigo se crea un efecto.
    public GameObject impactEffect;

    //Radio de explosión.
    public float explosionRadius = 0f;

    //Daño que hace la bala.
    public int damage=50;

    //Se le dara valor a target.
    public void Seek(Transform _target)
    {
        target = _target;
    }
	
	// Update is called once per frame
	void Update () {
        if (target==null)
        {
            Destroy(gameObject);
            return;
        }

        //Representará la dirección que seguira la bala.
        Vector3 dir = target.position - transform.position;

        //Distancia que se recorrerá en un frame.
        float distanceThisFrame = speed * Time.deltaTime;

        //Si se golpea al enemigo.
        if (dir.magnitude<=distanceThisFrame)
        {
            HitTarget();
            return;
        }

        //Mueve la bala hacia el enemigo.
        transform.Translate(dir.normalized*distanceThisFrame, Space.World);

        //Se utilizará para que el misil rote hacia el enemigi con un efecto realista.
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        //Crea el efecto del impacto.
        GameObject effectIns=(GameObject)Instantiate(impactEffect, transform.position, transform.rotation);

        //Destruye el efecto despues de dos segundos.
        Destroy(effectIns, 2f);

        if (explosionRadius>0f)
        {
            Explode();
        }else
        {
            Damage(target);
        }

        //Destruye la bala.
        Destroy(gameObject);
    }

    //Destruye a los enemigos colindantes.
    void Explode()
    {
        Collider[] colliders=Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.tag=="Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        //Esto nos devuelve el objeto Enemy desde el Transform
        Enemy e =enemy.GetComponent<Enemy>();

        if (e!=null)e.TakeDamage(damage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
