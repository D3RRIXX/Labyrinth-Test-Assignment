using System.Collections;
using Labyrinth.Game.Characters.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Labyrinth.Game.Characters.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class EnemyAI : MonoBehaviour
	{
		[SerializeField] private Transform[] _patrolPoints;
		[SerializeField] private float _changePointCooldown;

		private NavMeshAgent _agent;
		private Coroutine _moveToPointRoutine;

		public int CurrentPatrolPoint { get; set; }

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
		}

		private void Start()
		{
			if (_patrolPoints.Length == 0)
				return;
				
			StartMovingToPoint(CurrentPatrolPoint);
		}

		public void OnSpottedPlayer(PlayerMovement player)
		{
			StopCoroutine(_moveToPointRoutine);

			_agent.ResetPath();
			_agent.updateRotation = false;

			transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);

			//TODO: Add chase logic
		}

		private void StartMovingToPoint(int pointIndex)
		{
			CurrentPatrolPoint = pointIndex;
			_moveToPointRoutine = StartCoroutine(MoveToPoint(pointIndex));
		}

		private IEnumerator MoveToPoint(int pointIndex)
		{
			_agent.SetDestination(_patrolPoints[pointIndex].position);

			yield return new WaitUntil(() => Vector3.Distance(transform.position, _agent.destination) <= 0.1f);
			yield return new WaitForSeconds(_changePointCooldown);

			int nextIndex = (pointIndex + 1) % _patrolPoints.Length;

			StartMovingToPoint(nextIndex);
		}
	}
}