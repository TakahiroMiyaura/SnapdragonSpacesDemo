// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float highAngleDistanceRatio = 3f; // 高い時の距離の比率

        [SerializeField]
        private float inputMappingCurve = 5f;

        [SerializeField]
        private float lowAngleDistanceRatio = 0.5f; // 低い時の距離の比率

        [SerializeField]
        private float maxCameraAngle = 75; // x軸回転の上限

        [SerializeField]
        private float minCameraAngle = -45; // x軸回転の下限

        [SerializeField]
        private InputActionReference rightStick;

        [SerializeField]
        private InputActionReference rightStickDelta;

        [SerializeField]
        private Vector2 rotationSpeed = new(-180, 180); // 1秒間90度

        [SerializeField]
        private AnimationCurve StickSensitivity;

        public float cameraDistance = 2f; // 平均的なカメラの距離
        private Cinemachine3rdPersonFollow follow;

        private bool isPressed;

        private Vector2 rightStickValue;

        private CinemachineVirtualCamera vCam;

        public string DebugText => rightStickValue.ToString();

        // Start is called before the first frame update
        private void Start()
        {
            vCam = GetComponent<CinemachineVirtualCamera>();
            if (vCam != null) follow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            rightStick.action.started += ctx =>
            {
                isPressed = true;
                rightStickValue = ctx.ReadValue<Vector2>();
            };
            rightStickDelta.action.performed += ctx =>
            {
                isPressed = true;
                rightStickValue += ctx.ReadValue<Vector2>();
            };
            rightStick.action.canceled += ctx =>
            {
                isPressed = false;
                rightStickValue = Vector2.zero;
            };
        }

        private void FixedUpdate()
        {
            if (vCam != null)
            {
                var target = vCam.Follow; // バーチャルカメラの追跡ターゲットを取得
                if (target != null && isPressed)
                {
                    var current = new Vector2(StickSensitivity.Evaluate(rightStickValue.x),
                        StickSensitivity.Evaluate(rightStickValue.y));

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