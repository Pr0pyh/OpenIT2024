using Godot;
using System;
using System.Reflection;

public partial class Gun : Weapon
{
    AnimationPlayer animPlayer;
    RayCast3D rayCast;
    PackedScene gunPickupScene;
    RandomNumberGenerator random;
    Node3D flash;
    Label label;
    //private promenjive
    int magazine = 7;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        label = GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("Label");  
        gunPickupScene = ResourceLoader.Load<PackedScene>("res://scenes/WeaponPickUp/GunPickup.tscn");
        random = new RandomNumberGenerator();
        rayCast = GetNode<RayCast3D>("RayCast3D");
        flash = GetNode<Node3D>("Flash");
        flash.Visible = false;
        label.Text = "" + magazine;
    }
    public override void _Process(double delta)
    {
        label.Text = "" + magazine;
    }
    public override double shoot(Player player)
    {
        if(magazine <= 0)
        {
            animPlayer.Play("break");
            return 0.0f;
        }
        random.Randomize();
        rayCast.TargetPosition = new Godot.Vector3(random.RandfRange(-1.0f, 1.0f), random.RandfRange(-1.0f, 1.0f), -20.0f);
		rayCast.ForceRaycastUpdate();
        Vector3 collisionPoint = rayCast.GetCollisionPoint();
        if(rayCast.IsColliding())
            flash.LookAt(collisionPoint);
        if(rayCast.IsColliding() && rayCast.GetCollider().GetType() == typeof(Enemy))
        {
            Enemy enemy = (Enemy)rayCast.GetCollider();
            player.heal(enemy.damage(3, player));
        }
        magazine -= 1;
        animPlayer.Stop();
        animPlayer.Play("shoot");
        return 0.03;
    }

    public override void drop(Vector3 dropLocation)
    {
        GunPickup gunPickup = (GunPickup)gunPickupScene.Instantiate();
        gunPickup.gunScene = ResourceLoader.Load<PackedScene>("res://scenes/Weapon/Gun/Gun.tscn");
        gunPickup.gunModelScene = ResourceLoader.Load<PackedScene>("res://assets/models/gun/pistoljcePick.tscn");
        GetParent().GetParent().GetParent().GetParent().AddChild(gunPickup);
        gunPickup.GlobalPosition = dropLocation;
        QueueFree();
    }
}
