using System.Threading.Tasks;

namespace RayTracerAvalonia.RayTracing;
public class Renderer(int CanvasWidth, int CanvasHeight)
{
    public void Render(ref byte[] canvas, Scene scene)
    {
        var tempCanvas = canvas;
        var tempScene = scene;


        Parallel.For(0, CanvasHeight, h =>
        {
            Parallel.For(0, CanvasWidth, w =>
            {
                var x = ((float)w / CanvasWidth) - .5f;
                var y = ((float)h / CanvasHeight) - .5f;
                var color = tempScene.Trace(x, y);


                var index = (h * CanvasWidth + w) * 4;
                tempCanvas[index] = color.B;
                tempCanvas[index + 2] = color.R;
                tempCanvas[index + 1] = color.G;
                tempCanvas[index + 3] = 255;
            });

        });
    }
}
