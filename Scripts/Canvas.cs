using Godot;
using System;
using System.Collections.Generic;

namespace ParticleSimulation;

public partial class Canvas : ColorRect
{
    static int particleAmount = 250;
    static Random random = new();

    Image particleDataImage = Image.Create(particleAmount, 1, false, Image.Format.Rgb8);
    Image particleColorImage = Image.Create(particleAmount, 1, false, Image.Format.Rgb8);
    Image particleSizeImage = Image.Create(particleAmount, 1, false, Image.Format.Rh);
    ImageTexture particleData = new();
    ImageTexture particleColors = new();
    ImageTexture particleSizes = new();

    List<Particle> particles = new();

    public static Color[] ParticleColors = new Color[]
    {
        new Color(0.835f, 1.000f, 0.800f),
        new Color(0.584f, 1.000f, 0.812f),
        new Color(0.427f, 0.831f, 0.761f),
        new Color(0.337f, 0.639f, 0.698f),
        new Color(0.251f, 0.447f, 0.573f),
        new Color(0.173f, 0.259f, 0.451f),
        new Color(0.157f, 0.063f, 0.224f),
        new Color(0.051f, 0.012f, 0.078f)
    };

    public static Color GetRandomColor()
    {
        int randomIndex = random.Next(0, ParticleColors.Length);
        return ParticleColors[randomIndex];
    }

    public void Regenerate()
    {
        particles.Clear();

        for (int x = 0; x < particleAmount; x++) {
            float rx = random.NextSingle();
            float ry = random.NextSingle();

            Vector2 position = new(rx, ry);

            Electron electron = new(position);
            particles.Add(electron);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Regenerate")) {
            Regenerate();
        }
    }

    public void _OnResize()
	{
		Vector2 canvasSize = Size;
		Material.Set("shader_parameter/canvasSize", canvasSize);
	}

    Dictionary<string, float> distanceCache = new();
    public Vector2 ComputeForces(Particle p1)
    {
        Vector2 force = Vector2.Zero;

        for (int i = 0; i < particles.Count; i++) {
            Particle p2 = particles[i];
            if (p2.ID == p1.ID) continue;

            Vector2 dif = p2.Position - p1.Position;
            Vector2 dir = dif.Normalized();
            float distance = dif.Length();

            if (distanceCache.ContainsKey($"{p1.ID}-{p2.ID}")) {
                distance = distanceCache[$"{p1.ID}-{p2.ID}"];
            }
            else {
                distance = dif.Length();
                distanceCache[$"{p1.ID}-{p2.ID}"] = distance;
                distanceCache[$"{p2.ID}-{p1.ID}"] = distance;
            }
            if (distance <= (p1.Size+p2.Size)/2) continue;

            double localForce = 1E+54 * (p1.Mass * p2.Mass / (distance * distance));
            force += dir * (float)localForce;
        }

        return force;
    }

    public void UpdateParticles()
    {
        distanceCache.Clear();

        for (int i=0; i < particles.Count; i++) {
            Particle particle = particles[i];
            Vector2 force = ComputeForces(particle);
            particle.Velocity += force;
            particle.Position += particle.Velocity;
        }
    }

    public void RenderParticles()
    {
        for (int i = 0; i < particles.Count; i++) {
            Particle particle = particles[i];
            float x = particle.Position.X;
            float y = particle.Position.Y;
            Color data = new(x, y, particle.Size);
            Color sizeData = new(particle.Size, 0, 0);

            particleDataImage.SetPixel(i, 0, data);
            particleColorImage.SetPixel(i, 0, particle.Color);
            particleSizeImage.SetPixel(i, 0, sizeData);
        }

        particleData.SetImage(particleDataImage);
        particleColors.SetImage(particleColorImage);
        particleSizes.SetImage(particleSizeImage);
        Material.Set("shader_parameter/particleData", particleData);
        Material.Set("shader_parameter/particleColors", particleColors);
        Material.Set("shader_parameter/particleSizes", particleSizes);

        GetNode<TextureRect>("%DebugTexture").Texture = particleColors;
    }

    public override void _Process(double delta)
    {
        UpdateParticles();
        RenderParticles();
    }
}
