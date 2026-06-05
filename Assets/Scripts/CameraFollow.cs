using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    public float smoothMoveX = 6f;
    public float xMargin = 2f;
    public Vector2 marginMax = new Vector2(10, 7);
    public Vector2 marginMin = new Vector2(-10, -7);
    void Start()
    {
        //player = GameObject.Find("Hero").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    bool NeedMoveX()
    {
        return (Mathf.Abs(transform.position.x - player.position.x)> xMargin );
    }
    void TrackPlayer()
    {
        float cameraNewX = transform.position.x;
        float cameraNewY = transform.position.y;
        cameraNewX = Mathf.Lerp(transform.position.x, player.position.x
                                , smoothMoveX * Time.deltaTime);
        cameraNewX = Mathf.Clamp(cameraNewX, marginMin.x, marginMax.x);
        transform.position = new Vector3(cameraNewX, cameraNewY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
    }
}
