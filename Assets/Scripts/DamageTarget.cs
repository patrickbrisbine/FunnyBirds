using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTarget : MonoBehaviour
{
    public int hitPoints = 2;
    private int currentHitPoints;
    public Sprite damagedSprite;
    public float damageImpactSpeed;
    private float damageImpactSpeedSquared;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHitPoints = hitPoints;
        damageImpactSpeedSquared = damageImpactSpeed * damageImpactSpeed;
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Damager" || other.relativeVelocity.sqrMagnitude < damageImpactSpeedSquared)
        {
            return;
        }

		spriteRenderer.sprite = damagedSprite;
		currentHitPoints --;

		if ( currentHitPoints < 1)
		{
			Kill();
		}
    }

	void Kill()
	{
		spriteRenderer.enabled = false;
		spriteRenderer.GetComponent<Collider2D>().enabled = false;
		spriteRenderer.GetComponent<Rigidbody2D>().isKinematic = false;
	}

    // Update is called once per frame
    void Update()
    {

    }
}
