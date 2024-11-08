using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

namespace DefaultNamespace
{
    public class Spawner : MonoBehaviour
    {
        public ControlBehaviour Controller;
        public Transform Container;
        public ParticleSystem Effect;
        public SplineAnimate SplineMotor;
        public int maxElements = 30;
        public AudioSource Audio;

        public void Update()
        {
            if (Controller.ToSpawn.Count > 0)
            {
                var element = Controller.ToSpawn.Dequeue();
                StartCoroutine(SpawnElement(element));
            }
        }

        private IEnumerator SpawnElement(GameObject element)
        {
            Effect.Play();

            yield return new WaitForSeconds(0.5f);

            Audio.Play();

            yield return new WaitForSeconds(0.5f);

            element.transform.SetParent(Container);
            element.transform.SetPositionAndRotation(transform.position, transform.rotation);

            var body2d = element.GetComponent<Rigidbody2D>();
            if (SplineMotor.ObjectForwardAxis == SplineComponent.AlignAxis.XAxis)
            {
                body2d.linearVelocityX = 4f;
            }
            else if (SplineMotor.ObjectForwardAxis == SplineComponent.AlignAxis.NegativeXAxis)
            {
                body2d.linearVelocityX = -4f;
            }

            element.SetActive(true);


            CheckMax();
        }

        private void CheckMax()
        {
            if (Container.childCount <= maxElements)
            {
                return;
            }

            var childToRemove = Container.GetChild(0);
            Destroy(childToRemove.gameObject);
        }
    }
}