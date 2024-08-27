using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Death : MonoBehaviour
{
    public static Action OnPlayerDeath; //to invoke to inform other scripts that the player has died

    [SerializeField] private float animationTime = 1.5f; // animation upon death, this determines the duration
    private float curTime = 0f; //  current time 
    private bool isDying = false; // Prevents multiple deaths
    void Update()
    {
        if (gameObject.transform.position.y <= 0 && !isDying) // the player falls below y=0, trigger death
        {
            isDying = true;
            StartCoroutine(PlayerDeathAnimationCoroutine());
        }
    }

    IEnumerator PlayerDeathAnimationCoroutine()
    {
        //  death event 
        OnPlayerDeath?.Invoke(); // nmotify other scripts like CameraMovement that the player is dying

        Vector3 curSize = gameObject.transform.localScale; //  player's scale
        Vector3 deathSize = Vector3.zero; // Final scale 

        while (curTime <= animationTime) 
        {
            //interpolate between current size and zero size, gradually starting from 0 till 1
            gameObject.transform.localScale = Vector3.Lerp(curSize, deathSize, curTime / animationTime);

            curTime += Time.deltaTime; //new current  time
            yield return null;
        }

        Destroy(gameObject); // kill the player object upon death
    }
}


