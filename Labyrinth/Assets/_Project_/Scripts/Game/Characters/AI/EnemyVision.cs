using System.Collections;
using Labyrinth.Game.Characters.Player;
using UnityEngine;

namespace Labyrinth.Game.Characters.AI
{
	public class EnemyVision : MonoBehaviour
	{
		[SerializeField] private float _viewDistance;
		[SerializeField, Range(0f, 360f)] private float _viewAngle;
		[Header("Layer Settings")]
		[SerializeField] private LayerMask _targetMask;
		[SerializeField] private LayerMask _obstacleMask;

		private EnemyAI _ai;

		private static readonly Collider[] OVERLAP_RESULTS = new Collider[50];

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, _viewDistance);
		}

		private void Awake()
		{
			_ai = GetComponent<EnemyAI>();
		}

		private void Start()
		{
			StartCoroutine(LookForPlayer());
		}

		private IEnumerator LookForPlayer()
		{
			while (true)
			{
				if (TryFindPlayer(out PlayerMovement player))
				{
					_ai.OnSpottedPlayer(player);
					yield break;
				}

				yield return null;
			}
			
		}
		private bool TryFindPlayer(out PlayerMovement player)
		{
			int count = Physics.OverlapSphereNonAlloc(transform.position, _viewDistance, OVERLAP_RESULTS, _targetMask);

			for (int i = 0; i < count; i++)
			{
				Collider target = OVERLAP_RESULTS[i];

				Vector3 direction = target.transform.position - transform.position;
				if (!ResultIsInFov(direction) || HasObstacleAhead(direction) || !target.TryGetComponent(out player))
					continue;

				if (!player.IsHiding)
					return true;
			}

			player = null;
			return false;

			bool ResultIsInFov(Vector3 direction) => Vector3.Angle(transform.forward, direction) <= _viewAngle / 2f;
			bool HasObstacleAhead(Vector3 direction) => Physics.Raycast(transform.position, direction, _viewDistance, _obstacleMask);
		}
	}
}