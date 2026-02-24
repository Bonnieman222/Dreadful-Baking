using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MobileOnlyButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        // Show only on mobile platforms
#if UNITY_ANDROID || UNITY_IOS
        button.gameObject.SetActive(true);
#else
        button.gameObject.SetActive(false);
#endif
    }

    // Example function to assign to the button's OnClick() in the inspector
    public void OnButtonPressed()
    {
        Debug.Log("Mobile button pressed!");
        // Add your logic here
    }
}
