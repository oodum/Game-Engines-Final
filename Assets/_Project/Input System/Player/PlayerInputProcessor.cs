using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;
namespace Input {
    [CreateAssetMenu(fileName = "Input Processor", menuName = "Scriptable Objects/Input Processor")]
    public class PlayerInputProcessor : ScriptableObject, IPlayerActions {
        public PlayerInputActions InputActions { get; private set; }

        public event Action OnUpEvent = delegate { };
        public event Action OnDownEvent = delegate { };
        public event Action OnLeftEvent = delegate { };
        public event Action OnRightEvent = delegate { };
        public event Action OnRotateLeftEvent = delegate { };
        public event Action OnRotateRightEvent = delegate { };

        void OnEnable() {
            InputActions = new();
            InputActions.Player.SetCallbacks(this);
            InputActions.Enable();
        }

        void OnDisable() {
            DisableInputs();
            InputActions.Disable();
        }

        public void DisableInputs() {
            Debug.Log("PlayerInputProcessor: Disabling actions");
            InputActions.Player.Disable();
        }

        public void SetPlayer() {
            DisableInputs();
            Debug.Log("PlayerInputProcessor: Fight actions enabled");
            InputActions.Player.Enable();
        }
        public void OnUp(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) OnUpEvent.Invoke();
        }
        public void OnDown(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) OnDownEvent.Invoke();
        }
        public void OnLeft(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) OnLeftEvent.Invoke();
        }
        public void OnRight(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) OnRightEvent.Invoke();
        }
        public void OnRotateLeft(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) OnRotateLeftEvent.Invoke();
        }
        public void OnRotateRight(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) OnRotateRightEvent.Invoke();
        }
    }
}
