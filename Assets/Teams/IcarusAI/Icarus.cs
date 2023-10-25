using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;

namespace Icarus
{

	public class Icarus : BaseSpaceShipController
    {
        [SerializeField] private BehaviorTree _b;

        private SpaceShipView _otherSpaceship;

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
            base.Initialize(spaceship, data);
            _otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
        }

        public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
            //float thrust = 1.0f;
            //float targetOrient = spaceship.Orientation + 90.0f;
            //bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
            //return new InputData(thrust, targetOrient, needShoot, false, false);
            SharedVariable a = _b.GetVariable("thrust");
            float b = (float)a.GetValue();
            return new InputData(b, 0.0f, false, false, false);

		}
	}

}
