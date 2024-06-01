using Godot;
using System;

public partial class fuzhu_bugaa : CharacterBody3D
{	
	enum RobotState 
	{
		TRACKING,
		ATTACKING
	}
	
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
	}
	
	public override void _PhysicsProcess(double delta)
	{			
		GD.Print($"Robot state: {this.state}");
		
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
				
				Velocity = direction*1.2f;
			} break;
		}
		
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
	
	private void _on_timer_timeout()
	{
		this.state = RobotState.TRACKING;
		area.SetDeferred("monitoring", true);		
	}
}
