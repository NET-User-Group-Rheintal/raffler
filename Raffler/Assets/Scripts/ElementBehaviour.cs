using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ElementBehaviour : MonoBehaviour
{
    public SpriteRenderer sprite;
    public AudioSource audio;

    private Vector3 startSize = new Vector3(0.1f, 0.1f, 0.1f);
    private float fadeTime = 0.3f;
    private float growTime = 0.5f;
    private float startTime;

    void Start()
    {
        sprite.color = Color.clear;
        sprite.transform.localScale = startSize;
        startTime = Time.time;
    }

    void Update()
    {
        if (startTime <= 0f)
        {
            return;
        }

        var changed = false;
        var delta = Time.time - startTime;

        if (delta > fadeTime)
        {
            sprite.color = Color.white;
        }
        else
        {
            sprite.color = Color.Lerp(Color.clear, Color.white, delta);

            changed = true;
        }

        if (delta > growTime)
        {
            sprite.transform.localScale = Vector3.one;
        }
        else
        {
            sprite.transform.localScale = Vector3.Lerp(startSize, Vector3.one, delta);
            changed = true;
        }

        if (!changed)
        {
            startTime = -1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Alien"))
        {
            return;
        }


        var audioVolume = (Math.Clamp(other.relativeVelocity.magnitude, 0.5f, 15f) / 15f) * 0.4f;
        if (audioVolume <= 0.1f)
        {
            return;
        }
        
        audio.volume = audioVolume;
        audio.Play();
    }
}