using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    [Export]
    int health = 10;
    [Export]
    PackedScene weaponPickScene;
    [Export]
    PackedScene weapon;
    [Export]
    PackedScene weaponDisplay;
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public int damage(int count, Player player)
    {
        animPlayer.Play("damage");
        health -= count;
        GD.Print(health);
        if(health <= 0)
        {
            GunPickup weaponPick = (GunPickup)weaponPickScene.Instantiate();
            weaponPick.GlobalPosition = GlobalPosition;
            weaponPick.gunScene = weapon;
            weaponPick.gunModelScene = weaponDisplay;
            weaponPick.magazine = 7;
            GetParent().AddChild(weaponPick);
            QueueFree();
            return 10;
        }
        return 0;
    }
}
