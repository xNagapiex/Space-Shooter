﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCam : MonoBehaviour {

    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBG;

    public RawImage background;
    public AspectRatioFitter fit;

    private void Start()
    {
        StartCam();
    }

    private void Update()
    {
        UpdateCam();
    }

    void StartCam()
    {
        defaultBG = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("Camera not found.");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("Back camera not found.");
            return;
        }

        backCam.Play();
        background.texture = backCam;

        camAvailable = true;
    }

    void UpdateCam()
    {
        if (!camAvailable)
            return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localEulerAngles = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}