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

public class Projectile : MonoBehaviour
{
    public float MaxStretch = 3.0f;
    public float CircleRadius;
    public bool isLoaded;

    /// Saved references for other scripts
    private FollowProjectile followProjectile;
    private GameReset gameReset;
    private Catapult catapult;

    /// Saved references to components for this object
    private Rigidbody2D rb;
    private SpringJoint2D spring;

    /// Saved references to other objects
    private Rigidbody2D catapultRigidBody;

    private float maxStretchSquared;
    public bool clickedOn;

    private bool isFired = false;
    private Vector2 prevVelocity;

    private void Awake()
    {
        followProjectile = GameObject.FindObjectOfType(typeof(FollowProjectile)) as FollowProjectile;
        gameReset = GameObject.FindObjectOfType(typeof(GameReset)) as GameReset;
        catapult = GameObject.FindObjectOfType(typeof(Catapult)) as Catapult;

        catapultRigidBody = GameObject.Find("/Catapult").GetComponent<Rigidbody2D>();

        spring = GetComponent<SpringJoint2D>();

        CircleRadius = (GetComponent<Collider2D>() as CircleCollider2D).radius;

        maxStretchSquared = MaxStretch * MaxStretch;

        rb = GetComponent<Rigidbody2D>();
        rb.mass = 5.0f;
        rb.drag = 0.15f;
        rb.angularDrag = 15.0f;
        rb.gravityScale = 1.0f;
    }

    void Start()
    {

    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        if (!isFired)
        {
            if (spring != null && isLoaded)
            {
                spring.enabled = false;
                clickedOn = true;
            }
            else
            {
                AttachProjectileToCatapult();
            }
        }
    }

    private void AttachProjectileToCatapult()
    {
        if (!isFired)
        {
            if (catapult.LoadProjectile(rb))
            {
                rb.position = new Vector3(-9.0f, -5.0f, 0.0f);
                catapult.LineRendererUpdate();
                followProjectile.projectile = rb.transform;
                gameReset.projectile = rb;

                spring.connectedBody = catapultRigidBody;
                isLoaded = true;
            }
        }
    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    void OnMouseUp()
    {
        if (isFired)
        {
            return;
        }

        if (isLoaded && clickedOn)
        {
            if (spring != null)
            {
                spring.enabled = true;
                rb.isKinematic = false;
                clickedOn = false;
                isLoaded = false;
                isFired = true;

                catapult.FireProjectile();
            }
        }
    }

    void Update()
    {
        if (spring != null)
        {
            if (clickedOn)
            {
                Dragging();
            }

            if (!rb.isKinematic && prevVelocity.sqrMagnitude > rb.velocity.sqrMagnitude)
            {
                if (spring != null)
                {
                    Destroy(spring);
                }

                rb.velocity = prevVelocity;
            }

            if (!clickedOn)
            {
                prevVelocity = rb.velocity;
            }

            catapult.LineRendererUpdate();
        }
    }

    void Dragging()
    {
        if (!isFired && isLoaded)
        {
            Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 catapultToMouse = (Vector2)mouseWorldPoint - catapultRigidBody.position;

            if (catapultToMouse.sqrMagnitude > maxStretchSquared)
            {
                catapult.rayToMouse.direction = catapultToMouse;
                mouseWorldPoint = catapult.rayToMouse.GetPoint(MaxStretch);
            }

            mouseWorldPoint.z = 0.0f;
            transform.position = mouseWorldPoint;
        }
    }
}
