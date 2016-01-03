using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class XboxInput : MonoBehaviour 
{
	PlayerIndex player1Index;
	PlayerIndex player2Index;

	GamePadState player1State;
	GamePadState player1PrevState;
	GamePadState player2State;
	GamePadState player2PrevState;

	bool m_isWorking;

	// Use this for initialization
	void Start () 
	{
		m_isWorking = true;
		ConnectControllers ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		player1State = GamePad.GetState (player1Index);
		player2State = GamePad.GetState (player2Index);
	}

	void LateUpdate()
	{
		player1PrevState = player1State;
		player2PrevState = player2State;
	}

	void ConnectControllers()
	{
		PlayerIndex testPlayerIndex = (PlayerIndex)0;
		GamePadState testState = GamePad.GetState (testPlayerIndex);
		if (testState.IsConnected) {
			Debug.Log ("Player 1 Connected");
			player1Index = testPlayerIndex;
			player1State = testState;
		} else {
			m_isWorking = false;
		}
		
		testPlayerIndex = (PlayerIndex)1;
		testState = GamePad.GetState (testPlayerIndex);
		if (testState.IsConnected) {
			Debug.Log ("Player 2 Connected");
			player2Index = testPlayerIndex;
			player2State = testState;
		}
	}

	public bool IsWorking()
	{
		return m_isWorking;
	}

	public float GetAxis(string axisName)
	{
		switch (axisName) {
		case "Vertical":
			return player1State.ThumbSticks.Left.Y;

		case "Horizontal":
			return player1State.ThumbSticks.Left.X;

		case "Rotate":
			return player1State.ThumbSticks.Right.X;
		
		case "Arm":
			if(player2State.DPad.Right == ButtonState.Pressed)
				return 1f;
			else if(player2State.DPad.Left ==  ButtonState.Pressed)
				return -1f;
			else if(player1State.DPad.Right == ButtonState.Pressed)
				return 1f;
			else if(player1State.DPad.Left ==  ButtonState.Pressed)
				return -1f;
			else
				return 0;

		case "Lift":
			float lift;
			if(player1State.Buttons.RightShoulder == ButtonState.Pressed)
				lift = 1f;
			else if (player1State.Triggers.Right > 0)
				lift = -1f;
			else if (player2State.Buttons.RightShoulder == ButtonState.Pressed)
				lift = 1f;
			else if (player2State.Triggers.Right > 0)
				lift = -1f;
			else
				lift = 0f;
			return lift;

		default:
			return 0f;
		}

		return 0f;
	}

	public bool GetButtonDown(string buttonName)
	{
		switch (buttonName) {
		case "Block Clamp":
			bool blockClamp;
			blockClamp = player2State.IsConnected && player2PrevState.Buttons.A == ButtonState.Released && player2State.Buttons.A == ButtonState.Pressed;
			if(!blockClamp)
				blockClamp = player2State.IsConnected && player2PrevState.Buttons.Y == ButtonState.Released && player2State.Buttons.Y == ButtonState.Pressed;
			if(!blockClamp)
				blockClamp = player1PrevState.Buttons.X == ButtonState.Released && player1State.Buttons.X == ButtonState.Pressed;
			if(!blockClamp)
				blockClamp = player1PrevState.Buttons.B == ButtonState.Released && player1State.Buttons.B == ButtonState.Pressed;
			return blockClamp;

		case "Skyrise Clamp":
			bool skyriseClamp;
			skyriseClamp = player1PrevState.DPad.Up == ButtonState.Released && player1State.DPad.Up == ButtonState.Pressed;
			if(!skyriseClamp)
				skyriseClamp = player1PrevState.DPad.Down == ButtonState.Released && player1State.DPad.Down == ButtonState.Pressed;
			if(!skyriseClamp)
				skyriseClamp = player2PrevState.DPad.Up == ButtonState.Released && player2State.DPad.Up == ButtonState.Pressed;
			if(!skyriseClamp)
				skyriseClamp = player2PrevState.DPad.Down == ButtonState.Released && player2State.DPad.Down == ButtonState.Pressed;
			return skyriseClamp;

		case "Camera View":
			bool cameraView; 
			cameraView = player1PrevState.Buttons.Back == ButtonState.Released && player1State.Buttons.Back == ButtonState.Pressed;
			if(!cameraView)
				cameraView = player2PrevState.Buttons.Back == ButtonState.Released && player2State.Buttons.Back == ButtonState.Pressed;
			return cameraView;

		case "Start":
			bool start; 
			start = player1PrevState.Buttons.Start == ButtonState.Released && player1State.Buttons.Start == ButtonState.Pressed;
			if(!start)
				start = player2PrevState.Buttons.Start == ButtonState.Released && player2State.Buttons.Start == ButtonState.Pressed;
			return start;

		default:
			return false;
		}

		return false;
	}
}
