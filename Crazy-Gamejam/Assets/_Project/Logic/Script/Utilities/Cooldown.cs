using UnityEngine;

namespace Main.Utilities
{
    [System.Serializable]
    public class Cooldown
    {
        [SerializeField] public float _cooldownTime;

        private float _nextFireTime;

        public bool IsCoolingDown => Time.time < _nextFireTime;
        public void StartCoolDown() => _nextFireTime = Time.time + _cooldownTime;
    }
}
