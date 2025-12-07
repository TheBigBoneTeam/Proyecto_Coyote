using CombatEffect;
using Services;
using UnityEngine;


    public class HealOrb : TouchCombatEffectSource, IPoolObject
    {
    int health;
        IHealthSpawner spawner;
        protected override void OnTriggerEnter(Collider other)
        {
            AGameCharacter character = other.GetComponent<AGameCharacter>();
            if (character)
            {
            Player player = character.GetComponent<Player>();
                if ( player && player.HealthPoint < player._maxHealthPoint )
                {
                    addEffectsToChar(character);
                    spawner.returnOrb(this);
                }

            }
        }
        public bool Active { get => gameObject.activeSelf; set { gameObject.SetActive(value); } }

        public void Clean()
        {

        }

        public IPoolObject Clone(Transform parent = null, bool active = false)
        {
            var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
            instance.gameObject.SetActive(active);
            instance.spawner = ServiceLocator.Instance.Get<IHealthSpawner>();
            return instance;
        }
        public void setHeal(int heal)
        {
        health = heal;
            effects.Clear();
            effects.Add(new HealEffect(this, heal));
        }
    }

