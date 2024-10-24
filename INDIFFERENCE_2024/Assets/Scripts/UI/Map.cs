using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Map : MonoBehaviour, IScrollHandler, IDragHandler, IBeginDragHandler
{
    public RawImage fogImage; 
    public Transform player; 
    public float revealRadius = 5f; 
    private Texture2D fogTexture; 

    public GameObject mapUI; 
    public RectTransform mapRect; 
    public float zoomSpeed = 0.1f; 
    public float minZoom = 0.5f; 
    public float maxZoom = 2.0f; 
    private float currentZoom = 1.0f; 

    private Vector2 lastMousePosition; 
    private Vector3 lastPlayerPosition; 

    public Vector2 worldSize = new Vector2(100, 100); 
    public Vector2 mapSize; 

    void Start()
    {
        mapSize = new Vector2(mapRect.rect.width, mapRect.rect.height);

        fogTexture = new Texture2D((int)fogImage.rectTransform.rect.width, (int)fogImage.rectTransform.rect.height);
        for (int y = 0; y < fogTexture.height; y++)
        {
            for (int x = 0; x < fogTexture.width; x++)
            {
                fogTexture.SetPixel(x, y, Color.black); 
            }
        }
        fogTexture.Apply();
        fogImage.texture = fogTexture;

        lastPlayerPosition = player.position;
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) > 0.1f)
        {
            Vector2 playerPosOnMap = WorldToMapPosition(player.position);
            RevealFog(playerPosOnMap, revealRadius);
            lastPlayerPosition = player.position;
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M))
        {
            mapUI.SetActive(!mapUI.activeSelf);
        }
    }

    //월드 좌표를 맵 좌표로 변환
    Vector2 WorldToMapPosition(Vector3 worldPos)
    {
        RectTransform fogRect = fogImage.rectTransform;
        float mapWidth = fogRect.rect.width;
        float mapHeight = fogRect.rect.height;

        float x = (worldPos.x / worldSize.x) * mapWidth;
        float y = (worldPos.y / worldSize.y) * mapHeight; 

        return new Vector2(x, y);
    }

    //안개 제거 로직
    void RevealFog(Vector2 center, float radius)
    {
        int texWidth = fogTexture.width;
        int texHeight = fogTexture.height;
        int revealRadiusInPixels = Mathf.RoundToInt(radius * texWidth / mapSize.x); //맵 크기 기반 반경 픽셀 수 계산

        int startX = Mathf.Clamp(Mathf.RoundToInt(center.x - revealRadiusInPixels), 0, texWidth);
        int startY = Mathf.Clamp(Mathf.RoundToInt(center.y - revealRadiusInPixels), 0, texHeight);
        int endX = Mathf.Clamp(Mathf.RoundToInt(center.x + revealRadiusInPixels), 0, texWidth);
        int endY = Mathf.Clamp(Mathf.RoundToInt(center.y + revealRadiusInPixels), 0, texHeight);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                fogTexture.SetPixel(x, y, Color.clear); 
            }
        }

        fogTexture.Apply();
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (eventData.scrollDelta.y > 0)
        {
            ZoomIn();
        }
        else if (eventData.scrollDelta.y < 0)
        {
            ZoomOut();
        }
    }

    //확대
    void ZoomIn()
    {
        currentZoom = Mathf.Clamp(currentZoom + zoomSpeed, minZoom, maxZoom);
        mapRect.localScale = new Vector3(currentZoom, currentZoom, 1);
        ClampMapPosition();
    }

    //축소
    void ZoomOut()
    {
        currentZoom = Mathf.Clamp(currentZoom - zoomSpeed, minZoom, maxZoom);
        mapRect.localScale = new Vector3(currentZoom, currentZoom, 1);
        ClampMapPosition();
    }

    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;

        mapRect.anchoredPosition += diff;
        lastMousePosition = currentMousePosition;

        ClampMapPosition();
    }

    //드래그 화면 밖으로 나가지 않도록 제한
    void ClampMapPosition()
    {
        Vector3 pos = mapRect.anchoredPosition;

        float maxX = (mapRect.rect.width * currentZoom - mapSize.x) / 2f;
        float maxY = (mapRect.rect.height * currentZoom - mapSize.y) / 2f;

        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pos.y = Mathf.Clamp(pos.y, -maxY, maxY);

        mapRect.anchoredPosition = pos;
    }
}