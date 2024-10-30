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
        public float cameraDistance = 2f; // 平均的なカメラの距離
        private Cinemachine3rdPersonFollow follow;
        public float highAngleDistanceRatio = 3f; // 高い時の距離の比率
        public float inputMappingCurve = 5f;
        public float lowAngleDistanceRatio = 0.5f; // 低い時の距離の比率
        public float maxCameraAngle = 75; // x軸回転の上限
        public float minCameraAngle = -45; // x軸回転の下限
        public InputActionReference RightStick;

        public InputActionReference RightStickDelta;

        public Vector2 rotationSpeed = new(-180, 180); // 1秒間90度

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
                var target = vCam.Follow; // バーチャルカメラの追跡ターゲットを取得
                if (target != null && _isPressed)
                {
                    var current = new Vector2(StickSensitivity.Evaluate(_rightStickValue.x),
                        StickSensitivity.Evaluate(_rightStickValue.y));

                    // ターゲットの回転をオイラー角度（x, y, z）で取得
                    var targetEulerAngles = target.rotation.eulerAngles;

                    var curve = Mathf.Max(1, inputMappingCurve);
                    var speed = new Vector2(
                        Mathf.Pow(Mathf.Abs(current.x), curve) * Mathf.Sign(current.x),
                        Mathf.Pow(Mathf.Abs(current.y), curve) * Mathf.Sign(current.y));

                    // y軸の回転を変える
                    targetEulerAngles.y += speed.x * rotationSpeed.x * Time.fixedDeltaTime;

                    // y軸と同様にx軸の回転を変える
                    targetEulerAngles.x += speed.y * rotationSpeed.y * Time.fixedDeltaTime;

                    // target.rotation.eulerAnglesは0～360の角度を返す。これを-180～180に変える。
                    if (targetEulerAngles.x > 180f) targetEulerAngles.x -= 360f;

                    // この状態で値を制限する。
                    // 「Clamp」は一つ目の引数を2つ目と3つ目の引数の間に制限するメソッド
                    targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, minCameraAngle, maxCameraAngle);

                    // オイラー角度をクオータニオンに変換して追跡ターゲットの回転を変える
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