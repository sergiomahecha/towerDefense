using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

    private Enemy enemy;

	//El punto de referencia donde giraran lo enemigos.
	private Transform target;

	private int wavepointIndex = 0;

	//Asigna un valor a target.
	void Start()
	{
        enemy=GetComponent<Enemy>();

		target = Waypoints.points[0];
	}

	void Update()
	{
		//Realiza el giro.
		Vector3 dir = target.position - transform.position;
		transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

		if (Vector3.Distance(transform.position, target.position) <= 0.4f)
		{
			GetNextWayPoint();
		}

        enemy.speed = enemy.startSpeed;
	}

	void GetNextWayPoint()
	{
		//Destruye el objeto al llegar al ultimo punto de refenrencia.
		if (wavepointIndex >= Waypoints.points.Length - 1)
		{
			EndPath();
			return;
		}

		//Si no es el ultimo punto de referencia aumenta el indice.
		wavepointIndex++;
		target = Waypoints.points[wavepointIndex];
	}

	void EndPath()
	{
		PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive--;
		Destroy(gameObject);
	}
}
