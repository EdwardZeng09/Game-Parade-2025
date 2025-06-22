using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow2D : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;      // 要跟随的目标（例如玩家角色）
    [Header("地图Tilemap")]
    public Tilemap tilemap;       // 场景中的Tilemap，用于获取地图边界
    [Header("平滑跟随时间")]
    public float smoothTime = 0.3f;   // 平滑跟随的时间参数，值越小跟随越快

    private Vector3 velocity = Vector3.zero;  // SmoothDamp使用的速度参考
    private float camHalfHeight;
    private float camHalfWidth;
    private float xMin, xMax, yMin, yMax;     // 摄像机可移动的世界边界范围

    void Start()
    {
        // 获取摄像机组件并计算半个视口宽高
        Camera cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic)
        {
            Debug.LogWarning("CameraFollow2D: 当前物体没有正交摄像机组件！");
            return;
        }
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        // 通过 Tilemap 自动计算地图的世界边界
        if (tilemap != null)
        {
            // 获取Tilemap的本地边界Bounds（包含地图所有瓦片的范围）
            Bounds localBounds = tilemap.localBounds;
            // 将边界的min和max点转换到世界坐标
            Vector3 worldMin = tilemap.transform.TransformPoint(localBounds.min);
            Vector3 worldMax = tilemap.transform.TransformPoint(localBounds.max);

            // 计算摄像机在X轴和Y轴可移动的极限值
            xMin = worldMin.x + camHalfWidth;
            xMax = worldMax.x - camHalfWidth;
            yMin = worldMin.y + camHalfHeight;
            yMax = worldMax.y - camHalfHeight;

            // 特殊情况处理：如果地图比摄像机视野小，则固定摄像机在中心
            if (xMin > xMax)
            {
                float centerX = (worldMin.x + worldMax.x) * 0.5f;
                xMin = xMax = centerX;
            }
            if (yMin > yMax)
            {
                float centerY = (worldMin.y + worldMax.y) * 0.5f;
                yMin = yMax = centerY;
            }
        }
        else
        {
            Debug.LogWarning("CameraFollow2D: 未设置Tilemap引用，无法限定边界！");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        // 取得目标位置（跟随XY平面，Z保持摄像机自身的Z）
        Vector3 targetPos = target.position;
        // 将目标位置限制在摄像机边界范围内
        float clampedX = Mathf.Clamp(targetPos.x, xMin, xMax);
        float clampedY = Mathf.Clamp(targetPos.y, yMin, yMax);
        Vector3 clampedPos = new Vector3(clampedX, clampedY, transform.position.z);

        // 平滑地插值移动摄像机位置到目标位置
        transform.position = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, smoothTime);
    }
}