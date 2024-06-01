using Godot;
using System;

public partial class GunPickup : Area3D
{
    [Export]
    PackedScene gunScene;
    //privatne scene
    Label label;
    Player player;
    public override void _Ready()
    {
        label = GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("Label");
        label.Visible = false;
    }
    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionJustPressed("pickup") && player != null)
        {
            player.pickup(gunScene);
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
