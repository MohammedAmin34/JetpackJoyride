using Godot;
using System;


public partial class Barry : CharacterBody2D
{
	[Signal]
	public delegate void PlayerTouchEventHandler();

	[Signal]
	public delegate void CoinCollectedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var mVelocity = Velocity;
		if (!IsOnFloor())
		{
			mVelocity += GetGravity() * (float)delta;
		}

		if(Input.IsActionPressed("Fly"))
		{
			mVelocity += -GetGravity() * (float)delta;
			mVelocity += -GetGravity() * (float)delta * 2;

		}
		
		Velocity = mVelocity;

		var mPosition = Position;
		mPosition.X = 0;
		Position = mPosition;

		MoveAndSlide();

	
	}
		public void OnPlayerTouch(Node2D body)
		{
			if (body is Plasma)
			{
			EmitSignal(SignalName.PlayerTouch);
			
			GD.Print("Your dead");
			}
		}

		public void OnAreaEntered(Area2D area)
	{
		if(area.GetParent() is Coins)
		{
			EmitSignal(SignalName.CoinCollected);
			GD.Print("You have collected a coin!");
		}
		
	}

		public void OnRestartPressed()
{
   	 		GetTree().Paused = false;
    		GetTree().ReloadCurrentScene();
}
	
}

