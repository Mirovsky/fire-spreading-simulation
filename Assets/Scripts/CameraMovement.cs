using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour, ISettingsInjectable
{
    [SerializeField]
    float cameraSpeed;
    [SerializeField]
    float cameraScrollSpeed;
    [SerializeField]
    float cameraDragSpeed;
    [SerializeField]
    SimulationSettings settings;

    Transform cameraTransform;
    Camera mainCamera;

    Vector3 terrainOrigin;
    Vector3 terrainSize;

    Vector2 lastMousePos;

    const float cameraMin = 25f;
    const float cameraMax = 150f;


    public void InjectSettings(SimulationSettings s)
    {
        settings = s;
    }

    void Start()
    {
        mainCamera = Camera.main;
        cameraTransform = transform;

        terrainOrigin = settings.simulationTerrain.transform.position;
        terrainSize = settings.simulationTerrain.terrainData.size;

        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var pos = cameraTransform.position;

        pos.y += cameraScrollSpeed * scroll * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, cameraMin, cameraMax);

        var mousePos = Input.mousePosition;
        if (Input.GetMouseButton(2)) {
            var dragSpeed = cameraDragSpeed * (pos.y / cameraMax);

            pos.x += (lastMousePos.x - mousePos.x) * dragSpeed * Time.deltaTime;
            pos.z += (lastMousePos.y - mousePos.y) * dragSpeed * Time.deltaTime;
        } else {
            pos.x += cameraSpeed * horizontal * Time.deltaTime;
            pos.z += cameraSpeed * vertical * Time.deltaTime;
        }

        cameraTransform.position = pos;

        lastMousePos = mousePos;
    }

    
}
