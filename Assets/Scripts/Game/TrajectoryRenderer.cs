using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class TrajectoryRenderer : MonoBehaviour
    {
        [SerializeField] private int dotCount = 7;
        [SerializeField] private Vector2 minSize = new(0.1f, 0.1f);
        [SerializeField] private float dotsDistanceStep = 0.1f;
        [SerializeField] private GameObject dotPrefab;

        [Inject] private ShootingController _shootingController;
        
        private List<Transform> _dots = new();

        private bool _areDotsShown;

        void Awake()
        {
            var initialSize = dotPrefab.transform.localScale;
            
            for (int i = 0; i < dotCount; i++)
            {
                var dotTransform = Instantiate(dotPrefab, transform).transform;

                dotTransform.localScale = Vector2.Lerp(initialSize, minSize, (float)i / (float)dotCount);
                
                _dots.Add(dotTransform);
            }
            
            ToggleDots(false);
        }
        
        private void OnEnable()
        {
            _shootingController.OnAiming += SetTrajectory;
            _shootingController.OnShoot += HideTrajectory;
        }
        
        private void OnDisable()
        {
            _shootingController.OnAiming -= SetTrajectory;
            _shootingController.OnShoot -= HideTrajectory;
        }

        private void SetTrajectory(Vector2 startPos, Vector2 direction, float power)
        {
            var velocity = direction * power;
            
            for (int i = 0; i < dotCount; i++)
            {
                var t = i * dotsDistanceStep;
                _dots[i].position = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;
            }

            if (!_areDotsShown) ToggleDots(true);
        }

        private void HideTrajectory()
        {
            ToggleDots(false);
        }

        private void ToggleDots(bool toggle)
        {
            foreach (var dot in _dots)
            {
                dot.gameObject.SetActive(toggle);
            }
            
            _areDotsShown = toggle;
        }
    }
}