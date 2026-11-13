mergeInto(LibraryManager.library, {
    WebGLPointerLock_RegisterCallbacks: function () {
        if (window.__bulletHellPointerLockCallbacksRegistered) {
            return;
        }

        window.__bulletHellPointerLockCallbacksRegistered = true;
        const canvas = Module.canvas;

        document.addEventListener("pointerlockchange", function () {
            SendMessage("WebGLFocusManager", "OnPointerLockChanged",
                document.pointerLockElement === canvas ? "1" : "0");
        });

        document.addEventListener("pointerlockerror", function () {
            SendMessage("WebGLFocusManager", "OnPointerLockChanged", "0");
        });

        document.addEventListener("visibilitychange", function () {
            SendMessage("WebGLFocusManager", "OnBrowserVisibilityChanged",
                document.visibilityState === "visible" ? "1" : "0");
        });

        window.addEventListener("focus", function () {
            SendMessage("WebGLFocusManager", "OnBrowserFocusChanged", "1");
        });

        window.addEventListener("blur", function () {
            SendMessage("WebGLFocusManager", "OnBrowserFocusChanged", "0");
        });
    },

    WebGLPointerLock_RequestPointerLock: function () {
        const canvas = Module.canvas;
        if (canvas && canvas.requestPointerLock) {
            canvas.requestPointerLock();
        }
    },

    WebGLPointerLock_ReleasePointerLock: function () {
        if (document.exitPointerLock) {
            document.exitPointerLock();
        }
    }
});
