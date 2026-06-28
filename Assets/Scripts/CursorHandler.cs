using Lugu.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG
{
    public class CursorHandler : SingletonMonoPersistent<CursorHandler>
    {
        [SerializeField] private Texture2D[] _cursor;
        [SerializeField] private Texture2D[] _cursorClick;
        private Texture2D _currentCursorTexture;

        private void Update()
        {
            //TO DO adicioanar verificacao de hover para mudar o index
            int cursorIndex = 0;
            Texture2D currentCursorTexture = _cursor[cursorIndex];

            if (Mouse.current != null)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    currentCursorTexture = _cursorClick[cursorIndex];
                }
            }

            if (_currentCursorTexture != currentCursorTexture)
            {
                _currentCursorTexture = currentCursorTexture;
                Cursor.SetCursor(_currentCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
    }
}
