using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torret : MonoBehaviour
{

    //Indica donde esta un objeto, que posteriormente representara al enemigo más cercano.
    private Transform target;

    //Posición del enemigo más cercano a la torreta.
    private Enemy targetEnemy;

    //Se utiliza para dividir los atributos en unity.
    [Header("General")]

    //Rango de afectación de la torreta.
    public float range = 15f;

    [Header("Use Bullets (Default)")]

    //GameObject que representara la bala que se dispara.
    public GameObject bulletPrefab;

    //Disparos por segundo.
    public float fireRate = 1f;

    //Cuenta atras para realizar un disparo.
    private float fireCountdown = 0f;

    [Header("Use Laser")]

    //Se utiliza para determinar si el arma que se ha seleccionado utiliza laser.
    public bool useLaser = false;

    //Daño por tiempo.
    public int damageOverTime=30;

    //Representa lo que ralentizara al enemigo.
    public float slowAmount = .5f;

    //Es la linea que dispara el laser.
    public LineRenderer lineRenderer;

    //Efecto al chocar el laser con el enemigo.
    public ParticleSystem impactEffect;

    public Light impactLight;

    [Header("Unity Setup Fields")]

    //Nombre del de la etiqueta del gameObject.
    public string enemyTag = "Enemy";

    //Se utilizará este objeto para que la torreta gire siguiendo al enemigo.
    public Transform partToRotate;

    //Velocidad de rotación de la torreta.
    public float turnSpeed = 10f;

    //Punto de aparición de las balas.
    public Transform firePoint;


    // Use this for initialization
    void Start()
    {

        //invoca el metodo que se pasa cada cierto tiempo que tambien se pasa como parámetro.
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        //Este campo representa una cantidad infinita, ira cambiando con la distancia al enemigo más cercano.
        float shortestDistance = Mathf.Infinity;

        //Este campo representara al enemigo más cercano.
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //Da valor a target para que represente al enemigo más cercano que esta dentro del rango de afectación de la torreta.
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        //Si se sale fuera del rango cambia el valor de target a null.
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Si target no vale nada este método no hará nada.
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        } 

        LookOnTarget();

        if (useLaser)
        {
            Laser();
        }else
        {
			if (fireCountdown <= 0f)
			{
				//Ejecuta el metodo de disparo y reinicializa el contador.
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			//Resta tiempo.
			fireCountdown -= Time.deltaTime;
        }

    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime*Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        //Donde empieza la linea.
        lineRenderer.SetPosition(0, firePoint.position);

        //Donde acaba la linea.
        lineRenderer.SetPosition(1, target.position);

        //Dirección que sigue la linea.
        Vector3 dir = firePoint.position - target.position;

        //Donde se crea el efecto del impacto.
        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void LookOnTarget()
    {
        Vector3 dir = target.position - transform.position;

        //Objeto que representara la rotación que seguira la torreta.
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        //Representa el angulo de rotación y la velocidad con la que rota.
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

        //Indica por cada frame la rotación en la que debe estar.
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);


    }

    void Shoot()
    {
        //Clona un objeto(primer campo), y lo genera en un punto(segundo campo) con su rotación(tercer campo).
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        //Instanciamos el objeto Bullet que hemos creado.
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    //Campo de afectación de la torreta.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
