using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

    //Color que utilizaremos para pintar el suelo al clickarlo.
    public Color hoverColor;

    //Objeto renderer que representará el nodo sobre el cual estamos operando.
    private Renderer rend;

    //Representará el color inicial del nodo.
    private Color startColor;

    [HideInInspector]
    //GameObject que representara la torreta.
    public GameObject turret;

    [HideInInspector]
    public TurretBluePrint turretBlueprint;

    [HideInInspector]
    public bool isUpgraded = false;

    //posicion para que lo torreta este a la altura correcta.
    public Vector3 positionOffset;

    //Al pulsar en un nodo se utilizara el buildManager para elegir el arma.
    BuildManager buildManager;

    //Color del que se pintara el arma en el menu cuando no hay dinero suficiente.
    public Color notEnoughMoneyColor;

    void Start()
    {
        rend = GetComponent<Renderer>();

        //Se asigna el color inicial.
        startColor = rend.material.color;
        //Se llama a instance ya que el buildManager es un singleton.
        buildManager = BuildManager.instance;
    }

    //Lo que pasará cuando se hace click sobre el nodo.
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //Si no hay torreta seleccionada no hace nada.
        if (turret!=null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        BuildTurret(buildManager.GetTurretToBuild());

    }

    void BuildTurret(TurretBluePrint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost) return;

        PlayerStats.Money -= blueprint.cost;
		//Instancia la torreta.
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);

		//Da valor al campo turret de la clase nodo. para que no se puedan crear mas armas en el mismo nodo.
		turret = _turret;

        turretBlueprint = blueprint;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);

		//Destruye el efecto que se ha creado tras 5 segundos.
		Destroy(effect, 5f);
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost) return;

		PlayerStats.Money -= turretBlueprint.upgradeCost;

        //Destruye la torreta actual para crear la mejorada.
        Destroy(turret);

		//Instancia la torreta.
		GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);

		//Da valor al campo turret de la clase nodo. para que no se puedan crear mas armas en el mismo nodo.
		turret = _turret;

		GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);

		//Destruye el efecto que se ha creado tras 5 segundos.
		Destroy(effect, 5f);

        isUpgraded = true;
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();
        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(turret);
        turretBlueprint = null;
    }

    //Lo que pasa cuando pasamos sobre el nodo.
    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //Si no hay una torreta seleccionada el nodo no destacara y tampoco se podra construir la torreta.
        if (!buildManager.CanBuild) return;

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        }else
        {
            rend.material.color = notEnoughMoneyColor;
        }

    }

    //Lo que pasa cuando dejamos pasar sobre el nodo (Cambiara al color inicial).
    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    //Se utilizara para sumar la altura necesaria para que la torreta no se cree dentro del nodo.
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }
}
