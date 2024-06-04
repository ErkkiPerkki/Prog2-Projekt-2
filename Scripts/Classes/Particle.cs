using Godot;

namespace ParticleSimulation;

public class Particle
{
    private Vector2 _Position;
    private Vector2 _Velocity;
    private int _ID;
    private float _Size;
    private double _Mass;
    private Color _Color;

    public Vector2 Position {
        get { return _Position; }
        set {
            _Position = value;
        }
    }
    public Vector2 Velocity {get{return _Velocity;} set{_Velocity = value;}}
    public int ID {get{return _ID;}}
    public float Size {get{return _Size;}}
    public double Mass {get{return _Mass;}}
    public Color Color {get{return _Color;} set{_Color = value;}}

    public Particle(Vector2 position, double mass, Color color, float size = 0.01f)
    {
        _Position = position;
        _Size = size;
        _Mass = mass;
        _Color = color;
        _Velocity = Vector2.Zero;
        _ID = GetID();
    }

    private static int largestID = -1;
    private static int GetID()
    {
        largestID++;
        return largestID;
    }
}
