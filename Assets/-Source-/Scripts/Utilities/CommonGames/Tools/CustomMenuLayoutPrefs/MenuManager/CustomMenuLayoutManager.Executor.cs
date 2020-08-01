using UnityEngine;
using UnityEditor;

namespace CommonGames.Tools.CustomMenuLayout
{
    using CommonGames.Utilities.CGTK;

    internal static partial class CustomMenuLayoutManager
    {

        /// <summary>Execution class of each menu command.</summary>
        private static class Executor
        {
            /// <summary> Execute a command.</summary>
            public static void ExecuteMenuPath(object command)
            {
                EditorApplication.ExecuteMenuItem(command as string);
            }

            /// <summary>Send a command event to Hierarchy window.</summary>
            private static void SendEventToHierarchy(string command)
            {
                EditorApplication.ExecuteMenuItem(menuItemPath: "Window/Hierarchy");
                EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent(command));
            }

            /// <summary>Send a command event to Project window.</summary>
            private static void SendEventToProject(string command)
            {
                EditorApplication.ExecuteMenuItem(menuItemPath: "Window/Project");
                EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent(command));
            }
            
            
            private static void RenameGameObject()
            {
                EditorApplication.ExecuteMenuItem("Window/Hierarchy");
                #if UNITY_EDITOR_WIN
                EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.F2, type = EventType.KeyDown });
                #elif UNITY_EDITOR_OSX
                EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.Return, type = EventType.keyDown });
                #endif
            }
            
            private static void RenameAsset()
            {
                EditorApplication.ExecuteMenuItem("Window/Project");
                #if UNITY_EDITOR_WIN
                EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.F2, type = EventType.KeyDown });
                #elif UNITY_EDITOR_OSX
                EditorWindow.focusedWindow.SendEvent(new Event { keyCode = KeyCode.Return, type = EventType.keyDown });
                #endif
            }
        }
    }
}
