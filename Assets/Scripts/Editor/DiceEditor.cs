using HDyar.DiceRoller;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

	[CustomEditor(typeof(Dice))]
	public class DiceEditor : Editor
	{
		
		public void OnSceneGUI()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
					RaycastHit hitInfo;
					//todo: change this to a button toggle thing
					if (Event.current.control)
					{
						if (Physics.Raycast(worldRay, out hitInfo))
						{
							var myDice = (target as Dice);
							if (hitInfo.collider == myDice.GetComponentInChildren<Collider>())
							{
								var t = myDice.transform;
								var dfm = new DiceFaceOnModel();
								
								dfm.ModelNormal = t.InverseTransformDirection(hitInfo.normal);
								myDice.AddFace(dfm);
								Undo.RegisterCompleteObjectUndo(target,"Add DiceFaceOnmodel");
								Event.current.Use();
							}
						}
					}

				}
			}
			
			if (GUI.changed)
				EditorUtility.SetDirty(target);
		}

	}
