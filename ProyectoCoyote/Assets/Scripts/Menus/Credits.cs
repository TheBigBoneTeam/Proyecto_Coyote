using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Credits : MonoBehaviour
{
    [System.Serializable]
    public class ZoneContent
    {
        public string zoneName;
        public Texture gifTexture;   
        public string text;
        public string text2;
    }

    public Image centralImage;          
    public RawImage gifDisplay;         
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI textDisplay2;
    public ZoneContent[] zones = new ZoneContent[6];

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = centralImage.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 localMousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, Input.mousePosition, null, out localMousePos))
        {
            // Normalizar coordenadas dentro de la imagen
            Vector2 size = rectTransform.rect.size;
            float x = (localMousePos.x + size.x / 2) / size.x;
            float y = (localMousePos.y + size.y / 2) / size.y;

            int zoneIndex = GetZoneIndex(x, y);

            if (zoneIndex >= 0 && zoneIndex < zones.Length)
            {
                ShowZoneContent(zones[zoneIndex]);
            }
        }
    }

    int GetZoneIndex(float x, float y)
    {
        //3 columnas y 2 filas
        int col;
        if (x < 1f / 3f) col = 0;       
        else if (x < 2f / 3f) col = 1;  
        else col = 2;                   

        int row = y < 0.5f ? 0 : 1;     

        return row * 3 + col; // índice de 0 a 5
    }

    void ShowZoneContent(ZoneContent content)
    {
        gifDisplay.texture = content.gifTexture;
        textDisplay.text = content.text;
        textDisplay2.text = content.text2;
    }
}
