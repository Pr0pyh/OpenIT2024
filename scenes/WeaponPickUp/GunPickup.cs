using Godot;
using System;

public partial class GunPickup : Area3D
{
    [Export]
    public PackedScene gunScene;
    [Export]
    public PackedScene gunModelScene;
    [Export]
    public int magazine;
    //privatne scene
    Label label;
    Player player;
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        label = GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("Label");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("bob");
        label.Visible = false;
        Node3D gunModel = (Node3D)gunModelScene.Instantiate();
        AddChild(gunModel);
    }
    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionJustPressed("pickup") && player != null)
        {
            player.pickup(gunScene, magazine);
            label.Visible = false;
            player = null;
            QueueFree();
        }
    }
    public void _on_body_entered(Node body)
    {
        label.Visible = true;
        if(body.GetType() == typeof(Player))
        {
            player = (Player)body;
        }
    }

    public void _on_body_exited(Node body)
    {
        player = null;
        label.Visible = false;
    }
}
