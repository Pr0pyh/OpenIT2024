using Godot;
using System;

public partial class fuzhu_bugaa : CharacterBody3D
{	
	enum RobotState 
	{
		TRACKING,
		ATTACKING
	}
	[Export]
    int health = 10;
    AnimationPlayer animPlayer;
	RobotState state;
	Vector3 lastPlayerPos;
	Player player;
	NavigationAgent3D agent;
	Area3D area;
	Timer timer;
	
	public override void _Ready()
	{
		lastPlayerPos = new Vector3();
		state = RobotState.TRACKING;
		player = GetParent().GetNode<Player>("Player");
		agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		area = GetNode<Area3D>("Area3D");
		timer = GetNode<Timer>("Timer");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	public override void _PhysicsProcess(double delta)
	{			
		
		switch (state) 
		{
			case (RobotState.TRACKING) : 
			{
				agent.TargetPosition = player.GlobalPosition;
		
				Vector3 direction = new Vector3();
				
				direction = agent.GetNextPathPosition() - this.GlobalPosition;
				direction.Y = 0.0f;
				direction = direction.Normalized();
				
				Velocity = Velocity.Lerp(direction, (float)delta * 0.2f);
			} break;
			case (RobotState.ATTACKING) : 
			{	
				Vector3 direction = new Vector3();
				direction = lastPlayerPos - this.GlobalPosition;
				direction.Y = 0.0f;
				
				Velocity = direction*1.1f;
			} break;
		}
		animPlayer.Play("running");
		MoveAndSlide();
	}
	
	private void _on_area_3d_body_entered(Node3D body)
	{
		if (body is Player playerBody)
		{
			timer.Start();
			this.state = RobotState.ATTACKING;
			lastPlayerPos = playerBody.GlobalPosition;
			area.SetDeferred("monitoring", false);
		}
	}
	
	public int damage(int count, Player player)
    {
        health -= count;
        GD.Print(health);
        if(health <= 0)
        {
            QueueFree();
            return 10;
        }
        return 0;
    }
	private void _on_timer_timeout()
	{
		this.state = RobotState.TRACKING;
		area.SetDeferred("monitoring", true);		
	}
}
