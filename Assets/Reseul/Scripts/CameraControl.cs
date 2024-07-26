using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    CinemachineVirtualCamera vCam = null;
    Cinemachine3rdPersonFollow follow = null;

    public InputActionReference RightStick;

    public Vector2 rotationSpeed = new Vector2(-180, 180); // 1秒間90度
    public float inputMappingCurve = 5f;
    public float minCameraAngle = -45; // x軸回転の下限
    public float maxCameraAngle = 75; // x軸回転の上限
    public float cameraDistance = 2f; // 平均的なカメラの距離
    public float lowAngleDistanceRatio = 0.5f; // 低い時の距離の比率
    public float highAngleDistanceRatio = 3f; // 高い時の距離の比率

    [SerializeField]
    AnimationCurve StickSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        if (vCam != null)
        {
            follow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }
    }


    void FixedUpdate()
    {
        if (vCam != null)
        {
            Transform target = vCam.Follow; // バーチャルカメラの追跡ターゲットを取得
            if (target != null && RightStick.action.IsPressed())
            {
                Vector2 cameraRotationInput = RightStick.action.ReadValue<Vector2>();
                cameraRotationInput=new Vector2(StickSensitivity.Evaluate(cameraRotationInput.x), StickSensitivity.Evaluate(cameraRotationInput.y));
                
                // ターゲットの回転をオイラー角度（x, y, z）で取得
                Vector3 targetEulerAngles = target.rotation.eulerAngles;

                float curve = Mathf.Max(1, inputMappingCurve);
                Vector2 speed = new Vector2(
                    Mathf.Pow(Mathf.Abs(cameraRotationInput.x), curve) * Mathf.Sign(cameraRotationInput.x),
                    Mathf.Pow(Mathf.Abs(cameraRotationInput.y), curve) * Mathf.Sign(cameraRotationInput.y));

                // y軸の回転を変える
                targetEulerAngles.y += speed.x * rotationSpeed.x * Time.fixedDeltaTime;

                // y軸と同様にx軸の回転を変える
                targetEulerAngles.x += speed.y * rotationSpeed.y * Time.fixedDeltaTime;

                // target.rotation.eulerAnglesは0～360の角度を返す。これを-180～180に変える。
                if (targetEulerAngles.x > 180f)
                {
                    targetEulerAngles.x -= 360f;
                }

                // この状態で値を制限する。
                // 「Clamp」は一つ目の引数を2つ目と3つ目の引数の間に制限するメソッド
                targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, minCameraAngle, maxCameraAngle);

                // オイラー角度をクオータニオンに変換して追跡ターゲットの回転を変える
                target.transform.rotation = Quaternion.Euler(targetEulerAngles);

                if (follow)
                {
                    float anglePhase = (targetEulerAngles.x - minCameraAngle) / (maxCameraAngle - minCameraAngle);
                    float lowCameraDistance = cameraDistance * lowAngleDistanceRatio;
                    float highCameraDistance = cameraDistance * highAngleDistanceRatio;
                    follow.CameraDistance = lowCameraDistance + (highCameraDistance - lowCameraDistance) * anglePhase;
                }

            }
        }
    }
}
