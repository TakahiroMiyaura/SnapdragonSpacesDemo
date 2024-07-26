using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces
{
    public class PlayerController : MonoBehaviour
    {

        public float RespawnRange = 10f;

        public Camera ARCamera;
        public Camera PhoneCamera;
        public InputActionReference LeftStick;
        public InputActionReference Button1;
        private Rigidbody _rigidBody;
        public Animator CharacterAnimator;
        public GameObject Character;

        [SerializeField]
        AnimationCurve StickSensitivity;

        [SerializeField]
        float stopThreshold = 0.005f;

        public float MoveSpeed;

        private bool _isPlayable => _rigidBody != null && _rigidBody.useGravity;

        // Start is called before the first frame update
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < -RespawnRange)
            {
                transform.position = Camera.main.transform.position + new Vector3(Camera.main.transform.forward.x,2f, Camera.main.transform.forward.z);
                _rigidBody.velocity = Vector3.zero;
            }
        }

        public void Playable()
        {
            _rigidBody.useGravity = true;
        }

        private bool useARCamera;
        public void SwitchMoveFromARCamera(bool value)
        {
            useARCamera = value;
        }

        public void FixedUpdate()
        {

            if (!_isPlayable)
            {
                return;
            }

            var leftStickValue = LeftStick.action.ReadValue<Vector2>();

            CharacterAnimator.SetBool("walk", true);
            if (LeftStick.action.IsPressed())
            {
                var forward = new Vector3(StickSensitivity.Evaluate(leftStickValue.x), 0, StickSensitivity.Evaluate(leftStickValue.y));
                if (useARCamera)
                    forward = ARCamera.transform.rotation * forward;
                else
                    forward = PhoneCamera.transform.rotation * forward;
                forward = new Vector3(forward.x, 0, forward.z);

                if (forward.sqrMagnitude < stopThreshold) return;

                transform.position += forward * MoveSpeed;
                Character.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            }

            if (Button1.action.IsPressed() && Mathf.Abs(_rigidBody.velocity.y) < 0.01f)
            {
                _rigidBody.AddForce(new Vector3(0, 4, 0),ForceMode.Impulse);
            }
        }


    }
}
