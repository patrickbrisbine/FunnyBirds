/* 
 * Peril Beasts Copyright (C) Patrick Brisbine - All Rights Reserved
 *
 * Peril Beasts is licensed under a Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 *
 * You should have received a copy of the license along with this
 * work.  If not, see <http://creativecommons.org/licenses/by-nc-nd/3.0/>.
 *  
 * Written by Patrick Brisbine December 2017
 */

using System;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public Rigidbody2D projectile;
    Projectile projectileScript;

    LineRenderer catapultLineFront;
    LineRenderer catapultLineBack;

    public Ray rayToMouse;
    Ray leftCatapultToProjectile;
    Transform catapult;

    void Start()
    {
        catapultLineFront = GameObject.Find("/Catapult/CatapultFront").GetComponent<LineRenderer>();
        catapultLineBack = GetComponent<LineRenderer>();

        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);

        catapult = transform;

        InitCatapult();
    }

    void Update()
    {

    }

    public bool LoadProjectile(Rigidbody2D projectileToLoad)
    {
        if (projectile == null)
        {
            projectile = projectileToLoad;
            projectileScript = projectile.GetComponent<Projectile>();
            return true;
        }

        return false;
    }

    public void FireProjectile()
    {
        projectile = null;
    }

    void InitCatapult()
    {
        catapultLineFront.enabled = true;
        catapultLineBack.enabled = true;

        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Foreground";

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;

        catapultLineFront.material.color = Color.black;
        catapultLineBack.material.color = Color.black;

        rayToMouse = new Ray(catapult.position, Vector3.zero);
    }

    public void LineRendererUpdate()
    {
        if (catapultLineFront == null || catapultLineBack == null)
        {
            return;
        }

        if (projectile == null)
        {
            catapultLineFront.SetPosition(0, new Vector3(-8.55f, -5.82f, 0.0f));
            catapultLineFront.SetPosition(1, new Vector3(-9.4f, -4.77f, 0.0f));

            catapultLineBack.SetPosition(0, new Vector3(-8.55f, -5.82f, 0.0f));
            catapultLineBack.SetPosition(1, new Vector3(-8.35f, -4.76f, 0.0f));

            return;
        }

        if (projectile.transform.position.y.Equals(-7.6f))
        {
            return;
        }

        Vector2 catapultToProjectile = projectile.transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + projectileScript.CircleRadius);

        catapultLineFront.SetPosition(0, new Vector3(-9.35f, -4.86f, 0.0f));
        catapultLineFront.SetPosition(1, holdPoint);

        catapultLineBack.SetPosition(0, new Vector3(-8.35f, -4.76f, 0.0f));
        catapultLineBack.SetPosition(1, holdPoint);

        // Set the default position of the line going backwards
        if (!projectileScript.clickedOn)
        {
            catapultLineFront.SetPosition(1, new Vector3(-9.50f, -5.35f, 0.0f));
        }
    }
}
