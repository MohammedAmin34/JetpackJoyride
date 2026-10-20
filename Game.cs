using Godot;
using System;

public partial class Game : Node2D
{

	Label scoreLabel;
	Label totalScoreLabel;
	int score = 0;
	float distance = 0;
	Marker2D SpawnPoint;
	[Export]
	PackedScene plasmaPackedScene, CoinPattern1, CoinPattern2;
	
	CanvasLayer GameOverScreen;
	
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
		GameOverScreen = GetNode<CanvasLayer>("GameOverScreen");
		GameOverScreen.ProcessMode = ProcessModeEnum.Always;
		GameOverScreen.Hide();
		var button = GetNode<Button>("GameOverScreen/Restart");
		button.Pressed += OnRestartPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		distance += Plasma.speed * (float)delta;
		totalScoreLabel.Text = "Distance: " + ((int)(distance / 10)).ToString() + "m";
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
		Random random = new Random();
		int offset = random.Next(-160, 170);
		Vector2 mPosition = SpawnPoint.Position;
		AddChild(plasma);
		mPosition.Y += offset;
		plasma.Position = mPosition;
	}	

	public void OnCoinSpawnerTimeOut()
	{
		Random random = new Random();
		int pattern = random.Next(1, 3);
		PackedScene coinPattern = pattern == 1 ? CoinPattern1 : CoinPattern2;

		var coins = coinPattern.Instantiate<Node2D>();
		coins.Position = SpawnPoint.Position;
		AddChild(coins);
	}

	public void OnCoinTimerTimeout()
	{
		OnCoinSpawnerTimeOut();
	}

	public void UpdateScore()
	{
		score += 1;
		scoreLabel.Text = "Score: " + score.ToString();
	}
}
