using System.Collections;
using UnityEngine;

namespace Evolutionary_perceptron.Examples.Survival
{
    using Evolutionary_perceptron.MendelMachine;
    public class SurvivalMendelMachine : MendelMachine
    {
        public Transform[] spawnPoints;
        public Transform[] monsterSpawnPoints;

        public GameObject monsterPrefab;

        public float lifeTime;

        int index;

        protected override void Start()
        {
            individualsPerGeneration = spawnPoints.Length;
            base.Start();

            StartCoroutine(InstantiateBotCoroutine());
        }
        public override void NeuralBotDestroyed(NeuralBot neuralBot)
        {
            base.NeuralBotDestroyed(neuralBot);

            Destroy(neuralBot.gameObject);

            index--;

            if (index <= 0)
            {
                Save();
                population = Mendelization();
                generation++;

                StartCoroutine(InstantiateBotCoroutine());
            }
        }
        IEnumerator InstantiateBotCoroutine()
        {
            yield return null;

            index = individualsPerGeneration;

            for (int i = 0; i < individualsPerGeneration; i++)
            {
                InstantiateBot(population[i], lifeTime, spawnPoints[i], i);
            }

            for (int i = 0; i < monsterSpawnPoints.Length; i++)
            {
                var g = Instantiate(monsterPrefab, monsterSpawnPoints[i].position, Quaternion.identity);
                Destroy(g, lifeTime);
            }

        }
    }
}