using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Icarus
{
    [TaskCategory("Icarus/Actions")] //[TaskDescription("Adjust current orientation to avoid obstacles.")]
    public class AvoidObstacle : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Position of the target.")]
        public SharedVector2 targetPosition;
            
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Tags considered as obstacles.")]
        public string[] raycastTags = {"Asteroid"};

        public float safeDistanceTolerance;
        
        private Icarus _icarusController;
        private SpaceShipView _icarus;
        private GameData _gameData;

        public override void OnStart()
        {
            base.OnStart();
            _icarusController = gameObject.GetComponent<Icarus>();
            _icarus = _icarusController.IcarusShip;
        }

        public override TaskStatus OnUpdate()
        {
            if (targetPosition == null)
                return TaskStatus.Failure;
            
            UnityEngine.Vector2 dir = (UnityEngine.Vector2)targetPosition.GetValue() - _icarus.Position;
            RaycastHit2D hit = Physics2D.CircleCast(
                _icarus.Position,
                _icarus.Radius, 
                dir.normalized, 
                dir.magnitude,
                ~LayerMask.GetMask("Player"));
            
            if (!hit)
                return TaskStatus.Success;
            
            Debug.Log("hit tag = " + hit.collider.tag);
            
            CircleCollider2D colliderToAvoid = null;
            foreach (string tag in raycastTags)
            {
                if (!hit.collider.CompareTag(tag))
                    continue;
                
                colliderToAvoid = (CircleCollider2D)hit.collider;
                break;
            }

            if (colliderToAvoid == null)
                return TaskStatus.Success;

            return
                TryAvoid(colliderToAvoid) ?
                TaskStatus.Success : 
                TaskStatus.Failure;
        }
        
        private bool TryAvoid(CircleCollider2D colliderToAvoid)
        {
            UnityEngine.Vector2 colliderWorldPos = (UnityEngine.Vector2)colliderToAvoid.transform.position + colliderToAvoid.offset;
            UnityEngine.Vector2 obstacleToShip = _icarus.Position - colliderWorldPos;
            UnityEngine.Vector2 obstacleToTarget = (UnityEngine.Vector2)targetPosition.GetValue() - colliderWorldPos;
            
            obstacleToShip.Normalize();
            obstacleToTarget.Normalize();

            // On regarde quel quartier de l'obstacle on partage avec le target.
            // (1,1) = meme quartier,
            // (0,1) = meme cote horizontalement,
            // (1,0) = meme cote verticalement,
            // (0,0) = quartiers opposes.
            Vector2Int normalizedOffset = 
                new(
                Mathf.CeilToInt(obstacleToShip.x) & Mathf.CeilToInt(obstacleToTarget.x),
                Mathf.CeilToInt(obstacleToShip.y) & Mathf.CeilToInt(obstacleToTarget.y)
                );

            if (_icarusController.GetDrawDebugs)
            {
                Debug.DrawRay(colliderWorldPos, new Vector3(0, 1), Color.red);
                Debug.DrawRay(colliderWorldPos, new Vector3(1, 0), Color.red);
                Debug.DrawRay(colliderWorldPos, new Vector3(0, -1), Color.red);
                Debug.DrawRay(colliderWorldPos, new Vector3(-1, 0), Color.red);
                Debug.LogError($"AvoidObstacle : normalizedOffset = {normalizedOffset}.");
            }

            UnityEngine.Vector2 newTargetPos = new UnityEngine.Vector2(-99, -99);
            RaycastHit2D hit = new RaycastHit2D();
            if (normalizedOffset != Vector2Int.zero)
            {
                obstacleToShip *= normalizedOffset;
                newTargetPos = CalculateNewTargetPos(colliderWorldPos, colliderToAvoid, obstacleToShip, out hit);
                
                if (newTargetPos != new UnityEngine.Vector2(-99, -99))
                {
                    targetPosition.SetValue(newTargetPos);
                    return true;
                }
            }
            else
            {
                for (int i = 1; i < 3; i++)
                {
                    obstacleToShip *= new UnityEngine.Vector2(i % 2, (i - 1) % 2);
                    newTargetPos = CalculateNewTargetPos(colliderWorldPos, colliderToAvoid, obstacleToShip, out hit);

                    if (newTargetPos != new UnityEngine.Vector2(-99, -99))
                    {
                        targetPosition.SetValue(newTargetPos);
                        return true;
                    }
                }
            }
            
            // dernier essai avant de m'avouer vaincu, calculer par rapport a la normale
            newTargetPos = hit.point + (colliderToAvoid.radius + _icarus.Radius + safeDistanceTolerance) * hit.normal;
            hit = Physics2D.CircleCast(
                _icarus.Position, 
                _icarus.Radius,
                (newTargetPos - _icarus.Position).normalized,
                (newTargetPos - _icarus.Position).magnitude,
                ~LayerMask.GetMask("Player"));

            if (hit)
                return false; // Allez nsm hein
            
            targetPosition.SetValue(newTargetPos);
            return true;
        }

        private UnityEngine.Vector2 CalculateNewTargetPos(UnityEngine.Vector2 colliderWorldPos, CircleCollider2D colliderToAvoid, UnityEngine.Vector2 obstacleToShip, out RaycastHit2D hit)
        {
            UnityEngine.Vector2 newTargetPos =
                colliderWorldPos +
                (colliderToAvoid.radius + _icarus.Radius + safeDistanceTolerance) * obstacleToShip;
            
            // Un autre cast
            hit = Physics2D.CircleCast(
                _icarus.Position, 
                _icarus.Radius, 
                (newTargetPos - _icarus.Position).normalized,
                (newTargetPos - _icarus.Position).magnitude,
                ~LayerMask.GetMask("Player"));

            return hit ? newTargetPos : new UnityEngine.Vector2(-99, -99);
        }
    }
}