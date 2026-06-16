using QuestMaker.Domain.Helpers;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = -500f;
        [SerializeField] private GameObject _sprite = null;

        private Rigidbody2D _rb;
        private Vector2 _direction;
        [SerializeField, ReadOnlyInspector] private bool _spinning;

        // the total rotation we want to perform, in degrees
        private readonly float _totalRotation = 360f;

        // the rotation we've done so far, in degrees
        private float _currentRotation = 0.0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.bodyType = RigidbodyType2D.Dynamic;

            _rotationSpeed = Mathf.Abs(_rotationSpeed);
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            _direction = new Vector2(horizontal, vertical).normalized;

            if (Input.GetKeyDown(KeyCode.Space) && !_spinning)
                _spinning = true;

            if (_spinning)
                Spin();
            
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _direction * _moveSpeed;
        }

        private void Spin()
        {
            // Rotation modified from: https://www.reddit.com/r/Unity3D/comments/ii9xu2/trying_to_rotate_a_full_circle_around_an_object/

            // if we haven't completed the rotation yet
            if (_currentRotation < _totalRotation)
            {
                // get the amount we want to rotate this frame in degrees
                float degreesToRotate = _rotationSpeed * Time.deltaTime;

                // clamp it so we don't overshoot
                // the overshoot would only be tiny, but we might as well get it to stop in exactly the right place
                degreesToRotate = Mathf.Min(degreesToRotate, _totalRotation - _currentRotation);

                _sprite.transform.Rotate(0f, 0f, degreesToRotate);

                // track how much rotation we've performed so far
                _currentRotation += degreesToRotate;
            }
            else
            {
                _currentRotation = 0f;
                _spinning = false;
            }

        }
    }
}