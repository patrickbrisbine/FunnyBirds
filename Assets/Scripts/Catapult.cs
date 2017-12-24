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

    private LineRenderer catapultLineFront;
    private LineRenderer catapultLineBack;

    void Start()
    {
        catapultLineFront = GameObject.Find("/CatapultFront").GetComponent<LineRenderer>();
        catapultLineBack = GetComponent<LineRenderer>();
    }

    void Update()
    {
		
    }

    public void LoadProjectile(Rigidbody2D projectileToLoad)
    {
        projectile = projectileToLoad;
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
    }

    void LineRendererUpdate()
    {
        Debug.Log("LineRendererUpdate");

        if (projectile == null)
        {
            catapultLineFront.SetPosition(1, new Vector3(-9.35f, -4.86f, 0.0f));
            catapultLineBack.SetPosition(1, new Vector3(-8.35f, -4.76f, 0.0f));
            return;
        }

        //Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        //leftCatapultToProjectile.direction = catapultToProjectile;
        //Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
        //catapultLineFront.SetPosition(1, holdPoint);
        //catapultLineBack.SetPosition(1, holdPoint);
    }

}
