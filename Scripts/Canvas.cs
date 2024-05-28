using Godot;
using System;
using System.Threading;

public partial class Canvas : ColorRect
{
    static int particles = 100;
    static Random random = new();

    Image particleDataImage = Image.Create(particles, 1, false, Image.Format.Rgb8);
    Image particleColorImage = Image.Create(particles, 1, false, Image.Format.Rgb8);

    public static Color[] ParticleColors = new Color[]
    {
        new Color(0.776f, 0.302f, 1f),
        new Color(1f, 1f, 1f),  
        new Color(0.302f, 0.776f, 1f)
    };

    public static Color GetRandomColor()
    {
        int randomIndex = random.Next(0, ParticleColors.Length);
        return ParticleColors[randomIndex];
    }

    public void Regenerate()
    {
        for (int x = 0; x < particles; x++) {
            float rx = random.NextSingle();
            float ry = random.NextSingle();
            float rs = random.NextSingle() / 100f + 0.1f;
            Color pos = new Color(rx, ry, rs);
            Color color = GetRandomColor();

            particleDataImage.SetPixel(x, 0, pos);
            particleColorImage.SetPixel(x, 0, color);
        }
        ImageTexture particleData = ImageTexture.CreateFromImage(particleDataImage);
        ImageTexture particleColors = ImageTexture.CreateFromImage(particleColorImage);
        Material.Set("shader_parameter/particleData", particleData);
        Material.Set("shader_parameter/particleColors", particleColors);

        GetNode<TextureRect>("%DebugTexture").Texture = particleColors;
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

    public override void _Process(double delta)
    {
        Regenerate();
    }
}
