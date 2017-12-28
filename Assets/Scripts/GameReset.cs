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
using System.Collections;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour
{
    public Rigidbody2D projectile;
    public float resetSpeed = 0.01f;
    FollowProjectile followProjectile;
    float resetSpeedSquared;
    SpringJoint2D spring;

    void Awake()
    {
        projectile = null;

        followProjectile = GameObject.FindObjectOfType(typeof(FollowProjectile)) as FollowProjectile;
        resetSpeedSquared = resetSpeed * resetSpeed;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(NextLaunch());
        }

        if (projectile != null)
        {
            spring = projectile.GetComponent<SpringJoint2D>();

            if (spring == null && projectile.velocity.sqrMagnitude < resetSpeedSquared)
            {
                
                StartCoroutine(NextLaunch(1.0f));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == projectile)
        {
            projectile = null;
            StartCoroutine(NextLaunch());
        }
    }

    IEnumerator NextLaunch(float wait = 0.0f)
    {
        yield return new WaitForSeconds(wait);

        followProjectile.ResetCamera();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
