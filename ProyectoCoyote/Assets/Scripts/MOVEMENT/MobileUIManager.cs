using UnityEngine;

public class MobileUIManager : MonoBehaviour
{
    public static MobileUIManager Instance;

    [Header("UI Móvil")]
    [SerializeField] private GameObject mobileUI_Combat;
    [SerializeField] private GameObject mobileUI_NonCombat;

    public bool isMobile {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isMobile = Application.isMobilePlatform;

        // Si el dispositivo no es un movil, se desactivan las interfaces de movil
        if (!isMobile )
        {
            if (mobileUI_Combat) mobileUI_Combat.SetActive(false);
            if (mobileUI_NonCombat) mobileUI_NonCombat.SetActive(false);
            return;
        }

        SetNonCombatUI();
    }

    public void SetCombatUI()
    {
        // Si no es en un movil, no pasa nada
        if (!isMobile) return;

        // Si es en un movil, se desactiva la interfaz de NonCombat y se activa la de Combat
        if (mobileUI_Combat) mobileUI_Combat.SetActive(true);
        if (mobileUI_NonCombat) mobileUI_NonCombat.SetActive(false);
    }

    public void SetNonCombatUI()
    {
        // Si no es en un movil, no pasa nada
        if (!isMobile) return;

        // Si es en un movil, se activa la interfaz de NonCombat y se desactiva la de Combat
        if (mobileUI_Combat) mobileUI_Combat.SetActive(false);
        if (mobileUI_NonCombat) mobileUI_NonCombat.SetActive(true);
    }
}
