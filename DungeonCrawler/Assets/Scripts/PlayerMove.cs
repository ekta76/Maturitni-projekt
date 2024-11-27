using UnityEngine;
 

    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed = 100.0f;
        [SerializeField] private float _moveSpeed = 2.0f;
        [SerializeField] private float _stepSize = 1.0f;
        [SerializeField] private LayerMask _layer;
        private Vector3 _newPosition;
        private bool _isMoving = false;
        private bool _isRotation = false;
        private float _toAngle = 0;
 
        private void Awake()
        {
            _newPosition = transform.localPosition;
            _toAngle = transform.localEulerAngles.y;
        }
 
        private void Update()
        {
            if (!_isMoving && !_isRotation)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Direction(transform.forward * _stepSize);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Direction(-transform.forward * _stepSize);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Direction(-transform.right * _stepSize);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Direction(transform.right * _stepSize);
                }
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _newPosition, Time.deltaTime * _moveSpeed);
                if (transform.localPosition == _newPosition) _isMoving = false;
            }
 
            if (!_isMoving && !_isRotation)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Rotation(-90);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Rotation(90);
                }
            }
            else
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, _toAngle, 0), Time.deltaTime * _rotateSpeed);
                if (transform.localRotation == Quaternion.Euler(0, _toAngle, 0).normalized) _isRotation = false;
            }
 
        }
 
        private void MoveDirection(Vector3 vector)
        {
            Ray ray = new Ray(transform.localPosition, vector);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 1.0f, _layer))
                _newPosition = transform.localPosition + vector;
            _isMoving = true;
        }
 
        public void Direction(Vector3 vector)
        {
            if (!_isMoving)
                MoveDirection(vector);
        }
 
        public void Rotation(float angle)
        {
            if (!_isRotation)
            {
                _toAngle += angle;
                _isRotation = true;
            }
        }
    }
