using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// USystem : all required game system information
/// With all information stored in USystem, a person can start game design.
/// </summary>

public class USystem {
	/* STATIC */
	public static USystem Current;
	public static List<USystem> Instances;

	/* INTERNALS */


	/* CONSTRUCTOR */
	public USystem(UControl new_control, UDifficulty new_difficulty, UStage new_stage) {

		control = new_control;
		difficulty = new_difficulty;
		stage = new_stage;

		//Adding static references
		USystem.Instances.Add (this);
		USystem.Current = this;
	}


	/* INSTANCE */
	public UControl control;
	public UDifficulty difficulty;
	public UStage stage;
}

public class UControl {
	/// <summary>
	/// Initializes a new instance of the <see cref="USystem+UControls"/> class with presets.
	/// </summary>
	/// <param name="should_use_right_preset">If set to <c>true</c> should use right preset.</param>
	public UControl(bool should_use_right_preset = true) {
		if (should_use_right_preset) {
			leftKey = KeyCode.LeftArrow;
			rightKey = KeyCode.RightArrow;
			upKey = KeyCode.UpArrow;
			downKey = KeyCode.DownArrow;
			focusKey = KeyCode.LeftShift;
			shootKey = KeyCode.Z;
			bombKey = KeyCode.X;
		} else {
			leftKey = KeyCode.A;
			rightKey = KeyCode.D;
			upKey = KeyCode.W;
			downKey = KeyCode.S;
			focusKey = KeyCode.RightShift;
			shootKey = KeyCode.Greater;
			bombKey = KeyCode.Question;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="USystem+UControls"/> class.
	/// </summary>
	/// <param name="left">Left.</param>
	/// <param name="right">Right.</param>
	/// <param name="up">Up.</param>
	/// <param name="down">Down.</param>
	/// <param name="focus">Focus.</param>
	/// <param name="shoot">Shoot.</param>
	/// <param name="bomb">Bomb.</param>
	/// <param name="extras">Extras.</param>
	public UControl(KeyCode left, KeyCode right, KeyCode up, KeyCode down, KeyCode focus, KeyCode shoot, KeyCode bomb, List<KeyCode> extras) {
		leftKey = left;
		rightKey = right;
		upKey = up;
		downKey = down;
		focusKey = focus;
		shootKey = shoot;
		bombKey = bomb;
		extraKeys = extras;
	}

	public KeyCode leftKey = KeyCode.LeftArrow;
	public KeyCode rightKey = KeyCode.RightArrow;
	public KeyCode upKey = KeyCode.UpArrow;
	public KeyCode downKey = KeyCode.DownArrow;

	public KeyCode focusKey = KeyCode.LeftShift;
	public KeyCode shootKey = KeyCode.Z;
	public KeyCode bombKey = KeyCode.X;

	public List<KeyCode> extraKeys;
}

/// <summary>
/// Difficulty class
/// </summary>
public class UDifficulty {

	/// <summary>
	/// Initializes a new instance of the <see cref="USystem+UDifficulty"/> class.
	/// </summary>
	/// <param name="difficulty_count">Total difficulty amount.</param>
	/// <param name="current_difficulty">OPTIONAL: Current difficulty (lowest as 0).</param>
	public UDifficulty(int difficulty_count, int current_difficulty = 0) {
		_difficulty_count = difficulty_count;
		_current_difficulty = current_difficulty;
	}
	public UDifficulty(UDifficulty.Presets preset_count, UDifficulty.Presets current_preset = Presets.EASY) {
		_difficulty_count = (int) preset_count;
		_current_difficulty = (int) current_preset;
	}
	public UDifficulty() {
		_difficulty_count = (int) Presets.PRESET_COUNT;
		_current_difficulty = (int) Presets.EASY;
	}

	/// <summary>
	/// Difficulty presets, better naming.
	/// </summary>
	public enum Presets {
		EASY = 0,
		NORMAL = 1,
		HARD = 2,
		SUPER = 3,
		PRESET_COUNT = 4
	}

	/// <summary>
	/// how many difficulty levels
	/// </summary>
	private int _difficulty_count = 1;
	/// <summary>
	/// The current difficulty level (starting from 0 as the lowest difficulty).
	/// </summary>
	private int _current_difficulty = 0;


	/// <summary>
	/// Gets the total diffculty count.
	/// </summary>
	/// <returns>The total diffculty count.</returns>
	public int GetTotalDiffcultyCount() {
		return _difficulty_count;
	}

	/// <summary>
	/// Gets the current difficulty.
	/// </summary>
	/// <returns>The current difficulty.</returns>
	public int GetDifficulty() {
		return _current_difficulty;
	}
	/// <summary>
	/// Sets the current difficulty.
	/// </summary>
	/// <param name="new_difficulty">Difficulty level.</param>
	public void SetDifficulty(int new_difficulty) {
		if (new_difficulty >= _difficulty_count) {
			Debug.LogWarning ("Exceeding maximum difficulty level, set to the available highest difficulty level instead");
			_current_difficulty = _difficulty_count;
		} else if (new_difficulty < 0) {
			Debug.LogWarning ("Smaller than minimum difficulty level (level 0), set to lowest level 0 instead");
			_current_difficulty = 0;
		} else {
			_current_difficulty = new_difficulty;
		}
	}
}


public class UStage {

	/// <summary>
	/// Initializes stage information <see cref="USystem+UStage"/> class.
	/// </summary>
	/// <param name="stage_count">Stage count.</param>
	/// <param name="current_stage">Current stage.</param>
	public UStage(int stage_count, int current_stage = 0) {
		_stage_count = stage_count;
		_current_stage = current_stage;
	}

	/// <summary>
	/// how many stage levels
	/// </summary>
	private int _stage_count = 1;
	/// <summary>
	/// The current stage number (starting from 0 as the first stage).
	/// </summary>
	private int _current_stage = 0;


	/// <summary>
	/// Gets the total stage count.
	/// </summary>
	/// <returns>The total stage count.</returns>
	public int GetTotalStageCount() {
		return _stage_count;
	}

	/// <summary>
	/// Gets the current stage.
	/// </summary>
	/// <returns>The current stage.</returns>
	public int GetStage() {
		return _current_stage;
	}
	/// <summary>
	/// Sets the current stage.
	/// </summary>
	/// <param name="new_stage">New stage.</param>
	public void SetStage(int new_stage) {
		if (new_stage >= _stage_count) {
			Debug.LogWarning ("Exceeding maximum stage number, set to the available highest stage number instead");
			_current_stage = _stage_count;
		} else if (new_stage < 0) {
			Debug.LogWarning ("Smaller than minimum stage number (stage 0), set to lowest stage 0 instead");
			_current_stage = 0;
		} else {
			_current_stage = new_stage;
		}
	}
}

public class UKey {
	//TODO: implement event feature
	public delegate void KeyAction();
	public static event KeyAction OnKey;
	public static event KeyAction OnKeyDown;
	public static event KeyAction OnKeyUp;

	public KeyCode keyCode;
}

public class TestClass : MonoBehaviour {

	public USystem sys;

	void Start() {
		sys = new USystem (new UControl(), new UDifficulty(), new UStage(6));
	}
}
