using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour
{
    public Rigidbody2D projectile;
    public float resetSpeed = 0.025f;
    private float resetSpeedSquared;
    private SpringJoint2D spring;

    void Start()
    {
        resetSpeedSquared = resetSpeed * resetSpeed;
        spring = projectile.GetComponent<SpringJoint2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if (spring == null && projectile.velocity.sqrMagnitude < resetSpeedSquared)
        {
            Reset();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == projectile)
        {
            Reset();
        }
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
