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
    public float maxStretch = 3.0f;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;

    private FollowProjectile followProjectile;
    private GameReset gameReset;

    private Rigidbody2D rb;
    private SpringJoint2D spring;
    private Transform catapult;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSquared;
    private float circleRadius;
    private bool clickedOn;
    private bool isLoaded;
    private bool isFired = false;
    private Vector2 prevVelocity;

    private void Awake()
    {
        followProjectile = GameObject.FindObjectOfType(typeof(FollowProjectile)) as FollowProjectile;
        gameReset = GameObject.FindObjectOfType(typeof(GameReset)) as GameReset;

        spring = GetComponent<SpringJoint2D>();
        catapult = spring.connectedBody.transform;

        circleRadius = (GetComponent<Collider2D>() as CircleCollider2D).radius;

        maxStretchSquared = maxStretch * maxStretch;

        rb = GetComponent<Rigidbody2D>();
        rb.mass = 5.0f;
        rb.drag = 0.15f;
        rb.angularDrag = 15.0f;
        rb.gravityScale = 1.0f;
    }

    void Start()
    {

    }

    void InitCatapult()
    {
        Debug.Log("Front " + catapultLineFront.isVisible);
        Debug.Log("Back " + catapultLineBack.isVisible);

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

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        Debug.Log(String.Format("OnMouseDown isFired {0}", isFired));

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
                InitCatapult();
            }
        }
    }

    private void AttachProjectileToCatapult()
    {
        Debug.Log(String.Format("AttachProjectileToCatapult isFired {0}", isFired));

        if (!isFired)
        {
            leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
            rb.position = new Vector3(-9.0f, -5.0f, 0.0f);
            followProjectile.projectile = rb.transform;
            gameReset.projectile = rb;
            isLoaded = true;
        }
    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    void OnMouseUp()
    {
        Debug.Log(String.Format("OnMouseUp: isFired {0} isLoaded {1} clickedOn {2} spring {3}", isFired, isLoaded, clickedOn, spring));

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
            }
        }
    }

    void Update()
    {
        Debug.Log(String.Format("Update: spring {0}", spring));

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

            LineRendererUpdate();
        }
        else
        {
            //catapultLineFront.enabled = false;
            //catapultLineBack.enabled = false;
        }
    }

    void LineRendererUpdate()
    {
        Debug.Log(String.Format("LineRendererUpdate: isFired {0} isLoaded {1}", isFired, isLoaded));

        if (catapultLineFront == null || catapultLineBack == null)
        {
            return;
        }

        if (!isLoaded)
        {
            catapultLineFront.SetPosition(1, new Vector3(-9.35f, -4.86f, 0.0f));
            catapultLineBack.SetPosition(1, new Vector3(-8.35f, -4.76f, 0.0f));
            return;
        }

        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }

    void Dragging()
    {
        if (!isFired && isLoaded)
        {
            Debug.Log(String.Format("Dragging: isFired {0} isLoaded {1}", isFired, isLoaded));
            Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

            if (catapultToMouse.sqrMagnitude > maxStretchSquared)
            {
                rayToMouse.direction = catapultToMouse;
                mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
            }

            mouseWorldPoint.z = 0.0f;
            transform.position = mouseWorldPoint;
        }
    }
}
