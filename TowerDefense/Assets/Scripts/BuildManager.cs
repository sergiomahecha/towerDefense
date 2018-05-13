using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    //Un singleton que se utilizara para todos los nodos.
    public static BuildManager instance;

    //La torreta que se construira.
    private TurretBluePrint turretToBuild;

    private Node selectedNode;

    public GameObject buildEffect;

    public GameObject sellEffect;

    public NodeUI nodeUI;

    //este método se utilizara para determinar si hay más de un BuildManager.
    void Awake()
    {
        if (instance!=null)
        {
            Debug.Log("Más de un BuildManager en la escena!");
            return;
        }

        instance = this;
    }

    //Propiedada que comprueba si se ha seleccionado algun arma.
    public bool CanBuild{ get { return turretToBuild != null; }}

    //Indica si hay suficiente dinero para comprar la torreta.
    public bool HasMoney { get { return PlayerStats.Money>=turretToBuild.cost; } }

    public void SelectTurretToBuild(TurretBluePrint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    public void SelectNode(Node node)
    {
        if (selectedNode==node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public TurretBluePrint GetTurretToBuild()
    {
        return turretToBuild;
    }

}
