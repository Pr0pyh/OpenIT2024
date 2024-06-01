using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
    [Export]
    int health = 10;
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public int damage(int count, Player player)
    {
        animPlayer.Play("damage");
        health -= count;
        if(health <= 0)
        {
            QueueFree();
            return 10;
        }
        return 0;
    }
}
