using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Evolutionary_perceptron;

namespace Evolutionary_perceptron.Examples.Survival
{

    public class CustomBotHandler : BotHandler
    {
        BotMovement pm;
        BotHealth ph;
        BotShooting ps;

        public Transform eyes;
        public int rayCount;
        public int angles;
        public float maxDistance;

        protected override void Start()
        {
            base.Start();

            pm = GetComponent<BotMovement>();
            ph = GetComponent<BotHealth>();
            ps = GetComponentInChildren<BotShooting>();

            r = new Ray[rayCount];
        }

        float[] d, e;

        void Update()
        {
            GetSensors(out d, out e);

            float[,] input = new float[1, d.Length + e.Length + 2];
            for (int i = 0; i < d.Length; i++)
            {
                input[0, i] = d[i];
            }
            for (int i = 0; i < e.Length; i++)
            {
                input[0, i + d.Length] = e[i];
            }
            input[0, d.Length + e.Length] = ph.currentHealth;
            input[0, d.Length + e.Length + 1] = ps.shootCounts;
            var output = nb.SetInput(input);

            pm.h = output[0, 0];
            pm.v = output[0, 1];
            pm.turn = Mathf.Clamp(output[0, 2], -1, 1);
            ps.fireButton = output[0, 3] > 0.9f;

        }

        Ray[] r;
        void GetSensors(out float[] distances, out float[] enemies)
        {
            distances = new float[rayCount];
            enemies = new float[rayCount];

            for (int i = 0; i < rayCount; i++)
            {
                float angle = angles * (i - rayCount / 2.0f) / rayCount;
                angle += transform.eulerAngles.y;
                angle *= Mathf.Deg2Rad;

                r[i] = new Ray(eyes.position, new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)));

                RaycastHit rh;

                Physics.Raycast(r[i], out rh, maxDistance);

                if (rh.collider != null)
                {
                    distances[i] = rh.distance / maxDistance;
                    if (rh.collider.CompareTag("Player"))
                    {
                        enemies[i] = 1;
                    }
                }
                else
                {
                    distances[i] = 1;
                }

                if (enemies[i] == 1)
                    Debug.DrawRay(r[i].origin, r[i].direction * distances[i] * maxDistance, Color.red);
                else
                    Debug.DrawRay(r[i].origin, r[i].direction * distances[i] * maxDistance, Color.green);
            }
        }

        void DamageAppied(int damage)
        {
            nb.AddFitness(damage);
        }

        void Dead()
        {
            nb.Destroy();
        }
    }
}