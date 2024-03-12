using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageSteganography
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the directory where the executing assembly is located
            string programDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Combine the program directory with the relative path to your image
            string originalImagePath = Path.Combine(programDirectory, @"..\..\..\..\..\..\..\Dev\ImageSteganography\ImageSteganography\original_image.png");

            // Adjust the directory accordingly based on your project structure
            string secretMessage = "This is a hidden top secret message. The quick brown fox jumped over the lazy dogs";
            string encryptedImagePath = Path.Combine(programDirectory, "encrypted_image.png");

            EncryptMessage(originalImagePath, encryptedImagePath, secretMessage);

            string decodedMessage = DecodeMessage(encryptedImagePath);
            Console.WriteLine("Decoded Message: " + decodedMessage);
        }

        static void EncryptMessage(string originalImagePath, string encryptedImagePath, string message)
        {
            using (var originalImage = Image.Load<Rgba32>(originalImagePath))
            {
                using (var encryptedImage = originalImage.CloneAs<Rgba32>())
                {
                    int charIndex = 0;
                    for (int y = 0; y < encryptedImage.Height; y++)
                    {
                        for (int x = 0; x < encryptedImage.Width; x++)
                        {
                            if (charIndex < message.Length)
                            {
                                char charToEncrypt = message[charIndex++];
                                int charValue = Convert.ToInt32(charToEncrypt);
                                var pixel = encryptedImage[x, y];
                                pixel.B = (byte)charValue;
                                encryptedImage[x, y] = pixel;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    encryptedImage.Save(encryptedImagePath);
                    Console.WriteLine("Message encrypted successfully.");
                }
            }
        }

        static string DecodeMessage(string encryptedImagePath)
        {
            using (var encryptedImage = Image.Load<Rgba32>(encryptedImagePath))
            {
                string decodedMessage = "";
                for (int y = 0; y < encryptedImage.Height; y++)
                {
                    for (int x = 0; x < encryptedImage.Width; x++)
                    {
                        var pixel = encryptedImage[x, y];
                        int charValue = pixel.B; // Extract the least significant bit from the blue channel
                        decodedMessage += (char)charValue;
                    }
                }

                return decodedMessage;
            }
        }
    }
}
