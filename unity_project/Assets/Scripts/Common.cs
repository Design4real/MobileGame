using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class SteerMode
{
	[System.Serializable]
	public class Abstract
	{
		public string name;
		//[line][column (from left to right)]
		protected List<List<WheelPair>> wheels;
		public List<List<Vector2>> deltas;
		public float maxAngle = 90;
		
		public virtual void init(List<List<WheelPair>> wheelLines)
		{
			wheels = new List<List<WheelPair>>();
			deltas = new List<List<Vector2>>();
			Vector3 center = new Vector3();
			int pairsCount = 0;
			foreach (List<WheelPair> curLine in wheelLines)
			{
				wheels.Add(new List<WheelPair>());
				foreach (WheelPair pair in curLine)
				{
					pairsCount++;
					center += pair.transform.position;
					wheels.Last().Add(pair);
				}
			}
			center /= pairsCount;
			foreach (List<WheelPair> curLine in wheelLines)
			{
				deltas.Add(new List<Vector2>());
				foreach (WheelPair pair in curLine)
				{
					float dz = center.z - pair.transform.position.z;
					float dx = center.x - pair.transform.position.x;
					deltas.Last().Add(new Vector2(dz, dx));
				}
			}
		}
		
		public virtual bool isCanMove(float desiredAngle)
		{
			return true;
		}
		
		public virtual float clampSteerAngle(float desiredAnlge)
		{
			return Mathf.Clamp(desiredAnlge, -maxAngle, maxAngle);
		}
		
		public virtual float getAngle(int line, int column, float desiredAngle)
		{
			throw new System.NotSupportedException();
		}

		public virtual Types getSteerModeType()
		{
			throw new System.NotSupportedException();
		}
	}
	
	[System.Serializable]
	public class Circle: Abstract
	{
		public List<List<float>> circleAngles;
		
		public Circle()
		{
			name = "Circle steering";
		}
		
		public override void init (List<List<WheelPair>> wheelLines)
		{
			base.init (wheelLines);
			
			circleAngles = new List<List<float>>();
			
			for (int i = 0; i < wheels.Count; i++)
			{
				circleAngles.Add(new List<float>());
				for (int j = 0; j < wheels[i].Count; j++)
				{
					Vector2 curDelta = deltas[i][j];
					float alpha = 180 -  Mathf.Atan2(curDelta.x, curDelta.y) * Mathf.Rad2Deg;
					if (wheels[i][j].platform.isReversed)
					{
						//alpha *= -1f;
					}
					if (alpha > 180)
					{
						alpha = alpha - 360f;
					}
					circleAngles.Last().Add(alpha);
				}
			}
		}
		
		public override float getAngle (int line, int column, float desiredAngle)
		{
			wheels[line][column].isMotorTorqueReversed = false;
			return circleAngles[line][column];
		}

		public override Types getSteerModeType()
		{
			return Types.Circle;
		}
	}
	
	[System.Serializable]
	public class DiagonalAlong: Abstract
	{
		public DiagonalAlong()
		{
			name = "Diagonal steering along";
			maxAngle = 70;
		}
		
		public override float getAngle (int line, int column, float desiredAngle)
		{
			wheels[line][column].isMotorTorqueReversed = false;
			return desiredAngle;
		}

		public override Types getSteerModeType()
		{
			return Types.DiagonalAlong;
		}
	}
	
	[System.Serializable]
	public class RearWheel: Abstract
	{
		public RearWheel()
		{
			name = "Rear-wheel steering";
		}
		
		public override float getAngle (int line, int column, float desiredAngle)
		{
			wheels[line][column].isMotorTorqueReversed = false;
			float rez = desiredAngle * line / (wheels.Count - 1f);
			return rez;
		}

		public override Types getSteerModeType()
		{
			return Types.RearWheel;
		}
	}
	
	[System.Serializable]
	public class FrontWheel: Abstract
	{
		public FrontWheel()
		{
			name = "Front-wheel steering";
		}
		
		public override float getAngle (int line, int column, float desiredAngle)
		{
			wheels[line][column].isMotorTorqueReversed = false;
			float rez = desiredAngle * (1f - line / (wheels.Count - 1f));
			return rez;
		}

		public override Types getSteerModeType()
		{
			return Types.FrontWheel;
		}
	}
	
	[System.Serializable]
	public class DiagonalCross: Abstract
	{
		public DiagonalCross()
		{
			name = "Diagonal steering cross";
			maxAngle = 35;
		}
		
		public override float getAngle (int line, int column, float desiredAngle)
		{
			//float sign = (column % 2 == 0) ? 1f : -1f;
			float sign = (column + 1) > (wheels[line].Count / 2f) ? -1f : 1f;
			if (wheels[line][column].platform.isReversed)
			{
				sign *= -1f;
			}
			
			float rez = (90f + 15f * sign) * sign + desiredAngle;
			
			wheels[line][column].isMotorTorqueReversed = sign == -1;
			if (desiredAngle < 0)
			{
				wheels[line][column].isMotorTorqueReversed = !wheels[line][column].isMotorTorqueReversed;
			}
			
			return rez;
		}
		
		/*public override bool isCanMove (float desiredAngle)
		{
			return !Mathf.Approximately(0, desiredAngle);
		}*/

		public override Types getSteerModeType()
		{
			return Types.DiagonalCross;
		}
	}
	
	[System.Serializable]
	public class AllWheelAlong: Abstract
	{
		public Vector2 maxDelta;
		
		public AllWheelAlong()
		{
			name = "All-wheel steering along";
		}
		
		public override void init (List<List<WheelPair>> wheelLines)
		{
			base.init (wheelLines);
			
			maxDelta.x = deltas.Max( a => a.Max( b => Mathf.Abs(b.x) ) );
			maxDelta.y = deltas.Max( a => a.Max( b => b.y ) );
		}
		
		public override float getAngle (int line, int column, float desiredAngle)
		{
			wheels[line][column].isMotorTorqueReversed = false;
			
			Vector2 curDelta = deltas[line][column];
			float alpha = desiredAngle * (Mathf.Abs(desiredAngle) / 180f) * Mathf.Sqrt(deltas[line][column].sqrMagnitude / maxDelta.sqrMagnitude);
			if (deltas[line][column].x > 0)
			{
				alpha *= -1;
			}
			return alpha;
		}

		public override Types getSteerModeType()
		{
			return Types.AllWheelAlong;
		}
	}
	
	[System.Serializable]
	public class AllWheelCross: Abstract
	{
		public Vector2 maxDelta;
		
		public AllWheelCross()
		{
			name = "All-wheel steering cross";
		}
		
		public override void init (List<List<WheelPair>> wheelLines)
		{
			base.init (wheelLines);
			
			maxDelta.x = deltas.Max( a => a.Max( b => Mathf.Abs(b.x) ) );
			maxDelta.y = deltas.Max( a => a.Max( b => b.y ) );
			
			/*for (int i = 0; i < deltas.Count; i++)
			{
				for (int j = 0; j < deltas[i].Count; j++)
				{
					deltas[i][j] -= new Vector2(maxDelta.x * 1.2f, 0);
				}
			}*/
		}
		
		public override float getAngle (int line, int column, float desiredAngle)
		{
			wheels[line][column].isMotorTorqueReversed = false;
			
			float sign = (column + 1) > (wheels[line].Count / 2f) ? -1f : 1f;
			if (wheels[line][column].platform.isReversed)
			{
				sign *= -1;
			}
			
			Vector2 curDelta = deltas[line][column];
			if (desiredAngle < 0)
			{
				curDelta += new Vector2(maxDelta.x * 1.2f, 0);
			}
			else
			{
				curDelta -= new Vector2(maxDelta.x * 1.2f, 0);
			}
			float alpha = Mathf.Atan2(curDelta.y, curDelta.x) * Mathf.Rad2Deg;
			if (desiredAngle > 0)
			{
				if (wheels[line][column].platform.isReversed)
				{
					alpha += 180 * sign;
				}
				else
				{
					alpha -= 180 * sign;
				}
				wheels[line][column].isMotorTorqueReversed = sign == -1;
			}
			else
			{
				wheels[line][column].isMotorTorqueReversed = sign == 1;
			}
			
			alpha *= Mathf.Abs(desiredAngle) / maxAngle;
			
			float rez = 90 * sign + alpha;
			return rez;
		}
		
		/*public override bool isCanMove (float desiredAngle)
		{
			return !Mathf.Approximately(0, desiredAngle);
		}*/

		public override Types getSteerModeType()
		{
			return Types.AllWheelCross;
		}
	}

	[System.Flags]
	public enum Types
	{
		None = 0,
		AllWheelAlong = 1 << 1,
		AllWheelCross = 1 << 2,
		Circle = 1 << 3,
		DiagonalAlong = 1 << 4,
		DiagonalCross = 1 << 5,
		FrontWheel = 1 << 6,
		RearWheel = 1 << 7
	}
}

