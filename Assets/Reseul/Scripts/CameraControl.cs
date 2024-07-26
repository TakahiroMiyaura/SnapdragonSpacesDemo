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

    public Vector2 rotationSpeed = new Vector2(-180, 180); // 1�b��90�x
    public float inputMappingCurve = 5f;
    public float minCameraAngle = -45; // x����]�̉���
    public float maxCameraAngle = 75; // x����]�̏��
    public float cameraDistance = 2f; // ���ϓI�ȃJ�����̋���
    public float lowAngleDistanceRatio = 0.5f; // �Ⴂ���̋����̔䗦
    public float highAngleDistanceRatio = 3f; // �������̋����̔䗦

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
            Transform target = vCam.Follow; // �o�[�`�����J�����̒ǐՃ^�[�Q�b�g���擾
            if (target != null && RightStick.action.IsPressed())
            {
                Vector2 cameraRotationInput = RightStick.action.ReadValue<Vector2>();
                cameraRotationInput=new Vector2(StickSensitivity.Evaluate(cameraRotationInput.x), StickSensitivity.Evaluate(cameraRotationInput.y));
                
                // �^�[�Q�b�g�̉�]���I�C���[�p�x�ix, y, z�j�Ŏ擾
                Vector3 targetEulerAngles = target.rotation.eulerAngles;

                float curve = Mathf.Max(1, inputMappingCurve);
                Vector2 speed = new Vector2(
                    Mathf.Pow(Mathf.Abs(cameraRotationInput.x), curve) * Mathf.Sign(cameraRotationInput.x),
                    Mathf.Pow(Mathf.Abs(cameraRotationInput.y), curve) * Mathf.Sign(cameraRotationInput.y));

                // y���̉�]��ς���
                targetEulerAngles.y += speed.x * rotationSpeed.x * Time.fixedDeltaTime;

                // y���Ɠ��l��x���̉�]��ς���
                targetEulerAngles.x += speed.y * rotationSpeed.y * Time.fixedDeltaTime;

                // target.rotation.eulerAngles��0�`360�̊p�x��Ԃ��B�����-180�`180�ɕς���B
                if (targetEulerAngles.x > 180f)
                {
                    targetEulerAngles.x -= 360f;
                }

                // ���̏�ԂŒl�𐧌�����B
                // �uClamp�v�͈�ڂ̈�����2�ڂ�3�ڂ̈����̊Ԃɐ������郁�\�b�h
                targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, minCameraAngle, maxCameraAngle);

                // �I�C���[�p�x���N�I�[�^�j�I���ɕϊ����ĒǐՃ^�[�Q�b�g�̉�]��ς���
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
