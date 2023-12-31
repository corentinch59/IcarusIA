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

        public SharedFloat boxLength;
        public SharedFloat addedThickness;
        public SharedVector2 positionFound;
        public TYPE type;
        
        private SpaceShipView _spaceShip;

        public override void OnAwake()
        {
            _spaceShip = gameObject.GetComponent<Icarus>().IcarusShip;
        }

        public override TaskStatus OnUpdate()
        {
            string tag = type == TYPE.ASTEROID ? "Asteroid" : "Mine";

#if UNITY_EDITOR
            Debug.DrawLine(_spaceShip.Position, _spaceShip.Position + _spaceShip.LookAt * boxLength.Value, Color.red);
#endif

            Collider2D[] hits = Physics2D.OverlapBoxAll(_spaceShip.Position + _spaceShip.LookAt * boxLength.Value / 2, new Vector2(_spaceShip.Radius + addedThickness.Value, boxLength.Value), _spaceShip.Orientation);

            if (hits.Length <= 0)
            {
                return TaskStatus.Failure;
            }

            foreach (Collider2D hit in hits)
            {
                if (!hit.CompareTag(tag))
                {
                    continue;
                }
                positionFound.Value = hit.transform.position;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}

