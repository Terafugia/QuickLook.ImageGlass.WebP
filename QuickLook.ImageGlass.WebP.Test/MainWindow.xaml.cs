using ImageGlass.Base.Photoing.Codecs;
using ImageGlass.WebP;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;

namespace QuickLook.ImageGlass.WebP.Test;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        const string path = "aa9c1debc03862d07e787f4dcb54ae71.webp";

        // Test for Animated WebP
        try
        {
            using var webp = new WebPWrapper();

            var aniWebP = webp.AnimLoad(path);
            var frames = aniWebP.Select(frame =>
            {
                var duration = frame.Duration > 0 ? frame.Duration : 100;
                var bitmap = frame.Bitmap;

                return new AnimatedImgFrame(frame.Bitmap, (uint)duration);
            });

            var animatedImg = new AnimatedImg(frames, frames.Count());

            for (var i = 0; i < animatedImg.Frames.Count(); i++)
            {
                var frame = animatedImg.Frames.ElementAt(i);
                ((Bitmap?)frame.Bitmap)?.Save($"{path}_{i}.png", ImageFormat.Png);
            }
        }
        catch
        {
            // Fallback to read the first frame
        }
    }
}
