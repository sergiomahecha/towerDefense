using UnityEngine;

public class Shop : MonoBehaviour {

    public TurretBluePrint standardTurret;
    public TurretBluePrint missileLauncher;
    public TurretBluePrint laserBeamer;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    //Método que se utilizara para comprar torretas.
    public void SelectStandardTurret()
    {
        Debug.Log("Comprando");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
	{
        buildManager.SelectTurretToBuild(missileLauncher);
	}

	public void SelectLaserBeamer()
	{
        buildManager.SelectTurretToBuild(laserBeamer);
	}
}
