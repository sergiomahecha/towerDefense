using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    //Velocidad inicial.
    public float startSpeed = 10f;

    //Velocidad con la que se mueve el enemigo//HideInInspector oculta el campo en Unity para evitar que se modifique.
    [HideInInspector]
    public float speed;

    public float startHealth = 100;

    //Salud que tiene el enemigo.
    private float health = 100;

    //Dinero que se gana al destruir a un enemigo.
    public int worth = 50;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

    private bool isDead = false;

    private void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if (health<=0&&!isDead)Die();
    }

    //Este método reduce la velocidad de movimiento.
    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    //Se suma el dinero por destruir al enemigo, se crea el efecto y se destruye al enemigo.
    private void Die()
    {
        isDead = true;

        PlayerStats.Money += worth;

        GameObject effect=(GameObject) Instantiate(deathEffect,transform.position,Quaternion.identity);
        Destroy(effect, 5f);

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }
}
