using Godot;
using System;

public partial class Game : Node2D
{

	Label scoreLabel;
	Label totalScoreLabel;
	int score = 0;
	float distance = 0;
	Marker2D SpawnPoint;
	Missile missile;
	Area2D missileArea;
	const float MissileSpeed = 700;
	[Export]
	PackedScene plasmaPackedScene, CoinPattern1, CoinPattern2;
	
	CanvasLayer GameOverScreen;
	private readonly Random _random = new Random();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		distance = 0;
		score = 0;
		SpawnPoint = GetNode<Marker2D>("PlasmaSpawner");
		scoreLabel = GetNode<Label>("CanvasLayer/Score");
		totalScoreLabel = GetNode<Label>("CanvasLayer/TotalScore");
		scoreLabel.Text = "Score: " + score.ToString();
		totalScoreLabel.Text = "0";
		missile = GetNode<Missile>("Missle");
		missileArea = missile.GetNode<Area2D>("Area2D");
		missileArea.BodyEntered += OnMissileBodyEntered;
		OnMissileTimeOut();
		GameOverScreen = GetNode<CanvasLayer>("GameOverScreen");
		GameOverScreen.ProcessMode = ProcessModeEnum.Always;
		GameOverScreen.Hide();
		var button = GetNode<Button>("GameOverScreen/Restart");
		button.Pressed += OnRestartPressed;
	}

	
	public override void _Process(double delta)
	{
		distance += Plasma.speed * (float)delta;
		totalScoreLabel.Text = "Distance: " + ((int)(distance / 5)).ToString() + "m";
		missile.Position += Vector2.Left * MissileSpeed * (float)delta;
	}

	private void OnMissileBodyEntered(Node2D body)
	{
		if (body is Barry)
		{
			OnBarryDeath();
		}
	}

	public void OnRestartPressed()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	public void OnBarryDeath()
	{
		GD.Print("Game Over");
		GameOverScreen.Show();
		GetTree().Paused = true;
	}

	public void OnSpawnTimerTimeout()
	{
		var plasma = plasmaPackedScene.Instantiate<Plasma>();
		Vector2 mPosition = SpawnPoint.Position;
		mPosition.Y = _random.Next(-200, 220);
		AddChild(plasma);
		plasma.Position = mPosition;
	}

	public void OnCoinSpawnerTimeOut()
	{
		int pattern = _random.Next(1, 3);
		PackedScene coinPattern = pattern == 1 ? CoinPattern1 : CoinPattern2;

		var coins = coinPattern.Instantiate<Node2D>();
		coins.Position = SpawnPoint.Position;
		coins.Position += new Vector2(0, _random.Next(-180, 180));
		AddChild(coins);
	}

	public void OnCoinTimerTimeout()
	{
		OnCoinSpawnerTimeOut();
	}



	public void OnMissileTimeOut()
	{
		missile.Position = new Vector2(1200, _random.Next(40, 600));
		OnSpawnTimerTimeout();
	}

	public void UpdateScore()
	{
		score += 1;
		scoreLabel.Text = "Score: " + score.ToString();
	}
}
