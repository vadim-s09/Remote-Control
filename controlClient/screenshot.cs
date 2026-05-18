using System.Diagnostics;

namespace control
{
    public class Screenshot
    {
        public static void TakeScreenshot(string filePath)
        {
            try
            {
                // Überprüfe, ob filePath ein Verzeichnis ist
                if (Directory.Exists(filePath))
                {
                    string fileName = $"screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
                    filePath = Path.Combine(filePath, fileName);
                }
                else if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    // Erstelle das Verzeichnis, falls es nicht existiert
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                }

                Screen focusedScreen = Screen.FromPoint(Cursor.Position);
                // Erstelle ein Bitmap-Objekt mit der Größe des Bildschirms
                if (focusedScreen == null)
                    throw new InvalidOperationException("No primary screen found.");
                
                using Bitmap bitmap = new Bitmap(focusedScreen.Bounds.Width, focusedScreen.Bounds.Height);
                // Erstelle ein Graphics-Objekt aus dem Bitmap
                using Graphics graphics = Graphics.FromImage(bitmap);
                // Kopiere den Bildschirm in das Bitmap
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                // Speichere das Bitmap als PNG-Datei
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