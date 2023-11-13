using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class NumberBehaviour : MonoBehaviour
    {
        public TextMesh Text;

        public int Number = 0;


        private void Start()
        {
            Text.text = Number.ToString();
        }
    }
}