using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class NumberBehaviour : MonoBehaviour
    {
        public TextMesh Text;

        public int Number = 0;


        public AudioSource Audio;

        private Vector3 startSize = new Vector3(0.1f, 0.1f, 0.1f);
        private float fadeTime = 0.3f;
        private float growTime = 0.5f;
        private float startTime;

        private Color startColor = new Color(0.5019608f, 0.03137255f, 1f, 0f);
        private Color finalColor = new Color(0.5019608f, 0.03137255f, 1f, 1f);

        private void Start()
        {
            Text.text = Number.ToString();
            Text.color = startColor;
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
                Text.color = finalColor;
            }
            else
            {
                Text.color = Color.Lerp(startColor, finalColor, delta);

                changed = true;
            }

            if (delta > growTime)
            {
                Text.transform.localScale = Vector3.one;
            }
            else
            {
                Text.transform.localScale = Vector3.Lerp(startSize, Vector3.one, delta);
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


            var audioVolume = Math.Clamp(other.relativeVelocity.magnitude, 0.5f, 15f) / 15f;
            if (audioVolume <= 0.1f)
            {
                return;
            }

            Audio.volume = audioVolume;
            Audio.Play();
        }
    }
}