public class EnumFlagsAttribute : PropertyAttribute
{
	public EnumFlagsAttribute() { }
}

public class SSSInput
{
	//Import the following.
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern System.IntPtr FindWindow(System.String className, System.String windowName);
	[DllImport("user32.dll")]
	private static extern System.IntPtr GetForegroundWindow();
	
	public enum InputSourceType
	{
		None,
		//Keyboard,
		Xbox,
		Custom
	}
	
	public enum InputType
	{
		//Start,
		Horizontal,
		Vertical,
		NextMode,
		NextCamera,
		Ok,
		Cancel,
		Tutorial,
		Steering_FrontWheel,
		Steering_RearWheel,
		Steering_AllWheelAlong,
		Steering_AllWheelCross,
		Steering_Circle,
		Steering_DiagonalAlong,
		Steering_DiagonalCross
	}
#region InputDictionaries
	/*private Dictionary<InputType, string> input_keyboard = new Dictionary<InputType, string>()
															{
																{InputType.Start,					"Keyboard Start"},
																{InputType.Horizontal,				"Keyboard Horizontal"},
																{InputType.Vertical,				"Keyboard Vertical"},
																{InputType.NextMode,				"Keyboard Next Movement Mode"},
																{InputType.NextCamera,				"Keyboard Next Camera"},
																{InputType.Ok,						"Keyboard Ok"},
																{InputType.Cancel,					"Keyboard Cancel"},
																{InputType.Tutorial,				"Keyboard Tutorial"},
																{InputType.Steering_FrontWheel,		"Keyboard Front-wheel steering"},
																{InputType.Steering_RearWheel,		"Keyboard Rear-wheel steering"},
																{InputType.Steering_AllWheelAlong,	"Keyboard All-wheel steering along"},
																{InputType.Steering_AllWheelCross,	"Keyboard All-wheel steering cross"},
																{InputType.Steering_Circle,			"Keyboard Circle steering"},
																{InputType.Steering_DiagonalAlong,	"Keyboard Diagonal steering"},
																{InputType.Steering_DiagonalCross,	"Keyboard Diagonal steering cross"}
															};*/
	
