
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class CanvasImageWiggler : MonoBehaviour
{
    [Range(0f,1f)] public float power = 0f;
    public RectTransform parent;
    public Vector2 baseResolution = new Vector2(1920, 1080);
    public Vector2 noiseSeed;
    public Vector2 range = new Vector2(10, 10);
    public float roughness = 0.4f;
    public Vector2 pivot = new Vector2(0.5f,0.5f);
    public Vector2 anchorsMin = new Vector2(0.5f,0.5f);
    public Vector2 anchorsMax = new Vector2(0.5f,0.5f);
    private RectTransform rectTransform;
    private Vector2 offsetPosition;
    private Vector3 offsetScale;
    public float time = 0f;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // rect = rectTransform.rect;
        offsetPosition = rectTransform.anchoredPosition;
        offsetScale = rectTransform.localScale;
    }

    // Update is called once per frame
    public void UpdateWiggle(float time)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.pivot = pivot;
        rectTransform.anchorMin = anchorsMin;
        rectTransform.anchorMax = anchorsMax;
        power = Mathf.Clamp(power, 0, 1);
        var scale = Mathf.Max((rectTransform.sizeDelta.x + range.x) / rectTransform.sizeDelta.x, (rectTransform.sizeDelta.y + range.y) / rectTransform.sizeDelta.y);
        scale = Mathf.Lerp(1f, scale, power);
        var x = (Mathf.PerlinNoise(noiseSeed.x+time*roughness, noiseSeed.y)-0.5f) * range.x * power;
        var y = (Mathf.PerlinNoise(noiseSeed.y, noiseSeed.y+time*roughness)-0.5f) * range.y * power;
        rectTransform.localScale = new Vector3(scale, scale, scale);
        rectTransform.rect.Set(x,y,parent.sizeDelta.x,parent.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    private void Update()
    {
        // UpdateWiggle(time);
    }

    private void OnDestroy()
    {
        rectTransform.anchoredPosition = offsetPosition;
        rectTransform.localScale = offsetScale;
    }
}
