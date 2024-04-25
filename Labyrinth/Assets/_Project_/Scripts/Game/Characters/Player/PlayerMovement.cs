using UnityEngine;

namespace Labyrinth.Game.Characters.Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float _moveSpeed = 3f;

		private Rigidbody _rb;
		private Vector3 _inputDirection;

		public Vector3 Velocity { get; private set; }

		public bool IsHiding => Velocity == Vector3.zero;

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
			Velocity = _inputDirection * _moveSpeed;
			_rb.MovePosition(_rb.position + Velocity * Time.deltaTime);

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