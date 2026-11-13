using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WebGLFocusManager : MonoBehaviour
{
    public static WebGLFocusManager Instance { get; private set; }

    [SerializeField] private VirtualCursor virtualCursor;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resumeButton;

    private bool isPaused = true;
    private bool hasFocus = true;
    private bool isVisible = true;
    private bool pointerLockRequestPending;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        startButton?.onClick.AddListener(StartGame);
        resumeButton?.onClick.AddListener(ResumeGame);
        ShowStartScreen();
    }

    private void OnDisable()
    {
        startButton?.onClick.RemoveListener(StartGame);
        resumeButton?.onClick.RemoveListener(ResumeGame);
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLInput.stickyCursorLock = false;
        WebGLPointerLock.RegisterCallbacks();
#endif
    }

    private void Update()
    {
        if (!isPaused && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            PauseGameplay();
    }

    public void StartGame()
    {
        RequestPointerLock();
    }

    public void ResumeGame()
    {
        RequestPointerLock();
    }

    public void OnPointerLockChanged(string lockedValue)
    {
        var locked = lockedValue == "1";
        pointerLockRequestPending = false;

        if (locked && hasFocus && isVisible)
        {
            BeginGameplay();
        }
        else if (!locked && !isPaused)
        {
            PauseGameplay();
        }
    }

    public void OnBrowserVisibilityChanged(string visibleValue)
    {
        isVisible = visibleValue == "1";
        if (!isVisible)
            PauseGameplay();
    }

    public void OnBrowserFocusChanged(string focusedValue)
    {
        hasFocus = focusedValue == "1";
        if (!hasFocus)
            PauseGameplay();
    }

    private void RequestPointerLock()
    {
        if (!isPaused || !hasFocus || !isVisible)
            return;

        pointerLockRequestPending = true;
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLPointerLock.RequestPointerLock();
#else
        BeginGameplay();
#endif
    }

    private void BeginGameplay()
    {
        isPaused = false;
        pointerLockRequestPending = false;
        Time.timeScale = 1f;
        startPanel?.SetActive(false);
        pausePanel?.SetActive(false);
        Cursor.visible = false;
        virtualCursor?.SetGameplayActive(true);
    }

    private void PauseGameplay()
    {
        if (isPaused && !pointerLockRequestPending)
            return;

        isPaused = true;
        pointerLockRequestPending = false;
        Time.timeScale = 0f;
        virtualCursor?.SetGameplayActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLPointerLock.ReleasePointerLock();
#endif

        if (startPanel != null)
            startPanel.SetActive(false);
        pausePanel?.SetActive(true);
    }

    private void ShowStartScreen()
    {
        isPaused = true;
        pointerLockRequestPending = false;
        Time.timeScale = 0f;
        virtualCursor?.SetGameplayActive(false);
        startPanel?.SetActive(true);
        pausePanel?.SetActive(false);
    }

    private void OnApplicationFocus(bool focus)
    {
        hasFocus = focus;
        if (!focus)
            PauseGameplay();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        hasFocus = !pauseStatus;
        if (pauseStatus)
            PauseGameplay();
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLPointerLock.ReleasePointerLock();
#endif
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Instance = null;
    }

    private static class WebGLPointerLock
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal", EntryPoint = "WebGLPointerLock_RegisterCallbacks")] public static extern void RegisterCallbacks();
        [DllImport("__Internal", EntryPoint = "WebGLPointerLock_RequestPointerLock")] public static extern void RequestPointerLock();
        [DllImport("__Internal", EntryPoint = "WebGLPointerLock_ReleasePointerLock")] public static extern void ReleasePointerLock();
#else
        public static void RegisterCallbacks() { }
        public static void RequestPointerLock() { }
        public static void ReleasePointerLock() { }
#endif
    }
}
