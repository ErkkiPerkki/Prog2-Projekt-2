using Godot;

namespace ParticleSimulation;

public class Particle
{
    private Vector2 _Position;
    private float _Size;
    private double _Mass;
    private Color _Color;

    public Vector2 Position {
        get { return _Position; }
        set {
            if (value.X > 1 || value.X < 0) return;
            if (value.Y > 1 || value.Y < 0) return;
            _Position = value;
        }
    }
    public float Size {get{return _Size;}}
    public double Mass {get{return _Mass;}}
    public Color Color {get{return _Color;}}

    public Particle(Vector2 position, double mass, Color color, float size = 0.01f)
    {
        _Position = position;
        _Size = size;
        _Mass = mass;
        _Color = color;
    }
}
