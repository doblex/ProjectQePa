using UnityEngine;

namespace utilities.Controllers
{
    public class HealthController : MonoBehaviourWithAudio
    {
        public delegate void OnDeath();
        public delegate void OnDamage(float MaxHp, float currentHp);

        public OnDeath onDeath;
        public OnDamage onDamage;

        [Header("HP options")]
        [SerializeField] int maxHitPoints;
        [SerializeField] int currentHp;
        [SerializeField] bool invincible;

        [Header("Death Animation")]
        [SerializeField] GameObject deathEffect;

        /// <summary>
        /// sets the invincibility state of the health controller.
        /// </summary>
        public void SetInvicible(bool invincible) { this.invincible = invincible; }
        public int CurrentHp { get => currentHp; }
        public int MaxHitPoints { get => maxHitPoints; }


        private void Awake()
        {
            //currentHp = maxHitPoints;
            onDamage?.Invoke(maxHitPoints, currentHp);
        }

        /// <summary>
        /// Applies damage to the health controller.
        /// it also fires the onDamage event and if the health reaches 0 it fires the onDeath event.
        /// </summary>
        /// <param name="damage"> the damage to apply</param>
        public void DoDamage(int damage)
        {
            if (invincible)
                return;

            currentHp -= damage;
            onDamage?.Invoke(maxHitPoints, currentHp);

            if (currentHp <= 0)
            {
                if (deathEffect != null)
                {
                    GameObject clone = Instantiate(deathEffect, transform.position, Quaternion.identity);
                    Destroy(clone, clone.GetComponent<ParticleSystem>().main.duration);
                }

                onDeath?.Invoke();
                OnUnselectAudio?.Invoke(audioChannels);
                return;
            }
            if(damage !=0) OnPlayAudio?.Invoke(audioChannels);
        }

        /// <summary>
        /// Resets the health to the maximum hit points.
        /// </summary>
        public void ResetHealth()
        {
            currentHp = maxHitPoints;
            onDamage?.Invoke(maxHitPoints, currentHp);
        }

        /// <summary>
        /// Restores health by a specified amount, without exceeding the maximum hit points.
        /// </summary>
        /// <param name="amount">The amount of health to restore.</param>
        public void RestoreHealth(int amount)
        {
            currentHp += amount;
            currentHp = Mathf.Min(currentHp, maxHitPoints);
            onDamage?.Invoke(maxHitPoints, currentHp);
        }
    }
}