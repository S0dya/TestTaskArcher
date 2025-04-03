using Spine;
using Spine.Unity;
using UnityEngine;

namespace Game
{
    public class ArcherView : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
    
        [SpineBone(dataField: "skeletonAnimation")]
        [SerializeField] private string aimBoneName;
        [SpineBone(dataField: "skeletonAnimation")]
        [SerializeField] private string shootBoneName;

        private static readonly string AttackStartAnimation = "attack_start";
        private static readonly string AttackTargetAnimation = "attack_target";
        private static readonly string AttackFinishAnimation = "attack_finish";
        private static readonly string IdleAnimation = "idle";
        
        private Bone _aimBone;
        private Bone _shootBone;
        
        private float _targetAngle;
        
        private void Start()
        {
            skeletonAnimation.AnimationState.SetAnimation(0, IdleAnimation, true);
            
            _aimBone = skeletonAnimation.Skeleton.FindBone(aimBoneName);
            _shootBone = skeletonAnimation.Skeleton.FindBone(shootBoneName);
            
            skeletonAnimation.UpdateLocal += OnUpdateLocal;
        }

        private void OnUpdateLocal(ISkeletonAnimation anim)
        {
            if (_aimBone != null)
            {
                _aimBone.Rotation = _targetAngle;
            }
        }
        
        private void OnDestroy()
        {
            skeletonAnimation.UpdateLocal -= OnUpdateLocal;
        }
        
        public void StartAiming()
        {
            skeletonAnimation.AnimationState.SetAnimation(0, AttackStartAnimation, false);
            skeletonAnimation.AnimationState.AddAnimation(0, AttackTargetAnimation, true, 0);
        }
        
        public Vector2 Aim(Vector2 aimDirection)
        {
            _targetAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            if (aimDirection.x < 0)
            {
                skeletonAnimation.Skeleton.ScaleX = -1;
                _targetAngle = 180 - _targetAngle;
            }
            else
            {
                skeletonAnimation.Skeleton.ScaleX = 1;
            }

            return _shootBone.GetWorldPosition(skeletonAnimation.transform);
        }

        public void Shoot()
        {
            skeletonAnimation.AnimationState.SetAnimation(0, AttackFinishAnimation, false);
            skeletonAnimation.AnimationState.AddAnimation(0, IdleAnimation, true, 0);
        }
    }
}