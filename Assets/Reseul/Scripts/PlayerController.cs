// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces
{
    public class PlayerController : MonoBehaviour
    {
        private Vector3 _initialPos;
        private bool _isPressed;
        private Vector2 _leftStickValue;
        private Rigidbody _rigidBody;

        public Camera ARCamera;
        public InputActionReference Button1;
        public GameObject Character;
        public Animator CharacterAnimator;
        public InputActionReference LeftStick;
        public InputActionReference LeftStickDelta;

        public float MoveSpeed;
        public Camera PhoneCamera;

        public float RespawnRange = 10f;

        public string DebugText => _leftStickValue.ToString();

        [SerializeField]
        private float stopThreshold = 0.005f;

        private bool useARCamera;

        private bool _isPlayable => _rigidBody != null && _rigidBody.useGravity;

        // Start is called before the first frame update
        private void Start()
        {
            _initialPos = transform.position;
            _rigidBody = GetComponent<Rigidbody>();
            LeftStick.action.started += ctx =>
            {
                _isPressed = true;
                _leftStickValue = ctx.ReadValue<Vector2>();
            };
            LeftStickDelta.action.performed += ctx =>
            {
                _isPressed = true;
                _leftStickValue += ctx.ReadValue<Vector2>();
            };
            LeftStick.action.canceled += ctx =>
            {
                _isPressed = false;
                _leftStickValue = Vector2.zero;
            };
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.position.y < -RespawnRange)
            {
                if (SpacesGlassStatus.Instance.GlassConnectionState == SpacesGlassStatus.ConnectionState.Connected)
                {
                    transform.position = ARCamera.transform.position +
                                         new Vector3(ARCamera.transform.forward.x, 2f, ARCamera.transform.forward.z);
                    _rigidBody.velocity = Vector3.zero;
                }
                else
                {
                    transform.position = _initialPos;
                    _rigidBody.velocity = Vector3.zero;
                }
            }
        }

        public void Playable()
        {
            _rigidBody.useGravity = true;
        }

        public void SwitchMoveFromARCamera(bool value)
        {
            useARCamera = value;
        }

        public void FixedUpdate()
        {
            if (!_isPlayable) return;

            CharacterAnimator.SetBool("walk", true);
            if (_isPressed)
            {
                var forward = new Vector3(_leftStickValue.x, 0, _leftStickValue.y);
                if (useARCamera)
                    forward = ARCamera.transform.rotation * forward;
                else
                    forward = PhoneCamera.transform.rotation * forward;
                forward = new Vector3(forward.x, 0, forward.z).normalized;

                if (forward.sqrMagnitude < stopThreshold) return;

                transform.position += forward * MoveSpeed;
                Character.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            }

            if (Button1.action.IsPressed() && Mathf.Abs(_rigidBody.velocity.y) < 0.01f)
                _rigidBody.AddForce(new Vector3(0, 4, 0), ForceMode.Impulse);
        }
    }
}