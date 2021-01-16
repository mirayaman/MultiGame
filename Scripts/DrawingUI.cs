using UnityEngine;
using UnityEngine.UI;
public class DrawingUI: MonoBehaviour
{
    [Header("Данные для статус бара")]
    [SerializeField] private Image hpBar = null;
    [SerializeField] private Text drawGold = null;
    [SerializeField] private Text drawIron = null;
    [SerializeField] private Text drawBullet = null;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void UpdateStatusBar(int[] newValue)
    {
        float HeatPoints = newValue[0];
        hpBar.fillAmount = HeatPoints / 100;
        drawGold.text = newValue[1].ToString();
        drawIron.text = newValue[2].ToString();
        drawBullet.text = newValue[3].ToString();
    }

}
