using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputRebinder : MonoBehaviour
{
    [Header("Binding Target")]
    [SerializeField] string actionName;
    [SerializeField] int bindingIndex = 0;

    [Header("UI")]
    [SerializeField] TMP_Text bindingText;
    [SerializeField] string listeningText = "Listening...";

    [Header("Optional")]
    [SerializeField] bool allowMouseScrollBinding = false;

    InputAction targetAction;
    InputActionRebindingExtensions.RebindingOperation currentRebindOperation;

    void Start()
    {
        FindAction();
        UpdateBindingText();
    }

    void OnEnable()
    {
        FindAction();
        UpdateBindingText();
    }

    void OnDisable()
    {
        CleanUpRebindOperation();
    }

    void FindAction()
    {
        if (PlayerInputHandler.Instance == null)
        {
            targetAction = null;
            return;
        }

        targetAction = PlayerInputHandler.Instance.GetAction(actionName);

        if (targetAction == null)
        {
            Debug.LogWarning($"[KeybindRebinder] Action '{actionName}' was not found.", this);
        }
    }

    public void StartRebind()
    {
        FindAction();

        if (targetAction == null)
        {
            Debug.LogWarning($"[KeybindRebinder] Cannot rebind '{actionName}' because the action was not found.", this);
            return;
        }

        if (bindingIndex < 0 || bindingIndex >= targetAction.bindings.Count)
        {
            Debug.LogWarning($"[KeybindRebinder] Binding index {bindingIndex} is out of range for '{actionName}'.", this);
            return;
        }

        CleanUpRebindOperation();

        if (bindingText != null)
        {
            bindingText.text = listeningText;
        }

        targetAction.Disable();

        currentRebindOperation = targetAction.PerformInteractiveRebinding(bindingIndex);

        currentRebindOperation
            .WithControlsExcluding("Mouse/position")
            .WithControlsExcluding("Mouse/delta")
            .WithCancelingThrough("<Keyboard>/escape");

        if (!allowMouseScrollBinding)
        {
            currentRebindOperation.WithControlsExcluding("Mouse/scroll");
        }

        currentRebindOperation
            .OnComplete(operation =>
            {
                targetAction.Enable();

                if (!IsBindingValid(targetAction, bindingIndex))
                {
                    targetAction.RemoveBindingOverride(bindingIndex);
                }

                PlayerInputHandler.Instance?.SaveBindingOverrides();
                UpdateBindingText();
                CleanUpRebindOperation();
            })
            .OnCancel(operation =>
            {
                targetAction.Enable();
                UpdateBindingText();
                CleanUpRebindOperation();
            });

        currentRebindOperation.Start();
    }

    public void ResetThisBinding()
    {
        FindAction();

        if (targetAction == null)
        {
            return;
        }

        if (bindingIndex < 0 || bindingIndex >= targetAction.bindings.Count)
        {
            return;
        }

        targetAction.RemoveBindingOverride(bindingIndex);
        PlayerInputHandler.Instance?.SaveBindingOverrides();
        UpdateBindingText();
    }

    public void UpdateBindingText()
    {
        FindAction();

        if (bindingText == null)
        {
            return;
        }

        if (targetAction == null)
        {
            bindingText.text = "Missing";
            return;
        }

        if (bindingIndex < 0 || bindingIndex >= targetAction.bindings.Count)
        {
            bindingText.text = "Invalid";
            return;
        }

        bindingText.text = targetAction.GetBindingDisplayString(bindingIndex);
    }

    bool IsBindingValid(InputAction action, int targetBindingIndex)
    {
        string newBindingPath = action.bindings[targetBindingIndex].effectivePath;

        if (string.IsNullOrEmpty(newBindingPath))
        {
            return false;
        }

        if (newBindingPath == "<Keyboard>/escape")
        {
            return false;
        }

        return true;
    }

    void CleanUpRebindOperation()
    {
        if (currentRebindOperation != null)
        {
            currentRebindOperation.Dispose();
            currentRebindOperation = null;
        }
    }
}