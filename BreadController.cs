using UnityEngine;

// ✅ 继承接口 OnTouch3D
public class BreadController3D : MonoBehaviour, OnTouch3D
{
    [Tooltip("最小允许保留的长度（世界单位）")]
    public float minLength = 0.01f;

    private BoxCollider box;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        if (box == null)
        {
            Debug.LogError("BreadController3D 需要 BoxCollider 来定义面包尺寸");
        }
    }

    // 👉 OnTouch 接口实现（点面包时会调用）
    public void OnTouch()
    {
        Debug.Log("🍞 面包被点击，尝试切一刀！");
        
        // 这里用一个虚拟的切割点（比如正中间）
        // 如果你之后想用刀的位置，可以传入刀的世界坐标
        Vector3 slicePoint = transform.position + transform.forward * 0.0f;
        
        SliceInPlace(slicePoint);
    }

    /// <summary>
    /// 在 worldPoint 处切割，保留较短段，直接缩放原 Cube（固定 Z 轴）
    /// 每次都按当前 Cube 的长度计算
    /// </summary>
    public bool SliceInPlace(Vector3 worldPoint)
    {
        if (box == null) return false;

        // box 在世界空间的半长向量
        Vector3 halfWorld = Vector3.Scale(box.size * 0.5f, transform.lossyScale);

        // 世界空间下的 z 方向
        Vector3 zAxis = transform.forward; // 本地Z轴对应的世界方向

        // 世界空间下的中心点
        Vector3 worldCenter = transform.TransformPoint(box.center);

        // min/max 投影到世界Z轴方向上的坐标（标量）
        float minZ = Vector3.Dot(worldCenter - zAxis * halfWorld.z, zAxis);
        float maxZ = Vector3.Dot(worldCenter + zAxis * halfWorld.z, zAxis);

        // 刀的投影坐标
        float coord = Vector3.Dot(worldPoint, zAxis);

        // 容差（保持世界单位）
        float tolerance = 0.01f;
        if (coord < minZ - tolerance || coord > maxZ + tolerance)
        {
            Debug.Log("Miss: 刀不在面包范围内");
            return false;
        }

        // 左右长度（世界坐标下的长度）
        float leftLen = coord - minZ;
        float rightLen = maxZ - coord;

        // 保留短边，且不能小于 minLength
        bool keepLeft = leftLen < rightLen;
        float keepWorldLength = Mathf.Max(minLength, keepLeft ? leftLen : rightLen);

        // === 缩放 ===
        float originalWorldLength = (maxZ - minZ);
        float scaleFactor = keepWorldLength / originalWorldLength;

        Vector3 newScale = transform.localScale;
        newScale.z *= scaleFactor;
        transform.localScale = newScale;

        // === 平移 ===
        float newCenterZ = keepLeft
            ? (minZ + keepWorldLength / 2f)
            : (maxZ - keepWorldLength / 2f);

        Vector3 newWorldCenter = worldCenter + (newCenterZ - Vector3.Dot(worldCenter, zAxis)) * zAxis;
        transform.position += (newWorldCenter - worldCenter);

        Debug.Log($"Bread: sliced in world. keep {(keepLeft ? "left" : "right")}, new length={keepWorldLength:F3}");

        return true;
    }
}
