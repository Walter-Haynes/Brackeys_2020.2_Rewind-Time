using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace CommonGames.Tools.CustomMenuLayout
{
	using JetBrains.Annotations;
	
	using CommonGames.Utilities.CGTK;
	using CommonGames.Utilities.Extensions;

	[InitializeOnLoad]
    internal static partial class CustomMenuLayoutManager
    {
        #region Variables
		
		/// <summary>Menu for hierarchy.</summary>
		private static GenericMenu _hierarchyMenu = new GenericMenu();
		/// <summary>Menu for Project.</summary>
		private static GenericMenu _projectMenu = new GenericMenu();

        #endregion

        #region Methods
		
		static CustomMenuLayoutManager()
		{
			SetupProjectMenu();
			SetupHierarchyMenu();
			//SetupMenu<ProjectMenuLayout, ProjectMenuLayout>(menu: ref _projectMenu);
			//SetupMenu<HierarchyMenuLayout, HierarchyMenuLayout>(menu: ref _hierarchyMenu);
			
			EditorApplication.projectWindowItemOnGUI += OnProjectWindowGUI;
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowGUI;
		}

		#region OnGUI Events

		/// <summary>The callback called on drawing the GUI of hierarchy window.</summary>
		private static void OnHierarchyWindowGUI(int instanceId, Rect selectionRect)
		{
			if(HierarchyMenuLayout.Instances.ForEachReturnIf(predicate: pref => pref.IsCurrentMenuLayout) == null) return;
			
			Event __currentEvent = Event.current;
			if(__currentEvent.type != EventType.ContextClick) { return; }

			_hierarchyMenu.ShowAsContext();

			__currentEvent.Use();
		}

		/// <summary>The callback called on drawing the GUI of project window.</summary>
		private static void OnProjectWindowGUI(string guid, Rect selectionRect)
		{
			if(ProjectMenuLayout.Instances.ForEachReturnIf(predicate: pref => pref.IsCurrentMenuLayout) == null) return;
			
			Event __currentEvent = Event.current;
			if(__currentEvent.type != EventType.ContextClick) { return; }

			_projectMenu.ShowAsContext();

			__currentEvent.Use();
		}

		#endregion

		/// <summary>Validate the menus.</summary>
		[PublicAPI]
		public static void Validate()
		{
			SetupProjectMenu();
			SetupHierarchyMenu();
		}

		/// <summary>Load the layout, and create the Hierarchy menu.</summary>
		private static void SetupHierarchyMenu()
		{
			HierarchyMenuLayout __hierarchyMenuLayout = HierarchyMenuLayout.Instances.ForEachReturnIf(predicate: pref => pref.IsCurrentMenuLayout);

			if(__hierarchyMenuLayout != null)
			{
				CreateMenu(itemList: __hierarchyMenuLayout.layout, menu: ref _hierarchyMenu);
			}
		}
		
		/// <summary>Load the layout, and create the Hierarchy menu.</summary>
		private static void SetupProjectMenu()
		{
			ProjectMenuLayout __projectMenuLayout = ProjectMenuLayout.Instances.ForEachReturnIf(predicate: pref => pref.IsCurrentMenuLayout);

			if(__projectMenuLayout != null)
			{
				CreateMenu(itemList: __projectMenuLayout.layout, menu: ref _projectMenu);
			}
		}
		
		/*
		/// <summary>Load the layout, and create the menu.</summary>
		private static void SetupMenu<T_MenuLayout, T_MenuItem>(ref GenericMenu menu)
			where T_MenuLayout : BaseLayoutAsset<T_MenuLayout, T_MenuItem>
			where T_MenuItem : BaseMenuItem
		{
			T_MenuLayout __layoutPref = T_MenuLayout.Instances.ForEachReturnIf(predicate: pref => pref.IsCurrentMenuLayout);

			if(__layoutPref != null)
			{
				CreateMenu<T_MenuLayout>(__layoutPref.Layout, ref menu);
			}
		}
		*/

		#region MenuCreation

		/// <summary> Create menu from MenuLayout list. </summary>
		private static void CreateMenu<T_MenuItem>(in IList<T_MenuItem> itemList, ref GenericMenu menu)
			where T_MenuItem : BaseMenuItem
        {
			menu = new GenericMenu();

			if(itemList == null) return;
			
            int __itemCount = itemList.Count;
			
			T_MenuItem __prevItem = null;

			for(int __index = 0; __index < __itemCount; ++__index)
			{
				T_MenuItem __currItem = itemList[__index];

				switch(__currItem.elementType)
				{
					case ElementType.None:
					// If there is no previous item, skip.
					case ElementType.Separator when (__prevItem == null):
					// If previous item is null or empty, skip.
					case ElementType.Separator when (__prevItem.overridePath.IsNullOrEmpty()):
					// If the item is first or last, skip.
					case ElementType.Separator when (__index == 0 || __index == __itemCount-1):
						continue;
					
					case ElementType.Separator:
					{
						T_MenuItem __nextItem = itemList[index: __index+1];
						
						// Make sure the nextItem isn't nothing or a separator.
						if(__nextItem.elementType == ElementType.None || __nextItem.elementType == ElementType.Separator) continue;

						(string __matchingPart, var _, var _) = __prevItem.overridePath.SplitAtDeviation(__nextItem.overridePath);

						if(__matchingPart == null) continue;

						if(__matchingPart != "")
						{
							int __lastIndexOfSlash = __matchingPart.LastIndexOf('/');

							if(__lastIndexOfSlash != -1)
							{
								__matchingPart = __matchingPart.Substring(0, __lastIndexOfSlash+1);
							}
						}

						menu.AddSeparator(path: __matchingPart);
					
						break;
					}
					
					case ElementType.MenuPath:
						menu.AddItem(new GUIContent(__currItem.overridePath), false, func: (command => EditorApplication.ExecuteMenuItem(command as string)), userData: __currItem.originalPath);
						
						__prevItem = __currItem;
						break;
					default:
						throw new System.ArgumentOutOfRangeException();
				}
			}
		}
		
		#endregion

		#endregion
	}
}