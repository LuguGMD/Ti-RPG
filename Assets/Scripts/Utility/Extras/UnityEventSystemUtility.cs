using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEventSystem = UnityEngine.EventSystems.EventSystem;

using BaseUtility = LucasRozado.Utility.Utility;

namespace LucasRozado.Utility
{
    public static partial class Utility
    {
        public static Unity.EventSystem.Utility Get(UnityEventSystem of) => new(of);
    }

    public static partial class Unity
    {
        public static class EventSystem
        {
            public class Utility : BaseUtility.OfType<UnityEventSystem>
            {
                public Utility(UnityEventSystem of) : base(of)
                { }

                public bool IsCursorOverUIElement(Vector2 cursorPosition)
                {
                    List<RaycastResult> raycastResults = RaycastCursor(cursorPosition);

                    bool isAnyRaycastTargetUI = false;
                    foreach (RaycastResult raycastResult in raycastResults)
                    {
                        if (raycastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                        {
                            isAnyRaycastTargetUI = true;
                            break;
                        }
                    }
                    return isAnyRaycastTargetUI;
                }

                private List<RaycastResult> RaycastCursor(Vector2 cursorPosition)
                {
                    PointerEventData eventData = new(self)
                    { position = cursorPosition };

                    List<RaycastResult> raycastResults = new();
                    self.RaycastAll(eventData, raycastResults);

                    return raycastResults;
                }
            }
        }
    }
}
