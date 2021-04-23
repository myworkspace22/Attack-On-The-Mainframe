using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;

    public TextMeshProUGUI title;

    public TextMeshProUGUI description;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI range;
    public TextMeshProUGUI firerate;
    public TextMeshProUGUI upgradeDescription;

    public TextMeshProUGUI upgradeCost1;
    public TextMeshProUGUI upgradeCost2;
    public TextMeshProUGUI sellAmount;

    private Node target;
    public Button upgradeButton1;
    public Button upgradeButton2;

    public void SetTarget(Node _target)
    {
        target = _target;

        //transform.position = target.GetBuildPosition();

        if (!target.isMaxed)
        {

            int multiplyer = (target.upgradeNr > 0) ? 2 : 1;

            upgradeDescription.text = "Upgrades: <color=#00FF00>" + target.turretBlueprint.upgadeDescription[target.upgradeNr + 1 * multiplyer - 1] + " OR " + target.turretBlueprint.upgadeDescription[target.upgradeNr + 2 * multiplyer -1] + "</color>";
            upgradeCost1.text = target.turretBlueprint.upgadeDescription[target.upgradeNr + 1 * multiplyer - 1] + ": <color=#FFD500>$" + target.turretBlueprint.upgradeCost[target.upgradeNr + 1 * multiplyer - 1] + "</color>";
            upgradeCost2.text = target.turretBlueprint.upgadeDescription[target.upgradeNr + 2 * multiplyer - 1] + ": <color=#FFD500>$" + target.turretBlueprint.upgradeCost[target.upgradeNr + 2 * multiplyer - 1] + "</color>";
            upgradeButton1.interactable = true;
            upgradeButton2.interactable = true;
        }
        else
        {

            upgradeDescription.text = "<color=#00FF00>UPGRADED</color>";
            upgradeCost1.text = "MAXED";
            upgradeCost2.text = "MAXED";
            upgradeButton1.interactable = false;
            upgradeButton2.interactable = false;
        }

        title.text = target.turretBlueprint.title;
        description.text = target.turretBlueprint.description;
        damage.text = "DAMAGE: " + target.turret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage; //+ " -> <b><color=#00FF00>" + target.turretBlueprint.upgradedPrefab.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage + "</color></b>";
        range.text = "RANGE: " + target.turret.GetComponent<Turret>().range; //+ " -> <b><color=#00FF00>" + target.turretBlueprint.upgradedPrefab.GetComponent<Turret>().range + "</color></b>";
        firerate.text = "Firerate: " + target.turret.GetComponent<Turret>().fireRate; //+ " -> <b><color=#00FF00>" + target.turretBlueprint.upgradedPrefab.GetComponent<Turret>().fireRate + "</color></b>";

        sellAmount.text = "SELL: <color=#FFD500>$" + target.turretBlueprint.GetSellAmount() + "</color>";

        ui.SetActive(true);
    }


    public void ShowUpgradeStats(int upgradeIndex)
    {
        if (target.isMaxed)
            return;

        int multiplyer = (target.upgradeNr > 0) ? 2 : 1;
        damage.text = "DAMAGE: " + target.turret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage + " -> <b><color=#00FF00>" + target.turretBlueprint.upgradedPrefab[target.upgradeNr + upgradeIndex * multiplyer - 1].GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage + "</color></b>";
        range.text = "RANGE: " + target.turret.GetComponent<Turret>().range +" -> <b><color=#00FF00>" + target.turretBlueprint.upgradedPrefab[target.upgradeNr + upgradeIndex * multiplyer - 1].GetComponent<Turret>().range + "</color></b>";
        firerate.text = "Firerate: " + target.turret.GetComponent<Turret>().fireRate + " -> <b><color=#00FF00>" + target.turretBlueprint.upgradedPrefab[target.upgradeNr + upgradeIndex * multiplyer - 1].GetComponent<Turret>().fireRate + "</color></b>";
    }
    public void HideUpgradeStats()
    {
        if (target.isMaxed)
            return;

        damage.text = "DAMAGE: " + target.turret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage;
        range.text = "RANGE: " + target.turret.GetComponent<Turret>().range;
        firerate.text = "Firerate: " + target.turret.GetComponent<Turret>().fireRate;
    }

    public void Hide()
    {
        ui.SetActive(false);
    }
    public void Upgrade(int upgradeIndex)
    {
        target.UpgradeTurret(upgradeIndex);
        SetTarget(target);

        //BuildManager.instance.DeselectNode();

        //upgradeCost1.text = "DONE";
        //upgradeCost2.text = "DONE";
        //upgradeButton1.interactable = false;
        //upgradeButton2.interactable = false;
    }

    public void LevelUp()
    {
        target.levelUpTower();
    }
    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
        target.isMaxed = false;
    }
}
