using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DesplegableDLC : MonoBehaviour
{
    [SerializeField] private Button headerButton;
    [SerializeField] private GameObject infoPanel;

    private bool isOpen = false;
    void Start()
    {
        infoPanel.SetActive(false);

        headerButton.onClick.AddListener(TogglePanel);
    }

    private void TogglePanel()
    {
        isOpen = !isOpen;
        infoPanel.SetActive(isOpen);
    }

   
}
