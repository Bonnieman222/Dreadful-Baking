using UnityEngine;
using UnityEngine.UI;

public class ActiveInputUIController : MonoBehaviour
{
    [Header("References")]
    public CameraMover cameraMover;
    public FlashlightController flashlightController;
    public ProcessController processController;
    public PauseMenuWithIntro pauseMenuScript;

    [Header("UI Buttons")]
    public Button moveSlot1Button;
    public Button moveSlot2Button;
    public Button moveSlot3Button;
    public Button moveSlot4Button;
    public Button moveSlot5Button;

    public Button pickUpFlashlightButton;
    public Button toggleFlashlightButton;

    public Button pickUpShotgunButton;
    public Button fireShotgunButton;

    public Button startProcessButton;
    public Button finishProcessButton;

    [Header("Mobile UI Root")]
    public GameObject mobileUIRoot;

    private bool isMobile;

    private void Awake()
    {
        DeterminePlatform();
        ApplyMobileState();
    }

    private void Start()
    {
        // Extra safety to prevent anything from re-enabling it in Editor
        DeterminePlatform();
        ApplyMobileState();

        if (!isMobile)
            return;

        SetupButtonListeners();
        UpdateButtonVisibility();
    }

    private void DeterminePlatform()
    {
#if UNITY_ANDROID || UNITY_IOS
        isMobile = true;
#else
        isMobile = false;
#endif
    }

    private void ApplyMobileState()
    {
        if (mobileUIRoot != null)
            mobileUIRoot.SetActive(isMobile);

        if (!isMobile)
            HideAllButtons();
    }

    private void SetupButtonListeners()
    {
        if (moveSlot1Button) moveSlot1Button.onClick.AddListener(() => TryMoveToSlot(0));
        if (moveSlot2Button) moveSlot2Button.onClick.AddListener(() => TryMoveToSlot(1));
        if (moveSlot3Button) moveSlot3Button.onClick.AddListener(() => TryMoveToSlot(2));
        if (moveSlot4Button) moveSlot4Button.onClick.AddListener(() => TryMoveToSlot(3));
        if (moveSlot5Button) moveSlot5Button.onClick.AddListener(() => TryMoveToSlot(4));

        if (pickUpFlashlightButton) pickUpFlashlightButton.onClick.AddListener(TryPickUpFlashlight);
        if (toggleFlashlightButton) toggleFlashlightButton.onClick.AddListener(TryToggleFlashlight);

        if (pickUpShotgunButton) pickUpShotgunButton.onClick.AddListener(TryPickUpShotgun);
        if (fireShotgunButton) fireShotgunButton.onClick.AddListener(TryFireShotgun);

        if (startProcessButton) startProcessButton.onClick.AddListener(TryStartProcess);
        if (finishProcessButton) finishProcessButton.onClick.AddListener(TryFinishProcess);
    }

    private void TryMoveToSlot(int slotIndex)
    {
        if (cameraMover == null) return;
        if (!cameraMover.CanMoveTo(slotIndex)) return;

        cameraMover.TryMoveTo(slotIndex);
        UpdateButtonVisibility();
    }

    private void TryPickUpFlashlight()
    {
        if (flashlightController == null) return;
        if (!flashlightController.IsAtCameraSlot0Public()) return;

        flashlightController.ToggleFlashlightPickup();
        UpdateButtonVisibility();
    }

    private void TryToggleFlashlight()
    {
        if (flashlightController == null) return;
        flashlightController.ToggleFlashlightLight();
    }

    private void TryPickUpShotgun()
    {
        if (flashlightController == null) return;
        if (!flashlightController.IsAtCameraSlot0Public()) return;

        flashlightController.ToggleShotgunPickup();
        UpdateButtonVisibility();
    }

    private void TryFireShotgun()
    {
        if (flashlightController == null) return;
        flashlightController.TryFireShotgun();
        UpdateButtonVisibility();
    }

    private void TryStartProcess()
    {
        if (processController == null) return;
        if (processController.IsProcessingPublic()) return;

        processController.TryStartProcess();
        UpdateButtonVisibility();
    }

    private void TryFinishProcess()
    {
        if (processController == null) return;
        if (!processController.CanFinishPublic()) return;

        processController.TryFinishProcess();
        UpdateButtonVisibility();
    }

    public void UpdateButtonVisibility()
    {
        if (!isMobile) return;

        if (cameraMover != null)
        {
            if (moveSlot1Button) moveSlot1Button.gameObject.SetActive(cameraMover.CanMoveTo(0));
            if (moveSlot2Button) moveSlot2Button.gameObject.SetActive(cameraMover.CanMoveTo(1));
            if (moveSlot3Button) moveSlot3Button.gameObject.SetActive(cameraMover.CanMoveTo(2));
            if (moveSlot4Button) moveSlot4Button.gameObject.SetActive(cameraMover.CanMoveTo(3));
            if (moveSlot5Button) moveSlot5Button.gameObject.SetActive(cameraMover.CanMoveTo(4));
        }

        if (flashlightController != null)
        {
            bool atSlot0 = flashlightController.IsAtCameraSlot0Public();

            if (pickUpFlashlightButton)
                pickUpFlashlightButton.gameObject.SetActive(atSlot0);

            if (pickUpShotgunButton)
                pickUpShotgunButton.gameObject.SetActive(atSlot0);

            if (toggleFlashlightButton)
                toggleFlashlightButton.gameObject.SetActive(flashlightController.IsFlashlightHeld());

            if (fireShotgunButton)
                fireShotgunButton.gameObject.SetActive(
                    flashlightController.IsShotgunHeld() &&
                    flashlightController.CanFireShotgun());
        }

        if (processController != null)
        {
            if (startProcessButton)
                startProcessButton.gameObject.SetActive(!processController.IsProcessingPublic());

            if (finishProcessButton)
                finishProcessButton.gameObject.SetActive(processController.CanFinishPublic());
        }
    }

    private void HideAllButtons()
    {
        foreach (Button btn in GetComponentsInChildren<Button>(true))
            btn.gameObject.SetActive(false);
    }
}
