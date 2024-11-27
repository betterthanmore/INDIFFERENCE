using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Transform player; 
    public int fogTextureSize = 256; 
    public float revealRadius = 5f; 
    public Color fogColor = Color.black; 
    public Color revealedColor = Color.clear; 
    public RectTransform mapUI;
    public RectTransform fogUI;
    public RectTransform objectIconsParent; 
    public GameObject objectIconPrefab; 
    public List<Vector2> objectPositions;
    public GameObject mapObjcet;
    public GameObject optionWindow;

    [Header("Map Interaction")]
    public float minZoom = 1f; 
    public float maxZoom = 5f; 
    public float zoomSpeed = 1f; 
    public float dragSpeed = 1f; 

    private Texture2D fogOfWarTexture; 
    private Color[] fogColors;
    private Vector2 lastPlayerPosition;
    private float updateThreshold = 0.5f; 
    private Queue<GameObject> iconPool = new Queue<GameObject>(); 
    private Vector3 dragStartPos; 
    private Vector3 originalMapPos;
    private Vector3 originalFogPos;
    private float currentZoom = 1f;

    public Vector2 initialMapPosition = new Vector2(0, 0);

    void Start()
    {
        InitializeFogOfWar();
        InitializeObjectIcons();
    }

    void Update()
    {
        if (mapObjcet.gameObject.activeSelf == true && optionWindow.gameObject.activeSelf == true)
        {
            HandleMapZoom();
            HandleMapDrag();
        }
        UpdateFogOfWar();
    }

    // 안개 텍스처 초기화
    void InitializeFogOfWar()
    {
        fogOfWarTexture = new Texture2D(fogTextureSize, fogTextureSize);
        fogColors = new Color[fogTextureSize * fogTextureSize];
        for (int i = 0; i < fogColors.Length; i++)
        {
            fogColors[i] = fogColor; 
        }
        fogOfWarTexture.SetPixels(fogColors);
        fogOfWarTexture.Apply();

        mapUI.GetComponent<RawImage>().texture = fogOfWarTexture;   
    }

    // 플레이어 이동에 따른 안개 업데이트
    void UpdateFogOfWar()
    {
        if (Vector2.Distance(player.position, lastPlayerPosition) < updateThreshold)
            return;

        lastPlayerPosition = player.position;
        Vector2Int playerFogPos = WorldToFogPosition(player.position);
        int radiusInPixels = Mathf.RoundToInt(revealRadius * fogTextureSize / mapUI.rect.width);

        for (int y = -radiusInPixels; y <= radiusInPixels; y++)
        {
            for (int x = -radiusInPixels; x <= radiusInPixels; x++)
            {
                int px = playerFogPos.x + x;
                int py = playerFogPos.y + y;

                if (px < 0 || px >= fogTextureSize || py < 0 || py >= fogTextureSize)
                    continue;

                float distance = Mathf.Sqrt(x * x + y * y);
                if (distance <= radiusInPixels)
                {
                    int pixelIndex = py * fogTextureSize + px;
                    fogColors[pixelIndex] = revealedColor;
                }
            }
        }

        fogOfWarTexture.SetPixels(fogColors);
        fogOfWarTexture.Apply();
    }

    // 월드 좌표를 Fog of War 텍스처 좌표로 변환
    Vector2Int WorldToFogPosition(Vector2 worldPosition)
    {
        // 초기 좌표를 기준으로 월드 좌표를 변환
        Vector2 normalizedPosition = new Vector2(
            (worldPosition.x - (mapUI.rect.xMin + initialMapPosition.x)) / mapUI.rect.width,
            (worldPosition.y - (mapUI.rect.yMin + initialMapPosition.y)) / mapUI.rect.height
        );

        return new Vector2Int(
            Mathf.Clamp(Mathf.RoundToInt(normalizedPosition.x * fogTextureSize), 0, fogTextureSize - 1),
            Mathf.Clamp(Mathf.RoundToInt(normalizedPosition.y * fogTextureSize), 0, fogTextureSize - 1)
        );
    }

    // 맵 오브젝트 아이콘 초기화
    void InitializeObjectIcons()
    {
        foreach (Vector2 objPosition in objectPositions)
        {
            GameObject icon = GetPooledIcon();
            Vector2 mapPosition = WorldToMapPosition(objPosition);
            icon.GetComponent<RectTransform>().anchoredPosition = mapPosition;
        }
    }

    // 오브젝트 아이콘 풀링에서 가져오기
    GameObject GetPooledIcon()
    {
        if (iconPool.Count > 0)
        {
            GameObject icon = iconPool.Dequeue();
            icon.SetActive(true);
            return icon;
        }

        return Instantiate(objectIconPrefab, objectIconsParent);
    }

    // 월드 좌표를 맵 좌표로 변환
    Vector2 WorldToMapPosition(Vector2 worldPosition)
    {
        Vector2 normalizedPosition = new Vector2(
            (worldPosition.x - mapUI.rect.xMin) / mapUI.rect.width,
            (worldPosition.y - mapUI.rect.yMin) / mapUI.rect.height
        );

        return new Vector2(
            normalizedPosition.x * mapUI.rect.width,
            normalizedPosition.y * mapUI.rect.height
        );
    }

    // 맵 확대/축소 처리
    void HandleMapZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            currentZoom = Mathf.Clamp(currentZoom + scrollInput * zoomSpeed, minZoom, maxZoom);
            mapUI.localScale = Vector3.one * currentZoom;
            fogUI.localScale = Vector3.one * currentZoom;
        }
    }

    // 맵 드래그 처리
    void HandleMapDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            originalMapPos = mapUI.localPosition;
            originalFogPos = fogUI.localPosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 dragDelta = (Input.mousePosition - dragStartPos) * dragSpeed;
            mapUI.localPosition = originalMapPos + dragDelta;
            fogUI.localPosition = originalFogPos + dragDelta;
        }
    }
}