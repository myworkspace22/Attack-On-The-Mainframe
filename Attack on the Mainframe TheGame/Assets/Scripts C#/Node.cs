using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private Animator anim;
    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBluePrint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    BuildManager buildManager;

    private void Start()
    {
        anim = GetComponent<Animator>();

        buildManager = BuildManager.instance;
    }

    public Vector2 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }
    void BuildTurret(TurretBluePrint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Build more Pylons!");
            return;
        }

        // null check m�ske ikke n�dvendigt
        Collider2D[] canPlaceChecks = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        if (canPlaceChecks != null)
        {
            for (int i = 0; i < canPlaceChecks.Length; i++)
            {
                if (canPlaceChecks[i].gameObject.tag == "Tower")
                {
                    Debug.Log("there is no space for a turret");
                    return;
                }              
            }
        }
        
    
        PlayerStats.Money -= blueprint.cost;

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;

        Debug.Log("Turret build!");

        //move collider forwards to make it easy to select
        Vector3 pos = transform.position;
        pos.z = -0.1f;
        transform.position = pos;

        //make the collider as big as the tower
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(0.5f, 0.5f);

        //add tag "Tower"
        gameObject.tag = "Tower";
    }
    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to Upgrade!");
            return;
        }
        PlayerStats.Money -= turretBlueprint.upgradeCost;

        //Get rid of the old turret
        Destroy(turret);

        //Building a upgraded turret
        GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        isUpgraded = true;

        Debug.Log("Turret Upgraded!");
    }
    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        //Sell effect for later use!

        //move collider back again
        Vector3 pos = transform.position;
        pos.z = 0.0f;
        transform.position = pos;

        //make the collider as small again
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(0.25f, 0.25f);

        //remove tag "Tower"
        gameObject.tag = "Untagged";

        Destroy(turret);
        turretBlueprint = null;
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!buildManager.CanBuild)
        {
            return;
        }
        Collider2D[] canPlaceChecks = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        if (canPlaceChecks != null)
        {
            for (int i = 0; i < canPlaceChecks.Length; i++)
            {
                if (canPlaceChecks[i].gameObject.tag == "Tower")
                {
                    anim.SetBool("Decline", true);
                    return;
                }
            }
        }
        if (buildManager.HasMoney)
        {
            anim.SetBool("Place", true);
        }
        else
        {
            anim.SetBool("Decline", true);
        }
    }
    private void OnMouseExit()
    {
        anim.SetBool("Place", false);
        anim.SetBool("Decline", false);
    }
}
