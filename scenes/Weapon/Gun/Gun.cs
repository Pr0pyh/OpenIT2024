using Godot;
using System;

public partial class Gun : Weapon
{
    AnimationPlayer animPlayer;
    PackedScene gunPickupScene;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");   
        gunPickupScene = ResourceLoader.Load<PackedScene>("res://scenes/WeaponPickUp/GunPickup.tscn");
    }
    public override void shoot()
    {
        animPlayer.Play("shoot");
    }

    public override void drop(Vector3 dropLocation)
    {
        GunPickup gunPickup = (GunPickup)gunPickupScene.Instantiate();
        gunPickup.GlobalPosition = dropLocation;
        GetParent().GetParent().GetParent().GetParent().AddChild(gunPickup);
        QueueFree();
    }
}
