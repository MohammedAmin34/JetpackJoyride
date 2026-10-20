using Godot;
using System;

public partial class Coins : Node2D
{
	[Signal]
	public delegate void ScoreEventHandler();

	int updateScore = 1;
	static public float speed = 200;
	private bool isDead;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		var mPosition = Position;
		mPosition.X += -(speed) * (float)delta;
		Position = mPosition;
	}

	public void OnCoinsEntered(Node2D body)
	{
		if (isDead || body is not Barry)
			return;

		var game = GetTree().Root.GetNodeOrNull<Game>("Game");
		if (game != null)
		{
			//game.UpdateScore(updateScore);
		}

		EmitSignal(SignalName.Score);
		Die();
	}

	private void Die()
	{
		if (isDead)
			return;

		isDead = true;
		QueueFree();
	}
}
