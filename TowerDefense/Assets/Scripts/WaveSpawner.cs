using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public static int EnemiesAlive = 0;

    public Wave[] waves;

    //Punto de aparición de los enemigos.
    public Transform spawnPoint;

    //Tiempo que tarda en aparecer cada enemigo.
    public float timeBetweenWaves = 5f;

    //Contador de tiempo para la oleada.
    private float countdown = 2f;

    //Texto que representara el tiempo que falta para la siguiente oleada de enemigos.
    public Text waveCountdownText;

    //Indice de oleada.
    private int waveIndex = 0;

    public GameManager gameManager;

    private void Start()
    {
        EnemiesAlive = 0;
    }

    void Update () {

        if (EnemiesAlive>0)
        {
            return;
        }

		if (waveIndex == waves.Length)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}

        if (countdown<=0f)
        {
            //Empieza la oleada y reinicia el contador para la siguiente oleada.
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        //Resta tiempo y modifica el texto en el juego.
        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    //Suma una oleada y hace aparecer los enemigos de la oleada indicada.
    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;

    }

    //Hace aparecer un enemigo.
    private void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy , spawnPoint.position, spawnPoint.rotation);
    }
}
