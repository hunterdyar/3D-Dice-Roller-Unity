using UnityEditor;
using UnityEngine;

public static class CustomHandle
{
	public static bool DoHandle(Vector3 worldpos, float size, float pickSize)
	{
		int id = GUIUtility.GetControlID(FocusType.Passive);
		Event evt = Event.current;

		bool clicked = false;

		switch (evt.GetTypeForControl(id))
		{
			case EventType.MouseDown:
				if (evt.button == 0 && HandleUtility.nearestControl == id)
				{
					GUIUtility.hotControl = id;

					evt.Use(); // Using the MouseDown event
					clicked = true;
				}

				break;

			case EventType.MouseMove:
				HandleUtility.Repaint();
				evt.Use(); // Using the MouseMove event
				break;

			case EventType.MouseUp:
				if (evt.button == 0 && HandleUtility.nearestControl == id)
				{
					GUIUtility.hotControl = 0;
					evt.Use(); // Using the MouseUp event
				}

				break;

			case EventType.Layout:
				HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(worldpos, pickSize));
				// Keep in mind Layout events should not be Used!
				break;

			case EventType.Repaint:
				// Draw the handle here
				// Keep in mind Repaint events should not be Used!
				break;
		}

		return clicked;
	}
}