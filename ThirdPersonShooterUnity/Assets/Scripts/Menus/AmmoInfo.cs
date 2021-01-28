using UnityEngine;
using UnityEngine.UI;

public class AmmoInfo : MonoBehaviour
{
    public Text weaponText;             //  Active weapon
    public Text ammoText;               //  Ammo quantity of thw active weapon

    public void AmmoUIUpdate(string weapon, int ammo, int currentAmmo)
    {
        //  Update the ammo quatity of the weapon and shows how many bullets in total the player has
        weaponText.text = weapon;
        ammoText.text = ammo.ToString() + " / " + currentAmmo.ToString();
    }
}
