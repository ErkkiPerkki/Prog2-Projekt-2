using Godot;
using System;
using System.Threading;

public partial class Canvas : ColorRect
{
    static int particles = 100;
    static Random random = new();

    public void Regenerate()
    {
        Image img = Image.Create(particles, 1, false, Image.Format.Rgb8);
        float aspectRatio = Size.Y / Size.X;

        for (int x = 0; x < particles; x++) {
            float rx = 0f;
            float ry = 0f;
            float rs = random.NextSingle() / 100f;
            Color color = new Color(rx, ry, rs);

            img.SetPixel(x, 0, color);
        }
        ImageTexture texture = ImageTexture.CreateFromImage(img);
        Material.Set("shader_parameter/particleData", texture);
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
}
