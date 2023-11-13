using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Spawner : MonoBehaviour
    {
        public ControlBehaviour Controller;
        public Transform Container;
        public int maxElements = 30;
        
        public void Update()
        {
            if (Controller.ToSpawn.Count > 0)
            {
                SpawnElement(Controller.ToSpawn.Dequeue());
                CheckMax();
            }
        }

        private void SpawnElement(GameObject element)
        {
            element.transform.SetParent(Container);
            element.transform.SetPositionAndRotation(transform.position, transform.rotation);
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