/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2018.                                 *
 * Leap Motion proprietary and confidential.                                  *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using UnityEngine;
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;

namespace Leap.Unity.Examples {

	using IntObj = InteractionBehaviour;

	[AddComponentMenu("")]
	public class destroyOnTouch : MonoBehaviour {

		public IntObj objA, objB;

		public Object enemyA;

		// Use this for initialization
		void Start () {
			PhysicsCallbacks.OnPostPhysics += onPostPhysics;
		}
		

		private void onPostPhysics(){
			if( objA.isGrasped ){
				Destroy(enemyA);

			}
		}
		// Update is called once per frame
		void Update () {
			
		}
	}
}
