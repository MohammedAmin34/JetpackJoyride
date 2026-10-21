using Godot;


public partial class Missile : StaticBody2D
{
	static public float speed = 700;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		var mPosition = Position;
		mPosition.X += -(speed) * (float)delta;
		Position = mPosition;
	}

}
