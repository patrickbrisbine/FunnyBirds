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
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProjectile : MonoBehaviour
{
    public Transform farLeft;
    public Transform farRight;

    public Transform projectile;

    private Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;
        projectile = null;
    }

    void Update()
    {
        if (projectile != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = projectile.position.x;
            newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
            transform.position = newPosition;
        }
    }

    public void ResetCamera()
    {
        transform.position = initialPosition;
        projectile = null;
    }
}
