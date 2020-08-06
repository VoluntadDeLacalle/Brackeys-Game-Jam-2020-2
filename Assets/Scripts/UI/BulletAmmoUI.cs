using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletAmmoUI : MonoBehaviour
{
    public List<Image> bulletImages = new List<Image>();
    private int ammo = 3;
    private bool reload = false;

    void Update()
    {
        if (ammo > GameManager.instance.currentAmmo)
        {
            bulletImages[ammo - 1].enabled = false;
            ammo--;
        }

        if (ammo == 0)
        {
            reload = false;
        }

        if (GameManager.instance.currentAmmo == GameManager.instance.maxAmmo && !reload)
        {
            for (int i = 0; i < bulletImages.Count; i++)
            {
                bulletImages[i].enabled = true;
            }

            reload = true;
            ammo = GameManager.instance.maxAmmo;
        }
    }

}
