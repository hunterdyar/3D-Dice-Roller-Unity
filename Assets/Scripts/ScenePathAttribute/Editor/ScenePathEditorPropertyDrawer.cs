using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//Goes in Editor Folder

namespace Blooper.Utilities
{
	[CustomPropertyDrawer(typeof(ScenePathAttribute))]

	public class ScenePathEditorPropertyDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var container = new VisualElement();
			var sceneSelect = new PopupField<string>();
			sceneSelect.choices = EditorBuildSettings.scenes.Select(x => x.path).ToList();
			sceneSelect.Bind(property.serializedObject);
			sceneSelect.bindingPath = property.propertyPath;
			sceneSelect.label = property.displayName;
			container.Add(sceneSelect);
			return container;

			// var bpo = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(new BuildPlayerOptions());
			// sceneSelect.choices = bpo.scenes.ToList();
			// sceneSelect.Bind(property.serializedObject);
			// sceneSelect.label = "Scene";
			//

		}
	}
}