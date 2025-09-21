using UnityEngine;
using System.Collections;

public class KnifeController3D : MonoBehaviour
{
    public BreadController3D bread;
    public GameManager3D gameManager;

    [Header("左右移动参数")]
    public float horizontalSpeed = 2f;
    public float zMin = -0.5f;
    public float zMax = 0.5f;

    [Header("切割动画参数")]
    public float sliceMoveDistance = 10f;   // 刀上下移动的幅度
    public float sliceMoveSpeed = 10f;       // 移动速度（越大越快）

    int dir = 1;
    Vector3 originalPosition;
    bool isSlicing = false;

    void Start()
    {
        originalPosition = transform.localPosition; // 记录初始位置
    }

    void Update()
    {
        // 自动左右移动（只有不在切割动画时才动）
        if (!isSlicing)
        {
            Vector3 p = transform.position;
            p.z += dir * horizontalSpeed * Time.deltaTime;
            if (p.z > zMax) dir = -1;
            else if (p.z < zMin) dir = 1;
            transform.position = p;
        }

        // 鼠标点击切片
        if (Input.GetMouseButtonDown(0))
        {
            if (!isSlicing) StartCoroutine(SliceAnimation()); // 播放动画

            if (bread != null)
            {
                bool success = bread.SliceInPlace(transform.position);
                if (success)
                {
                    if (gameManager != null) gameManager.OnSuccessfulCut();
                }
                else
                {
                    if (gameManager != null) gameManager.OnMiss();
                }
            }
        }
    }

    private IEnumerator SliceAnimation()
    {
        isSlicing = true;

        Vector3 start = transform.localPosition;
        Vector3 upPos = start + Vector3.up * sliceMoveDistance; 


        // 向下
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * sliceMoveSpeed;
            transform.localPosition = Vector3.Lerp(start, upPos, t);
            yield return null;
        }

        // 回到原位
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * sliceMoveSpeed;
            transform.localPosition = Vector3.Lerp(upPos, start, t);
            yield return null;
        }

        isSlicing = false;
    }
}