using System.IO;
using UnityEngine;

/// <summary>
/// AudioClip 을 간단한 16bit PCM WAV 파일로 저장하는 유틸리티
/// </summary>
public static class WavUtility
{
    public static void Save(string filePath, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("WavUtility.Save: clip is null");
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            int sampleCount = clip.samples * clip.channels;
            int frequency = clip.frequency;

            // 헤더는 나중에 한 번에 쓰기 위해 버퍼로 만든다
            // 이후에 데이터 길이를 채워 넣을 것
            byte[] header = new byte[44];
            fileStream.Write(header, 0, header.Length);

            // 오디오 데이터 작성
            float[] samples = new float[sampleCount];
            clip.GetData(samples, 0);

            short[] intData = new short[sampleCount];
            byte[] bytesData = new byte[sampleCount * 2];

            const float rescaleFactor = 32767f;

            for (int i = 0; i < sampleCount; i++)
            {
                intData[i] = (short)(samples[i] * rescaleFactor);
                byte[] byteArr = System.BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            fileStream.Write(bytesData, 0, bytesData.Length);

            // 파일 끝에서 길이 계산 후, 헤더 다시 작성
            int fileSize = (int)fileStream.Length;

            fileStream.Seek(0, SeekOrigin.Begin);
            WriteHeader(fileStream, clip, fileSize - 44);
        }

        Debug.Log("WAV saved: " + filePath);
    }

    private static void WriteHeader(Stream stream, AudioClip clip, int dataLength)
    {
        int sampleRate = clip.frequency;
        short channels = (short)clip.channels;
        short bitsPerSample = 16;

        using (var writer = new BinaryWriter(stream))
        {
            writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(dataLength + 36);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));
            writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
            writer.Write(16);
            writer.Write((short)1); // PCM
            writer.Write(channels);
            writer.Write(sampleRate);
            writer.Write(sampleRate * channels * bitsPerSample / 8);
            writer.Write((short)(channels * bitsPerSample / 8));
            writer.Write(bitsPerSample);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            writer.Write(dataLength);
        }
    }
}
