using System.Collections;
using Labyrinth.Game.Characters.Player;
using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Labyrinth.Game.Characters.AI
{
	// Could implement FSM but I considered it to be overkill for such a small game
	[RequireComponent(typeof(NavMeshAgent), typeof(EnemyVision))]
	public class EnemyAI : MonoBehaviour
	{
		[SerializeField] private Transform[] _patrolPoints;
		[SerializeField] private float _changePointCooldown;
		[SerializeField] private float _catchPlayerDistance = 0.5f;

		private DisposeBox<Coroutine> _activeCoroutine;

		private Vector3 _cachedPoint;
		private IGameStateManager _gameStateManager;

		private NavMeshAgent _agent;
		private EnemyVision _vision;

		public int CurrentPatrolPoint { get; set; }

		[Inject]
		private void Construct(IGameStateManager gameStateManager)
		{
			_gameStateManager = gameStateManager;
		}

		private void Awake()
		{
			_activeCoroutine = new DisposeBox<Coroutine>(coroutine =>
			{
				if (coroutine != null)
					StopCoroutine(coroutine);
			});

			_agent = GetComponent<NavMeshAgent>();
			_vision = GetComponent<EnemyVision>();
		}

		private void Start()
		{
			StartPatrolling();
		}

		public void OnSpottedPlayer(PlayerMovement player)
		{
			_cachedPoint = transform.position;
			_activeCoroutine.Value = StartCoroutine(ChasePlayerRoutine(player.transform));
		}

		private IEnumerator ChasePlayerRoutine(Transform player)
		{
			var moveRoutine = StartCoroutine(MoveTowardsPlayer(player, 0.1f));

			while (_vision.CanSeeTarget(player))
			{
				transform.LookAt(player, Vector3.up);

				bool playerIsInDistance = Vector3.Distance(transform.position, player.position) < _catchPlayerDistance;
				if (playerIsInDistance)
				{
					_agent.ResetPath();
					_agent.velocity = Vector3.zero;
					
					_gameStateManager.CurrentState = GameState.LevelFailed;
					yield break;
				}

				yield return null;
			}

			StopCoroutine(moveRoutine);
			_activeCoroutine.Value = StartCoroutine(OnLostPlayerRoutine());
		}

		private IEnumerator MoveTowardsPlayer(Transform player, float interval)
		{
			Debug.Log("Started moving to player");
			_agent.updateRotation = false;
			while (true)
			{
				_agent.SetDestination(player.position);

				yield return new WaitForSeconds(interval);
			}
		}

		private IEnumerator OnLostPlayerRoutine()
		{
			Debug.Log("Lost player");
			_agent.updateRotation = true;
			_agent.SetDestination(_cachedPoint);
			yield return new WaitUntil(() => Vector3.Distance(transform.position, _agent.destination) <= 0.1f);

			StartPatrolling();
		}

		private void StartPatrolling()
		{
			_vision.StartLookingForPlayer(OnSpottedPlayer);

			if (_patrolPoints.Length == 0)
				return;

			StartMovingToPoint(CurrentPatrolPoint);
		}

		private void StartMovingToPoint(int pointIndex)
		{
			CurrentPatrolPoint = pointIndex;
			_activeCoroutine.Value = StartCoroutine(MoveToPointRoutine(pointIndex));
		}

		private IEnumerator MoveToPointRoutine(int pointIndex)
		{
			_agent.SetDestination(_patrolPoints[pointIndex].position);

			yield return new WaitUntil(() => Vector3.Distance(transform.position, _agent.destination) <= 0.1f);
			yield return new WaitForSeconds(_changePointCooldown);

			int nextIndex = (pointIndex + 1) % _patrolPoints.Length;

			StartMovingToPoint(nextIndex);
		}
	}
}