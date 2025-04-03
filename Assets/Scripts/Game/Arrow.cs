using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Game
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private Collider2D arrowCollider;

        private static readonly string AttackAnimation = "attack";
        private static readonly string IdleAnimation = "idle";

        private Action<Arrow> _returnAction;

        public void Init(Action<Arrow> returnAction)
        {
            _returnAction = returnAction;
        }
        
        private void Update()
        {
            transform.right = rb.velocity.normalized;
        }

        public void Shoot(Vector2 startPosition, Vector2 force)
        {
            transform.position = startPosition;
            skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimation, true);
            
            gameObject.SetActive(true);
            
            rb.isKinematic = false;
            rb.velocity = force;
            arrowCollider.enabled = true;
        }

        public void TogglePhysics(bool toggle)
        {
            rb.isKinematic = !toggle;
            arrowCollider.enabled = toggle;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            TogglePhysics(false);
            rb.velocity = Vector2.zero;
            
            var trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, AttackAnimation, false);
            trackEntry.Complete += OnAttackComplete;
        }

        private void OnAttackComplete(TrackEntry trackEntry)
        {
            trackEntry.Complete -= OnAttackComplete;

            _returnAction?.Invoke(this);
        }
    }
}