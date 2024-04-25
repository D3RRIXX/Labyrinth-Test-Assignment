using UnityEngine;

namespace Labyrinth.Game.Characters.Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float _moveSpeed = 3f;

		private Rigidbody _rb;
		private Vector3 _inputDirection;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			MoveInDirection();
		}

		public void ConsumeMovementInput(Vector2 direction)
		{
			_inputDirection = new Vector3(direction.x, 0f, direction.y);
		}

		private void MoveInDirection()
		{
			_rb.velocity = _inputDirection * _moveSpeed;

			RotateTowardsMovement(_inputDirection);
			_inputDirection = Vector3.zero;
		}

		private void RotateTowardsMovement(Vector3 dir)
		{
			if (dir == Vector3.zero)
				return;
			
			transform.rotation = Quaternion.LookRotation(dir);
		}
	}
}