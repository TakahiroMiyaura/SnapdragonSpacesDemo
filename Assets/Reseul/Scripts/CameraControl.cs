// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Samples.DualRenderFusionMRTK3
{
    public class CameraControl : MonoBehaviour
    {
        private Vector2 _rightStickValue;
        private bool _isPressed;
        public float cameraDistance = 2f; // ���ϓI�ȃJ�����̋���
        private Cinemachine3rdPersonFollow follow;
        public float highAngleDistanceRatio = 3f; // �������̋����̔䗦
        public float inputMappingCurve = 5f;
        public float lowAngleDistanceRatio = 0.5f; // �Ⴂ���̋����̔䗦
        public float maxCameraAngle = 75; // x����]�̏��
        public float minCameraAngle = -45; // x����]�̉���
        public InputActionReference RightStick;

        public InputActionReference RightStickDelta;

        public Vector2 rotationSpeed = new(-180, 180); // 1�b��90�x

        public string DebugText => _rightStickValue.ToString();

        [SerializeField]
        private AnimationCurve StickSensitivity;

        private CinemachineVirtualCamera vCam;

        // Start is called before the first frame update
        private void Start()
        {
            vCam = GetComponent<CinemachineVirtualCamera>();
            if (vCam != null) follow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            RightStick.action.started += ctx =>
            {
                _isPressed = true;
                _rightStickValue = ctx.ReadValue<Vector2>();
            };
            RightStickDelta.action.performed += ctx =>
            {
                _isPressed = true;
                _rightStickValue += ctx.ReadValue<Vector2>();
            };
            RightStick.action.canceled += ctx =>
            {
                _isPressed = false;
                _rightStickValue = Vector2.zero;
            };
        }

        private void FixedUpdate()
        {
            if (vCam != null)
            {
                var target = vCam.Follow; // �o�[�`�����J�����̒ǐՃ^�[�Q�b�g���擾
                if (target != null && _isPressed)
                {
                    var current = new Vector2(StickSensitivity.Evaluate(_rightStickValue.x),
                        StickSensitivity.Evaluate(_rightStickValue.y));

                    // �^�[�Q�b�g�̉�]���I�C���[�p�x�ix, y, z�j�Ŏ擾
                    var targetEulerAngles = target.rotation.eulerAngles;

                    var curve = Mathf.Max(1, inputMappingCurve);
                    var speed = new Vector2(
                        Mathf.Pow(Mathf.Abs(current.x), curve) * Mathf.Sign(current.x),
                        Mathf.Pow(Mathf.Abs(current.y), curve) * Mathf.Sign(current.y));

                    // y���̉�]��ς���
                    targetEulerAngles.y += speed.x * rotationSpeed.x * Time.fixedDeltaTime;

                    // y���Ɠ��l��x���̉�]��ς���
                    targetEulerAngles.x += speed.y * rotationSpeed.y * Time.fixedDeltaTime;

                    // target.rotation.eulerAngles��0�`360�̊p�x��Ԃ��B�����-180�`180�ɕς���B
                    if (targetEulerAngles.x > 180f) targetEulerAngles.x -= 360f;

                    // ���̏�ԂŒl�𐧌�����B
                    // �uClamp�v�͈�ڂ̈�����2�ڂ�3�ڂ̈����̊Ԃɐ������郁�\�b�h
                    targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, minCameraAngle, maxCameraAngle);

                    // �I�C���[�p�x���N�I�[�^�j�I���ɕϊ����ĒǐՃ^�[�Q�b�g�̉�]��ς���
                    target.transform.rotation = Quaternion.Euler(targetEulerAngles);

                    if (follow)
                    {
                        var anglePhase = (targetEulerAngles.x - minCameraAngle) / (maxCameraAngle - minCameraAngle);
                        var lowCameraDistance = cameraDistance * lowAngleDistanceRatio;
                        var highCameraDistance = cameraDistance * highAngleDistanceRatio;
                        follow.CameraDistance =
                            lowCameraDistance + (highCameraDistance - lowCameraDistance) * anglePhase;
                    }
                }
            }
        }
    }
}