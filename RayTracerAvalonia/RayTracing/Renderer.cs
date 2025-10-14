using System;
using System.Threading.Tasks;

namespace RayTracerAvalonia.RayTracing;
public class Renderer(int CanvasWidth, int CanvasHeight)
{
    public unsafe void Render(Memory<byte> canvas, Scene scene)
    {
        var tempCanvas = canvas;
        var tempScene = scene;
        var handle = (byte*)canvas.Pin().Pointer;
        

        Parallel.For(0, CanvasHeight, (int h) =>
        {
            var y = ((float)h / CanvasHeight) - .5f;
            Parallel.For(0, CanvasWidth, (int w) =>
            {
                var x = ((float)w / CanvasWidth) - .5f;
                var color = tempScene.Trace(x, y);

                var index = (h * CanvasWidth + w) * 4;
                
                
                *(handle + index * sizeof(byte)) = color.B;
                *(handle + (index+1) * sizeof(byte))  = color.G;
                *(handle + (index+2) * sizeof(byte))  = color.R;
                *(handle + (index+3) * sizeof(byte))  = 255;
            });
        });
    }
}
