using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow2D : MonoBehaviour
{
    [Header("����Ŀ��")]
    public Transform target;      // Ҫ�����Ŀ�꣨������ҽ�ɫ��
    [Header("��ͼTilemap")]
    public Tilemap tilemap;       // �����е�Tilemap�����ڻ�ȡ��ͼ�߽�
    [Header("ƽ������ʱ��")]
    public float smoothTime = 0.3f;   // ƽ�������ʱ�������ֵԽС����Խ��

    private Vector3 velocity = Vector3.zero;  // SmoothDampʹ�õ��ٶȲο�
    private float camHalfHeight;
    private float camHalfWidth;
    private float xMin, xMax, yMin, yMax;     // ��������ƶ�������߽緶Χ

    void Start()
    {
        // ��ȡ�����������������ӿڿ��
        Camera cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic)
        {
            Debug.LogWarning("CameraFollow2D: ��ǰ����û����������������");
            return;
        }
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        // ͨ�� Tilemap �Զ������ͼ������߽�
        if (tilemap != null)
        {
            // ��ȡTilemap�ı��ر߽�Bounds��������ͼ������Ƭ�ķ�Χ��
            Bounds localBounds = tilemap.localBounds;
            // ���߽��min��max��ת������������
            Vector3 worldMin = tilemap.transform.TransformPoint(localBounds.min);
            Vector3 worldMax = tilemap.transform.TransformPoint(localBounds.max);

            // �����������X���Y����ƶ��ļ���ֵ
            xMin = worldMin.x + camHalfWidth;
            xMax = worldMax.x - camHalfWidth;
            yMin = worldMin.y + camHalfHeight;
            yMax = worldMax.y - camHalfHeight;

            // ����������������ͼ���������ҰС����̶������������
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
            Debug.LogWarning("CameraFollow2D: δ����Tilemap���ã��޷��޶��߽磡");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        // ȡ��Ŀ��λ�ã�����XYƽ�棬Z��������������Z��
        Vector3 targetPos = target.position;
        // ��Ŀ��λ��������������߽緶Χ��
        float clampedX = Mathf.Clamp(targetPos.x, xMin, xMax);
        float clampedY = Mathf.Clamp(targetPos.y, yMin, yMax);
        Vector3 clampedPos = new Vector3(clampedX, clampedY, transform.position.z);

        // ƽ���ز�ֵ�ƶ������λ�õ�Ŀ��λ��
        transform.position = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, smoothTime);
    }
}