using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 仅改类名，其他逻辑完全不动
public class simpleCamera : MonoBehaviour
{
    public simpleInput pi;
    public float horizontalSpeed = 200.0f;//摄像机水平的速度
    public float verticalSpeed = 80.0f;//摄像机垂直的速度

    public GameObject PlayerHandle;
    public GameObject CameraHandle;

    private float tempEulerX;

    private GameObject model;
    public new GameObject camera;

    void Awake()
    {
        // 原有逻辑，无改动
        CameraHandle = transform.parent.gameObject;
        PlayerHandle = CameraHandle.transform.parent.gameObject;
        tempEulerX = 20.0f;
        model = PlayerHandle.GetComponent<ManController>().model;
        camera = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        // 原有逻辑，无改动
        if (model == null || PlayerHandle == null || camera == null || pi == null) return;

        Vector3 tempModelEuler = model.transform.eulerAngles;

        PlayerHandle.transform.Rotate(Vector3.up, pi.Jright * Time.fixedDeltaTime * horizontalSpeed * 0.1f);
        tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime * 0.5f;

        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        CameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

        model.transform.eulerAngles = tempModelEuler;
        camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, 0.1f);
        camera.transform.eulerAngles = transform.eulerAngles;
    }
}