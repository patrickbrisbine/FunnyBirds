﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDragging : MonoBehaviour
{
    public float maxStretch = 3.0f;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;

    private Rigidbody2D rb;
    private SpringJoint2D spring;
    private Transform catapult;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSquared;
    private float circleRadius;
    private bool clickedOn;
    private Vector2 prevVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spring = GetComponent<SpringJoint2D>();
        catapult = spring.connectedBody.transform;
    }

    void Start()
    {
        LineRendererSetup();
        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSquared = maxStretch * maxStretch;

        CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
        circleRadius = circle.radius;
    }

    void LineRendererSetup()
    {
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Foreground";

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;

        catapultLineFront.material.color = Color.black;
        catapultLineBack.material.color = Color.black;
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    void OnMouseUp()
    {
        spring.enabled = true;
        rb.isKinematic = false;
        clickedOn = false;
    }

    void Update()
    {
        if (clickedOn)
        {
            Dragging();
        }

        if (spring != null)
        {
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
            catapultLineFront.enabled = false;
            catapultLineBack.enabled = false;
        }
    }

    void LineRendererUpdate()
    {
        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

        if (catapultToMouse.sqrMagnitude > maxStretchSquared)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }

        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }
}
