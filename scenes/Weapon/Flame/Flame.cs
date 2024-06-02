using Godot;
using System;

public partial class Flame : Weapon
{
    AnimationPlayer animPlayer;
    PackedScene gunPickupScene;
    RayCast3D rayCast;
    public int magazine = 0;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");   
        gunPickupScene = ResourceLoader.Load<PackedScene>("res://scenes/WeaponPickUp/GunPickup.tscn");
        rayCast = GetNode<RayCast3D>("RayCast3D");
    }
    public override double shoot(Player player)
    {
        rayCast.ForceRaycastUpdate();
        animPlayer.Stop();
        animPlayer.Play("shoot");
        if(rayCast.IsColliding() && rayCast.GetCollider().GetType() == typeof(Enemy))
        {
            Enemy enemy = (Enemy)rayCast.GetCollider();
            player.heal(enemy.damage(1, player));
            return 0.2f;
        }
        if(rayCast.IsColliding() && rayCast.GetCollider().GetType() == typeof(fuzhu_bugaa))
        {
            fuzhu_bugaa enemy = (fuzhu_bugaa)rayCast.GetCollider();
            player.heal(enemy.damage(1, player));
            return 0.2f;
        }
        return 0.01;
    }

    public override void drop(Vector3 dropLocation)
    {
        GunPickup gunPickup = (GunPickup)gunPickupScene.Instantiate();
        gunPickup.gunScene = ResourceLoader.Load<PackedScene>("res://scenes/Weapon/Cleaver/Cleaver.tscn");
        gunPickup.gunModelScene = ResourceLoader.Load<PackedScene>("res://assets/models/cleaver/sataraPick.tscn");
        gunPickup.magazine = magazine;
        GetParent().GetParent().GetParent().GetParent().AddChild(gunPickup);
        gunPickup.GlobalPosition = dropLocation;
        QueueFree();
    }
}
