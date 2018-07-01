using UnityEngine;

namespace Evolutionary_perceptron.Examples.Survival
{
    public class BotHealth : MonoBehaviour
    {
        public int startingHealth = 100;                            // The amount of health the player starts the game with.
        public int currentHealth;                                   // The current health the player has.
        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.

        Animator anim;                                              // Reference to the Animator component.
        AudioSource playerAudio;                                    // Reference to the AudioSource component.
        BotMovement playerMovement;                              // Reference to the player's movement.
        BotShooting playerShooting;                              // Reference to the PlayerShooting script.
        bool isDead;                                                // Whether the player is dead.


        void Awake ()
        {
            // Setting up the references.
            anim = GetComponent <Animator> ();
            playerAudio = GetComponent <AudioSource> ();
            playerMovement = GetComponent <BotMovement> ();
            playerShooting = GetComponentInChildren <BotShooting> ();

            // Set the initial health of the player.
            currentHealth = startingHealth;
        }

        public void TakeDamage (int amount)
        {

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // Play the hurt sound effect.
            playerAudio.Play ();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if(currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                Death ();
            }
        }


        void Death ()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Turn off any remaining shooting effects.
            playerShooting.DisableEffects ();

            // Tell the animator that the player is dead.
            anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            playerAudio.clip = deathClip;
            playerAudio.Play ();

            // Turn off the movement and shooting scripts.
            playerMovement.enabled = false;
            playerShooting.enabled = false;

            SendMessage("Dead");

        }


    }
}