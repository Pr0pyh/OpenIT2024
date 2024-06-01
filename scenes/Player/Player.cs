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
    public int speed;
    [Export]
    public float sensitivity;
    //private scene promenjive
    Camera3D camera;
    Weapon weapon;
    Node3D inventory;
    //private promenjive
    //public override funkcije
    public override void _Ready()
    {
        //pristupanje scenama
        camera = GetNode<Camera3D>("Camera3D");
        inventory = camera.GetNode<Node3D>("Inventory");
        //inicijalizacija
        state = STATE.MOVING;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mouseMotion)
        {
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
                shootState();
                exitState();
                break;
            case STATE.SHOOTING:
                break;
        }
    }

    //privatne funkcije
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

        Velocity = moveVector*speed;
        MoveAndSlide();
    }

    private void shootState()
    {
        if(Input.IsActionJustPressed("shoot") && weapon != null)
            weapon.shoot();
    }
    //public metode
    public void pickup(PackedScene weaponScene)
    {
        if(weapon != null) weapon.drop(GlobalPosition);
        Weapon weaponInstance = (Weapon)weaponScene.Instantiate();
        inventory.AddChild(weaponInstance);
        weapon = weaponInstance;
    }
}
