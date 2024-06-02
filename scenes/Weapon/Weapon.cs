using Godot;
using System;

public partial class Weapon : Node
{
    public virtual void load(int magazine)
    {
        
    }
    public virtual double shoot(Player player)
    {
        return 0;
    }

    public virtual void drop(Vector3 dropLocation)
    {

    }
}
