using MagickConverter.Events;

namespace MagickConverter
{
    public interface IMagickService
    {
        void Convert(TaskSetup setup);
        void ConvertFinished(object sender, ConvertFinishedEventArgs args);
        void ConvertProgress(object sender, ConvertProgressEventArgs args);
    }
}