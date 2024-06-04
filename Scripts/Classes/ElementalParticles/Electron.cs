using Godot;

namespace ParticleSimulation;

public class Electron: Particle
{
    public Electron(Vector2 position) : base(position, 9.1093837E-31, new Color(0.345f, 0.714f, 1), 0.004f) //0.0015
    {

    }
}
