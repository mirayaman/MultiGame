using UnityEngine;

[RequireComponent(typeof(DrawingUI))]
public class SpecificationsPlayer : MonoBehaviour
{
    [SerializeField] private int[] values = { 100, 100, 0, 0 };
    /*
     * id | name value
     *  0 | Heat Point
     *  1 | Have gold
     *  2 | Have iron
     *  3 | Have bullet
     */
    [SerializeField] private DrawingUI drawUI = null;

    private void Start()
    {
        drawUI = GetComponent<DrawingUI>();
    }

    public void changValue(int id, int value)
    {
        values[id] = values[id] + value;
        drawUI.UpdateStatusBar(values);
    }

}
