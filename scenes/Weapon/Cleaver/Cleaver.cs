using Godot;
using System;

public partial class Cleaver : Weapon
{
    AnimationPlayer animPlayer;
    PackedScene gunPickupScene;
    RayCast3D rayCast;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");   
        gunPickupScene = ResourceLoader.Load<PackedScene>("res://scenes/WeaponPickUp/GunPickup.tscn");
        rayCast = GetNode<RayCast3D>("RayCast3D");
    }
    public override double shoot(Player player)
    {
        rayCast.ForceRaycastUpdate();
        animPlayer.Play("shoot");
        if(rayCast.IsColliding() && rayCast.GetCollider().GetType() == typeof(Enemy))
        {
            Enemy enemy = (Enemy)rayCast.GetCollider();
            player.heal(enemy.damage(5, player));
            return 0.2f;
        }
        return 0.01;
    }

    public override void drop(Vector3 dropLocation)
    {
        GunPickup gunPickup = (GunPickup)gunPickupScene.Instantiate();
        gunPickup.gunScene = ResourceLoader.Load<PackedScene>("res://scenes/Weapon/Cleaver/Cleaver.tscn");
        GetParent().GetParent().GetParent().GetParent().AddChild(gunPickup);
        gunPickup.GlobalPosition = dropLocation;
        QueueFree();
    }
}
