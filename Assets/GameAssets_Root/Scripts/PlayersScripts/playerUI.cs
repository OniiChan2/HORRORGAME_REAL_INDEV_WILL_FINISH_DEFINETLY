using UnityEngine;
using TMPro;


public class playerUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI prompTXT;
 

    public void UpdateText(string promtMSG)
    {
        prompTXT.text = promtMSG;
    }
}