	private Dictionary<InputType, string> input_xbox = new Dictionary<InputType, string>()
															{
																//{InputType.Start,					"Xbox Start"},
																{InputType.Horizontal,				"Xbox Horizontal"},
																{InputType.Vertical,				"Xbox Vertical"},
																{InputType.NextMode,				"Xbox Next Movement Mode"},
																{InputType.NextCamera,				"Xbox Next Camera"},
																{InputType.Ok,						"Xbox Ok"},
																{InputType.Cancel,					"Xbox Cancel"},
																{InputType.Tutorial,				"Xbox Tutorial"},
																{InputType.Steering_FrontWheel,		"Xbox Front-wheel steering"},
																{InputType.Steering_RearWheel,		"Xbox Rear-wheel steering"},
																{InputType.Steering_AllWheelAlong,	"Xbox All-wheel steering along"},
																{InputType.Steering_AllWheelCross,	"Xbox All-wheel steering cross"},
																{InputType.Steering_Circle,			"Xbox Circle steering"},
																{InputType.Steering_DiagonalAlong,	"Xbox Diagonal steering along"},
																{InputType.Steering_DiagonalCross,	"Xbox Diagonal steering cross"}
															};

	private Dictionary<InputType, string> input_custom = new Dictionary<InputType, string>()
															{
																//{InputType.Start,					"Custom Start"},
																{InputType.Horizontal,				"Custom Horizontal"},
																{InputType.Vertical,				"Custom Vertical"},
																{InputType.NextMode,				"Custom Next Movement Mode"},
																{InputType.NextCamera,				"Custom Next Camera"},
																{InputType.Ok,						"Custom Ok"},
																{InputType.Cancel,					"Custom Cancel"},
																{InputType.Tutorial,				"Custom Tutorial"},
																{InputType.Steering_FrontWheel,		"Custom Front-wheel steering"},
																{InputType.Steering_RearWheel,		"Custom Rear-wheel steering"},
																{InputType.Steering_AllWheelAlong,	"Custom All-wheel steering along"},
																{InputType.Steering_AllWheelCross,	"Custom All-wheel steering cross"},
																{InputType.Steering_Circle,			"Custom Circle steering"},
																{InputType.Steering_DiagonalAlong,	"Custom Diagonal steering along"},
																{InputType.Steering_DiagonalCross,	"Custom Diagonal steering cross"}
															};
#endregion
	
