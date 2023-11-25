using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public PlayerData playerData;
    public WorldData worldData;

    private float xMin, xMax, yMin, yMax;
    private float camY,camX;
    private float camOrthsize;
    private float cameraRatio;
    private Camera mainCam;
    private Vector3 smoothPos;
    public float smoothSpeed = 0.5f;

    private void Start()
    {
        
        // xMin = worldData.worldBounds.bounds.min.x;
        // xMax = worldData.worldBounds.bounds.max.x;
        // yMin = worldData.worldBounds.bounds.min.y;
        // yMax = worldData.worldBounds.bounds.max.y;
        // mainCam = GetComponent<Camera>();
        // camOrthsize = mainCam.orthographicSize;
        // cameraRatio = (xMax + camOrthsize) / 2.0f;
    }
    void LateUpdate()
    {
        if (playerData == null || playerData.playerTransform == null) {
            return;
        }
        // Transform followTransform = playerData.playerTransform;
        // camY = Mathf.Clamp(followTransform.position.y, yMin + camOrthsize, yMax - camOrthsize);
        // camX = Mathf.Clamp(followTransform.position.x, xMin + cameraRatio, xMax - cameraRatio);
        // smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, this.transform.position.z), smoothSpeed);
        this.transform.position = new Vector3(playerData.playerTransform.position.x,playerData.playerTransform.position.y,this.transform.position.z);
    }
}