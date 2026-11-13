using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class VirtualCursor : MonoBehaviour
{
    [SerializeField] private VirtualMouseInput virtualMouse;
    [SerializeField] private Graphic cursorGraphic;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private InputSystemUIInputModule uiInputModule;

    private InputActionAsset virtualUiActions;
    private InputAction physicalMouseDelta;
    private InputAction physicalMouseClick;
    private InputActionReference virtualPoint;
    private InputActionReference virtualClick;
    private InputActionReference originalPoint;
    private InputActionReference originalClick;

    private void Awake()
    {
        virtualMouse ??= GetComponent<VirtualMouseInput>();
        cursorGraphic ??= GetComponent<Graphic>();
        cursorTransform ??= GetComponent<RectTransform>();
        uiInputModule ??= FindFirstObjectByType<InputSystemUIInputModule>();

        cursorGraphic.raycastTarget = false;
        virtualMouse.cursorMode = VirtualMouseInput.CursorMode.SoftwareCursor;
        virtualMouse.cursorGraphic = cursorGraphic;
        virtualMouse.cursorTransform = cursorTransform;

        ConfigureInput();
        SetGameplayActive(false);
    }

    private void ConfigureInput()
    {
        physicalMouseDelta = new InputAction("VirtualCursorDelta", InputActionType.PassThrough, "<Mouse>{Mouse}/delta");
        physicalMouseClick = new InputAction("VirtualCursorClick", InputActionType.PassThrough, "<Mouse>{Mouse}/leftButton");
        virtualMouse.stickAction = new InputActionProperty(physicalMouseDelta);
        virtualMouse.leftButtonAction = new InputActionProperty(physicalMouseClick);

        virtualUiActions = ScriptableObject.CreateInstance<InputActionAsset>();
        virtualUiActions.name = "VirtualCursorUIActions";
        var map = virtualUiActions.AddActionMap("VirtualCursorUI");
        var point = map.AddAction("Point", InputActionType.PassThrough);
        point.expectedControlType = "Vector2";
        point.AddBinding("<Mouse>/position");
        var click = map.AddAction("Click", InputActionType.PassThrough);
        click.expectedControlType = "Button";
        click.AddBinding("<Mouse>/leftButton");
        virtualPoint = InputActionReference.Create(virtualUiActions.FindAction("VirtualCursorUI/Point"));
        virtualClick = InputActionReference.Create(virtualUiActions.FindAction("VirtualCursorUI/Click"));

        if (uiInputModule == null)
            return;

        originalPoint = uiInputModule.point;
        originalClick = uiInputModule.leftClick;
    }

    public void SetGameplayActive(bool active)
    {
        if (virtualMouse != null)
            virtualMouse.enabled = active;

        if (cursorGraphic != null)
            cursorGraphic.enabled = active;

        if (uiInputModule != null && virtualPoint != null && virtualClick != null)
        {
            uiInputModule.point = active ? virtualPoint : originalPoint;
            uiInputModule.leftClick = active ? virtualClick : originalClick;
        }
    }

    private void OnDestroy()
    {
        physicalMouseDelta?.Dispose();
        physicalMouseClick?.Dispose();
        if (virtualUiActions != null)
            Destroy(virtualUiActions);
    }
}
