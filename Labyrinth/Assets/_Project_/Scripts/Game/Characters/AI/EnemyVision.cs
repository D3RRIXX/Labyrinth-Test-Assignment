using System;
using System.Collections;
using Labyrinth.Game.Characters.Player;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Labyrinth.Game.Characters.AI
{
	[RequireComponent(typeof(EnemyAI))]
	public class EnemyVision : MonoBehaviour
	{
		[SerializeField] private float _viewDistance;
		[SerializeField] private float _eyeHeight = 1.7f;
		[SerializeField, Range(0f, 360f)] private float _viewAngle;
		[Header("Layer Settings")]
		[SerializeField] private LayerMask _targetMask;
		[SerializeField] private LayerMask _obstacleMask;

		private static readonly Collider[] OVERLAP_RESULTS = new Collider[50];

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Handles.color = Color.red;
			float halfFOV = _viewAngle / 2f;

			Vector3 position = EyePosition;
			Vector3 startPoint = Quaternion.Euler(0, -halfFOV, 0) * transform.forward * _viewDistance + position;
			Vector3 endPoint = Quaternion.Euler(0, halfFOV, 0) * transform.forward * _viewDistance + position;

			Handles.DrawWireArc(position, Vector3.up, Quaternion.Euler(0, -halfFOV, 0) * transform.forward, _viewAngle, _viewDistance);
			Handles.DrawLine(position, startPoint);
			Handles.DrawLine(position, endPoint);
		}
#endif

		private Vector3 EyePosition => transform.position + Vector3.up * _eyeHeight;

		public void StartLookingForPlayer(Action<PlayerMovement> spottedPlayerCallback)
		{
			StartCoroutine(LookForPlayer(spottedPlayerCallback));
		}

		public bool CanSeeTarget(Transform target)
		{
			Vector3 direction = target.position - EyePosition;
			direction.y = 0f;

			return ResultIsInFov() && !HasObstacleAhead(out RaycastHit _);

			bool ResultIsInFov() => Vector3.Angle(transform.forward, direction) <= _viewAngle / 2f;
			bool HasObstacleAhead(out RaycastHit hit) => Physics.Raycast(EyePosition, direction, out hit, direction.magnitude, _obstacleMask);
		}

		private IEnumerator LookForPlayer(Action<PlayerMovement> spottedPlayerCallback)
		{
			while (true)
			{
				if (TryFindPlayer(out PlayerMovement player))
				{
					spottedPlayerCallback(player);
					yield break;
				}

				yield return null;
			}
		}

		private bool TryFindPlayer(out PlayerMovement player)
		{
			int count = Physics.OverlapSphereNonAlloc(EyePosition, _viewDistance, OVERLAP_RESULTS, _targetMask);

			for (int i = 0; i < count; i++)
			{
				Transform target = OVERLAP_RESULTS[i].transform;
				if (!CanSeeTarget(target) || !target.TryGetComponent(out player))
					continue;

				return true;
			}

			player = null;
			return false;
		}
	}
}