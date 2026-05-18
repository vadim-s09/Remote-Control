using System.Diagnostics;

namespace control
{
    public class Screenshot
    {
        public static void TakeScreenshot(string filePath)
        {
            try
            {
                
                if (Directory.Exists(filePath))
                {
                    string fileName = $"screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
                    filePath = Path.Combine(filePath, fileName);
                }
                else if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                }

                Screen focusedScreen = Screen.FromPoint(Cursor.Position);
                
                if (focusedScreen == null)
                    throw new InvalidOperationException("No primary screen found.");
                
                using Bitmap bitmap = new Bitmap(focusedScreen.Bounds.Width, focusedScreen.Bounds.Height);
                
                using Graphics graphics = Graphics.FromImage(bitmap);
                
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                
                bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aufnehmen des Screenshots: {ex.Message}");
            }
        }
        
    }
}
