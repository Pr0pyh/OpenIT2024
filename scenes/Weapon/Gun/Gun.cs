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
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");   
        gunPickupScene = ResourceLoader.Load<PackedScene>("res://scenes/WeaponPickUp/GunPickup.tscn");
        random = new RandomNumberGenerator();
        rayCast = GetNode<RayCast3D>("RayCast3D");
        flash = GetNode<Node3D>("Flash");
        flash.Visible = false;
    }
    public override double shoot(Player player)
    {
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
        animPlayer.Play("shoot");
        return 0.03;
    }

    public override void drop(Vector3 dropLocation)
    {
        GunPickup gunPickup = (GunPickup)gunPickupScene.Instantiate();
        gunPickup.gunScene = ResourceLoader.Load<PackedScene>("res://scenes/Weapon/Gun/Gun.tscn");
        GetParent().GetParent().GetParent().GetParent().AddChild(gunPickup);
        gunPickup.GlobalPosition = dropLocation;
        QueueFree();
    }
}
