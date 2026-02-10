using System;
using UnityEditor;
using UnityEngine;

namespace Lugu.SceneLoader
{
    public class EditorUtils
    {
        public static object[] DropZone()
        {

            EventType eventType = Event.current.type;
            bool isAccepted = false;

            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    isAccepted = true;
                }
                Event.current.Use();
            }

            return isAccepted ? DragAndDrop.objectReferences : null;
        }
    }

}