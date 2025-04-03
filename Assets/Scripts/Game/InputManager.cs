using System;
using UnityEngine;

namespace Game
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        
        public event Action<Vector2> OnStartInput;
        public event Action<Vector2> OnInput;
        public event Action<Vector2> OnEndInput;

        private void Awake()
        {
            if (!cam) cam = Camera.main;
        }
        
        private void Update()
        {
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnStartInput.Invoke(cam.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (Input.GetMouseButton(0))
            {
                OnInput.Invoke(cam.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnEndInput.Invoke(cam.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
   
}