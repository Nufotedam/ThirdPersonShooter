using UnityEngine;
using UnityEngine.UI;

public class AmmoInfo : MonoBehaviour
{
    public Text weaponText;
    public Text ammoText;

    public void AmmoUIUpdate(string weapon, int ammo, int currentAmmo)
    {
        weaponText.text = weapon;
        ammoText.text = ammo.ToString() + " / " + currentAmmo.ToString();
    }
}
