// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private readonly float stopThreshold = 0.005f;

        [SerializeField]
        private Camera ARCamera;

        [SerializeField]
        private InputActionReference Button1;

        [SerializeField]
        private GameObject Character;

        [SerializeField]
        private Animator CharacterAnimator;

        [SerializeField]
        private InputActionReference LeftStick;

        [SerializeField]
        private InputActionReference LeftStickDelta;

        [SerializeField]
        private float MoveSpeed;

        [SerializeField]
        private Camera PhoneCamera;

        [SerializeField]
        private float RespawnRange = 10f;

        private Vector3 initialPos;
        private bool isPressed;

        private Vector2 leftStickValue;

        private Rigidbody rigidBody;
        private bool useARCamera;

        public string DebugText => leftStickValue.ToString();

        private bool IsPlayable => rigidBody != null && rigidBody.useGravity;

        // Start is called before the first frame update
        private void Start()
        {
            initialPos = transform.position;
            rigidBody = GetComponent<Rigidbody>();
            LeftStick.action.started += ctx =>
            {
                isPressed = true;
                leftStickValue = ctx.ReadValue<Vector2>();
            };
            LeftStickDelta.action.performed += ctx =>
            {
                isPressed = true;
                leftStickValue += ctx.ReadValue<Vector2>();
            };
            LeftStick.action.canceled += ctx =>
            {
                isPressed = false;
                leftStickValue = Vector2.zero;
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
                    rigidBody.velocity = Vector3.zero;
                }
                else
                {
                    transform.position = initialPos;
                    rigidBody.velocity = Vector3.zero;
                }
            }
        }

        public void Playable()
        {
            rigidBody.useGravity = true;
        }

        public void SwitchMoveFromARCamera(bool value)
        {
            useARCamera = value;
        }

        public void FixedUpdate()
        {
            if (!IsPlayable) return;

            CharacterAnimator.SetBool("walk", true);
            if (isPressed)
            {
                var forward = new Vector3(leftStickValue.x, 0, leftStickValue.y);
                if (useARCamera)
                    forward = ARCamera.transform.rotation * forward;
                else
                    forward = PhoneCamera.transform.rotation * forward;
                forward = new Vector3(forward.x, 0, forward.z).normalized;

                if (forward.sqrMagnitude < stopThreshold) return;

                transform.position += forward * MoveSpeed;
                Character.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            }

            if (Button1.action.IsPressed() && Mathf.Abs(rigidBody.velocity.y) < 0.01f)
                rigidBody.AddForce(new Vector3(0, 4, 0), ForceMode.Impulse);
        }
    }
}