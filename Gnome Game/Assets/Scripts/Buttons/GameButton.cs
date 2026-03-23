using UnityEngine;

public class GameButton : MonoBehaviour, IButton, ITriggerable
{
    public void Activate()
    {
        if (IsResetButton)
        {
            PressButton();  // visual down + material + isPressed true
            ResetAll();     // reset others
            return;
        }

        PressButton();
    }
    public void Deactivate()
    {
        if (!IsSticky)
        {
            UnpressButton();
        }
    }
    [SerializeField] float pressedHeight = -0.15f;
    [SerializeField] bool startPressed;

    [SerializeField] Transform buttonTop;
    Renderer buttonTopRenderer;
    Vector3 origLocalPos;
    Vector3 pressedLocalPos;

    bool isPressed;
    public bool IsPressed => isPressed;

    [SerializeField] bool isSticky = true;
    public bool IsSticky => isSticky;

    [SerializeField] bool isResetButton = false;
    public bool IsResetButton => isResetButton;

    [SerializeField] GameButton[] linkedButtons;

    [SerializeField] Material pressedMaterial;
    [SerializeField] Material unpressedMaterial;
    public void ResetAll()
    {
        foreach (GameButton button in linkedButtons)
        {
            if (button == null) continue;
            if (button == this) continue; // never reset self

            button.ResetButton();
        }
    }
    public void ActivateButton()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateButton()
    {
        gameObject.SetActive(false);
    }

    public void DestroyButton()
    {
        Destroy(gameObject);
    }

    public void PressButton()
    {
        if (isPressed) return;

        buttonTop.localPosition = pressedLocalPos;
        buttonTopRenderer.material = pressedMaterial;
        isPressed = true;

        if (isResetButton) return;

        foreach (GameButton button in linkedButtons)
        {
            if (button == null) continue;
            if (button == this) continue;

            button.FlipPress();
        }
    }

    public void UnpressButton()
    {
        if (!isPressed) return;

        buttonTop.localPosition = origLocalPos;
        buttonTopRenderer.material = unpressedMaterial;
        isPressed = false;
    }

    public void FlipPress()
    {
        if (isPressed)
        {
            buttonTop.localPosition = origLocalPos;
            buttonTopRenderer.material = unpressedMaterial;
            isPressed = false;
        }
        else
        {
            buttonTopRenderer.material = pressedMaterial;
            buttonTop.localPosition = pressedLocalPos;
            isPressed = true;
        }
    }
    public void ResetButton()
    {
        if (startPressed)
        {
            buttonTop.localPosition = pressedLocalPos;
            buttonTopRenderer.material = pressedMaterial;
            isPressed = true;
        }
        else
        {
            buttonTop.localPosition = origLocalPos;
            buttonTopRenderer.material = unpressedMaterial;
            isPressed = false;
        }
    }

    void Awake()
    {
        if (buttonTop == null)
        {
            Debug.LogError("ButtonTop is not assigned on Button.", this);
            enabled = false;
            return;
        }

        if (isResetButton)
        {
            if (linkedButtons == null || linkedButtons.Length == 0)
            {
                Debug.LogError("Reset button should have linked buttons.", this);
            }
            isSticky = false;
        }
        if (linkedButtons != null)
        {
            foreach (GameButton button in linkedButtons)
            {
                if (button == this)
                {
                    Debug.LogWarning("Button is linked to itself; ResetAll/Flip will ignore self.", this);
                    break;
                }
            }
        }
        buttonTopRenderer = buttonTop.GetComponent<Renderer>();

        origLocalPos = buttonTop.localPosition;
        pressedLocalPos = origLocalPos + new Vector3(0f, pressedHeight, 0f);

        ResetButton();
    }
}
