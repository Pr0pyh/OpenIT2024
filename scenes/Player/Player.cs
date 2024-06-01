using Godot;
using System;

public partial class Player : CharacterBody3D
{
    //enum state
    enum STATE 
    {
        MOVING,
        SHOOTING
    }
    STATE state;

    //export promenjive
    [Export]
    public float speed;
    [Export]
    public float sensitivity;
    //private scene promenjive
    Camera3D camera;
    Camera3D viewportCamera;
    Weapon weapon;
    Node3D inventory;
    Timer timer;
    //private promenjive
    float mouseMove;
    float sway = 5;
    int health = 100;
    double stamina = 50;
    double amount;
    double trauma;
    //public override funkcije
    public override void _Ready()
    {
        //pristupanje scenama
        camera = GetNode<Camera3D>("Camera3D");
        viewportCamera = camera.GetNode<SubViewportContainer>("SubViewportContainer").GetNode<SubViewport>("SubViewport").GetNode<Camera3D>("Camera3D");
        inventory = camera.GetNode<Node3D>("Inventory");
        timer = GetNode<Timer>("Timer");
        //inicijalizacija
        state = STATE.MOVING;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        timer.Start();
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
            mouseMove = -mouseMotion.Relative.X;
            camera.RotateX(Mathf.DegToRad(mouseMotion.Relative.Y*sensitivity*-1));
			camera.RotationDegrees = new Vector3(Mathf.Clamp(camera.RotationDegrees.X, -75.0f, 75.0f), 0.0f, 0.0f);
			this.RotateY(Mathf.DegToRad(mouseMotion.Relative.X*sensitivity*-1));
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        switch(state)
        {
            case STATE.MOVING:
                moveState(delta);
                swayMove(delta);
                shootState();
                shakeState(delta);
                exitState();
                cameraUpdate();
                break;
            case STATE.SHOOTING:
                break;
        }
    }

    //privatne funkcije
    private void cameraUpdate()
    {
        viewportCamera.GlobalTransform = camera.GlobalTransform;
    }
    private void exitState()
    {
        if(Input.IsActionPressed("exit"))
            GetTree().Quit();
    }
    private void moveState(double delta)
    {
        Vector3 moveVector = new Vector3(0.0f, 0.0f, 0.0f);
        if(Input.IsActionPressed("up"))
            moveVector -= camera.GlobalTransform.Basis.Z;
        if(Input.IsActionPressed("down"))
            moveVector += camera.GlobalTransform.Basis.Z;
        if(Input.IsActionPressed("right"))
            moveVector += camera.GlobalTransform.Basis.X;
        if(Input.IsActionPressed("left"))
            moveVector -= camera.GlobalTransform.Basis.X;
        moveVector = new Vector3(moveVector.X, 0.0f, moveVector.Z);

        //sprint
        if(Input.IsActionPressed("sprint") && stamina>0)
        {
            stamina -= 25*delta;
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }
        
        if(Input.IsActionJustReleased("sprint"))
        {
            stamina = 50.0f;
            speed = 5f;
        }

        Velocity = moveVector*speed;
        MoveAndSlide();
    }

    private void shootState()
    {
        if(Input.IsActionJustPressed("shoot") && weapon != null)
        {
            trauma = weapon.shoot(this);
        }
    }
    private void swayMove(double delta)
    {
        if(mouseMove != null)
        {
            if(mouseMove > sway)
                inventory.Rotation = inventory.Rotation.Lerp(new Godot.Vector3(0.0f, 0.1f, 0.0f), (float)(delta*5));
            else if(mouseMove < -sway)
                inventory.Rotation = inventory.Rotation.Lerp(new Godot.Vector3(0.0f, -0.1f, 0.0f), (float)(delta*5));
            else
				inventory.Rotation = inventory.Rotation.Lerp(new Godot.Vector3(0.0f, 0.0f, 0.0f), (float)(delta*5));
        }
    }
    private void shake()
    {
        amount = trauma;
		camera.HOffset = (float)(1 * amount * GD.RandRange(-1, 1));
		camera.VOffset = (float)(1 * amount * GD.RandRange(-1, 1));
    }
    private void shakeState(double delta)
	{
		if(!(trauma < 0.0))
		{
			shake();
			trauma = Mathf.Max((float)(trauma - 0.8*delta), 0.0f);
		}
	}
    //public metode
    public void pickup(PackedScene weaponScene)
    {
        if(weapon != null) weapon.drop(GlobalPosition);
        Weapon weaponInstance = (Weapon)weaponScene.Instantiate();
        inventory.AddChild(weaponInstance);
        weapon = weaponInstance;
    }
    public void heal(int count)
    {
        if(health <= 120)
            health += count;
    }
    public void _on_timer_timeout()
    {
        health -= 1;
        timer.Start();
        GD.Print(health);
        if(health <= 0)
            GetTree().ReloadCurrentScene();
    }
}
