using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class ScoutObstacleInFront : Action
    {
        public enum TYPE
        {
            ASTEROID,
            MINE,
        }

        public SharedVector2 boxSize;
        public TYPE type;
        public Color debugColor;
        
        private SpaceShipView _spaceShip;

        public override void OnAwake()
        {
            _spaceShip = gameObject.GetComponent<Icarus>().IcarusShip;
        }

        public override TaskStatus OnUpdate()
        {
            string tag = type == TYPE.ASTEROID ? "Asteroid" : "Mine";

#if UNITY_EDITOR
            Debug.DrawLine(_spaceShip.Position, _spaceShip.Position + _spaceShip.LookAt * boxSize.Value.y, Color.red);
#endif

            RaycastHit2D[] hits = Physics2D.BoxCastAll(_spaceShip.Position, new Vector2(_spaceShip.Radius, boxSize.Value.y), 0.0f, _spaceShip.LookAt);

            if (hits.Length <= 0)
            {
                Debug.Log("Fail");
                return TaskStatus.Failure;
            }

            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.collider.CompareTag(tag))
                {
                    continue;
                }
                Debug.Log("Success");
                return TaskStatus.Success;
            }

            Debug.Log("Fail");
            return TaskStatus.Failure;
        }
    }
}

