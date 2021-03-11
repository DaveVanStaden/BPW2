using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class EndGame : MonoBehaviour
    {
        public GameObject DungeonHolder;
        private DungeonGenerator gen;
        public GameObject mainHolder;
        private GeneratorInstantiator MainHolder;
        private void Awake()
        {
            DungeonHolder = GameObject.FindGameObjectWithTag("DungeonHolder");
            gen = DungeonHolder.GetComponent<DungeonGenerator>();
            mainHolder = GameObject.FindGameObjectWithTag("MainHolder");
            MainHolder = mainHolder.GetComponent<GeneratorInstantiator>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                DungeonHolder = GameObject.FindGameObjectWithTag("DungeonHolder");
                gen = DungeonHolder.GetComponent<DungeonGenerator>();
                MainHolder.SpawnGenerator();
                gen.EndGame();

            }
        }
    }
}

