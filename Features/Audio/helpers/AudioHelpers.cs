using System.Speech.Synthesis;

namespace WriteTogether.Features.Audio
{
    public class AudioHelpers
    {
        public MemoryStream GenerateAudio(string text)
        {
            var synth = new SpeechSynthesizer();

            synth.Rate = 0;

            var stream = new MemoryStream();

            synth.SetOutputToWaveStream(stream);

            synth.Speak(text);

            stream.Position = 0;

            return stream;
        }
    }
}