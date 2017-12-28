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

using UnityEngine;

public class DamageTarget : MonoBehaviour
{
    public int HitPoints = 2;
    public Sprite DamagedSprite;
    public float DamageImpactSpeed;

    int currentHitPoints;
    float damageImpactSpeedSquared;
    SpriteRenderer spriteRenderer;

    Transform damageTarget;
    GameObject explosion;
    SpriteRenderer sprite;
    Animator animator;

    bool isExploding;
    bool isKilled;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHitPoints = HitPoints;
        damageImpactSpeedSquared = DamageImpactSpeed * DamageImpactSpeed;

        damageTarget = transform;
        explosion = damageTarget.Find("Explosion").gameObject;
        sprite = explosion.GetComponent<SpriteRenderer>();
        animator = explosion.GetComponent<Animator>();

        sprite.enabled = false;
        //animator.enabled = false;
    }

    // Use this for initialization
    void Start()
    {

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

        currentHitPoints--;

        if (currentHitPoints < 1)
        {
            isExploding = true;
            isKilled = true;
        }
        else if (currentHitPoints < HitPoints)
        {
            spriteRenderer.sprite = DamagedSprite;
        }
    }

    void ExplodeOnce()
    {
        isExploding = false;
        sprite.enabled = true;
        animator.enabled = true;

        //animator.Play("Explosion", 0);

        animator.SetBool(animator.GetParameter(0).name, true);
    }

    void Kill()
    {
        //animator.SetBool(animator.GetParameter(0).name, false);
        sprite.enabled = false;
        //animator.enabled = false;

        spriteRenderer.enabled = false;
        spriteRenderer.GetComponent<Collider2D>().enabled = false;
        spriteRenderer.GetComponent<Rigidbody2D>().isKinematic = false;

        isKilled = false;
        Destroy(transform.GetComponent<GameObject>(), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isExploding)
        {
            ExplodeOnce();
        }
        else if (!isExploding && isKilled)
        {
            Kill();
        }
    }
}
