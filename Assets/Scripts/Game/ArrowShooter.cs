using Tools;
using UnityEngine;

namespace Game
{
    public class ArrowShooter : MonoBehaviour
    {
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private Transform arrowsParent;
    
        private ArrowPool _arrowPool = new();

        private void Awake()
        {
            _arrowPool.Init(arrowPrefab.gameObject, arrowsParent, 7);
        }
        
        public void Shoot(Vector2 startPosition, Vector2 direction, float power)
        {
            _arrowPool.Get().Shoot(startPosition, direction * power);
        }
    }
    
    class ArrowPool : PoolBase<Arrow>
    {
        protected override Arrow CreateObject()
        {
            var arrow = GameObject.Instantiate(_prefab, _parent).GetComponent<Arrow>();
            arrow.Init(Return);
            return arrow;
        }
        protected override void OnGet(Arrow arrow)
        {
            arrow.TogglePhysics(true);
        }
        protected override void OnSet(Arrow arrow)
        {
            arrow.gameObject.SetActive(false);
        }
        
        public void Return(Arrow arrow) => Set(arrow);
    }
}