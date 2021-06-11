using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;

    public BoolVars isInPast;

    public FloatVars cameraSize,
                    cameraSizePast;

    Camera thisCamera;

    Vector3 mapHeight, mapHeightPast;


    void Start()
    {
        mapHeight = new Vector3(0,2,0);
        mapHeightPast = new Vector3(0,50,0);

        thisCamera = GetComponent<Camera>();
    }

    void LateUpdate ()
    {
        if (isInPast.value)
        {
            transform.position = player.position + mapHeightPast;

            if (thisCamera.orthographicSize != cameraSizePast.value)
            {
                thisCamera.orthographicSize = cameraSizePast.value;
            }
        }
        else
        {
            transform.position = player.position + mapHeight;

            if (thisCamera.orthographicSize != cameraSize.value)
            {
                thisCamera.orthographicSize = cameraSize.value;
            }
        }
        
	}
}