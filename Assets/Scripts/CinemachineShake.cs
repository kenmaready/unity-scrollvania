using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{

    public static CinemachineShake Instance { get; private set; }
    CinemachineVirtualCamera shakeCam;
    CinemachineBasicMultiChannelPerlin perlin;
    float shakeTimer;
    bool isShaking;

    private void Awake() {
        Instance = this;
        shakeCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float duration) {
        perlin = shakeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;

        isShaking = true;
        shakeTimer = duration;
    }

    private void Update() {
        
        if (isShaking) {
            if (shakeTimer > 0) {
                shakeTimer -= Time.deltaTime;
            } else {
                perlin.m_AmplitudeGain = 0f;
                isShaking = false;
            }
        }
    }
}
