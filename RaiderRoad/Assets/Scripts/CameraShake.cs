using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin vCamNoise;

    private float count = 0;
    private float shakeDuration = 0f;
    private float shakeAmplitude = 0f;
    private float shakeFrequency = 0f;

    void Start()
    {
        vCamNoise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (count > 0)
        {
            vCamNoise.m_AmplitudeGain = shakeAmplitude * (count / shakeDuration);
            vCamNoise.m_FrequencyGain = shakeFrequency * (count / shakeDuration);
            count -= Time.deltaTime;
        }
        else
        {
            vCamNoise.m_AmplitudeGain = 0f;
            vCamNoise.m_FrequencyGain = 0f;
        }
    }

    public void Shake(float duration, float amplitude, float frequency)
    {
        shakeAmplitude = amplitude;
        shakeFrequency = frequency;
        shakeDuration = duration;
        count = duration;
    }
}
