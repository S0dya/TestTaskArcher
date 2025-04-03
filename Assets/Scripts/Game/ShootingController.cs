using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ShootingController : MonoBehaviour
    {
        // public event Action OnStartAiming;
        public event Action<Vector2, Vector2, float> OnAiming;
        public event Action OnShoot;
        
        [SerializeField] private ArrowShooter arrowShooter;
        [SerializeField] private ArcherView archerView;
        
        [Inject] private InputManager _inputManager;

        private Vector2 _aimDirection;
        private float _shootPower;
        
        private Vector2 _startPosition;
        private bool _isAiming;

        private Vector2 _aimPos;

        private void OnEnable()
        {
            _inputManager.OnStartInput +=StartAiming;
            _inputManager.OnInput +=Aim;
            _inputManager.OnEndInput +=StopAiming;
        }
        
        private void OnDisable()
        {
            _inputManager.OnStartInput -=StartAiming;
            _inputManager.OnInput -=Aim;
            _inputManager.OnEndInput -=StopAiming;
        }

        private void StartAiming(Vector2 pos)
        {
            _startPosition = pos;
            _isAiming = true;

            archerView.StartAiming();
        }

        private void Aim(Vector2 pos)
        {
            if (_isAiming)
            {
                _aimDirection = (_startPosition - pos).normalized;
                _shootPower = (_startPosition - pos).magnitude;

                _aimPos = archerView.Aim(_aimDirection);
                
                OnAiming.Invoke(_aimPos, _aimDirection, _shootPower);
            }
        }

        private void StopAiming(Vector2 pos)
        {
            _isAiming = false;

            OnShoot.Invoke();
            
            arrowShooter.Shoot(_aimPos, _aimDirection, _shootPower);
            archerView.Shoot();
        }
    }
}