	private Dictionary<InputType, string> selectedInput = null;
	
	private bool checkSelectedInput()
	{
		if (selectedInput == null)
		{
			if (Application.loadedLevel != 0)
			{
				Debug.LogError("Input source type is not selected");
			}
			return false;
		}
		return true;
	}
	
	public InputSourceType selectedInputSourceType
	{
		get
		{
			/*if (selectedInput == input_keyboard)
			{
				return InputSourceType.Keyboard;
			}*/
			if (selectedInput == input_xbox)
			{
				return InputSourceType.Xbox;
			}
			if (selectedInput == input_custom)
			{
				return InputSourceType.Custom;
			}
			return InputSourceType.None;
		}
	}
	
	public void setInputSourceType(InputSourceType type)
	{
		/*if (type == InputSourceType.Keyboard)
		{
			selectedInput = input_keyboard;
		}
		else */if (type == InputSourceType.Xbox)
		{
			selectedInput = input_xbox;
		}
		else if (type == InputSourceType.Custom)
		{
			selectedInput = input_custom;
		}
		else
		{
			selectedInput = null;
		}
		
		//Get the window handle.
		System.IntPtr windowPtr = GetForegroundWindow();
		//Set the title text using the window handle.
		SetWindowText(windowPtr, "MEGA MOVE (" + Common.selectedInputSourceTypeString + " input)");
	}
	
	public bool GetButtonDown(InputType type, out InputSourceType source)
	{
		source = InputSourceType.None;
		if (Input.GetJoystickNames().Any( a => a.Contains("XBOX") ))
		{
			source = InputSourceType.Xbox;
		}
		else
		{
			source = InputSourceType.Custom;
		}
		/*if (Input.GetButtonDown(input_keyboard[type]))
		{
			source = InputSourceType.Keyboard;
		}
		if (Input.GetButtonDown(input_xbox[type]))
		{
			source = InputSourceType.Xbox;
		}
		if (Input.GetButtonDown(input_custom[type]))
		{
			source = InputSourceType.Custom;
		}*/
		return (source != InputSourceType.None);
	}
	public bool GetButtonDown(InputType type)
	{
		if (!checkSelectedInput())
		{
			return false;
		}

		return CFInput.GetButtonDown(selectedInput[type]);
	}
	
	public float GetAxis(InputType type)
	{
		if (!checkSelectedInput())
		{
			return 0;
		}
		
		return Input.GetAxis(selectedInput[type]);
	}
	
	public float GetAxisRaw(InputType type)
	{
		if (!checkSelectedInput())
		{
			return 0;
		}
		if ( (CFInput.GetAxisRaw (selectedInput [type])) != 0 ) {Debug.Log (CFInput.GetAxisRaw (selectedInput [type]));}
		return CFInput.GetAxisRaw(selectedInput[type]);
	}
}

public class Common
{
	private static SSSInput _input;
	public static SSSInput input
	{
		get
		{
			if (_input == null)
			{
				Debug.Log("null");
				_input = new SSSInput();
				if (Application.loadedLevel != 0)
				{
					_input.setInputSourceType(SSSInput.InputSourceType.Custom);
				}
			}
			return _input;
		}
	}
	
	public static SSSInput.InputSourceType selectedInputSourceType
	{
		get
		{
			if (_input == null)
			{
				return SSSInput.InputSourceType.None;
			}
			return _input.selectedInputSourceType;
		}
	}
	
	public static string selectedInputSourceTypeString
	{
		get
		{
			if (_input == null)
			{
				return "NULL";
			}
			return _input.selectedInputSourceType.ToString();
		}
	}
}