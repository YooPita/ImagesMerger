using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class ImgMerger
{
    public static Bitmap MergeBitmaps(List<Bitmap> listOfTileBitmaps, int columnCount = 100, bool horizontalTiling = true)
    {
        int width = 1;
        int height = 1;
        foreach (Bitmap sourceTile in listOfTileBitmaps)
        {
            if (sourceTile.Width > width)
            {
                width = sourceTile.Width;
            }
            if (sourceTile.Height > height)
            {
                height = sourceTile.Height;
            }
        }
        if (columnCount > listOfTileBitmaps.Count)
        {
            columnCount = listOfTileBitmaps.Count;
        }
        int rowCount = (int)Math.Ceiling((double)(((double)listOfTileBitmaps.Count) / ((double)columnCount)));
        if (!horizontalTiling)
        {
            int temp = rowCount;
            rowCount = columnCount;
            columnCount = temp;
        }
        Bitmap image = new Bitmap(columnCount * width, rowCount * height, PixelFormat.Format32bppArgb);
        Graphics graphics = Graphics.FromImage(image);
        SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0xff, 240, 200));
        graphics.FillRectangle(brush, 0, 0, image.Width, image.Height);
        int n = 0;
        if (horizontalTiling)
        {
            foreach (Bitmap tileBitmap in listOfTileBitmaps)
            {
                int col = (int)Math.Floor((double)(((double)n) / ((double)columnCount)));
                int row = n % columnCount;
                Point location = new Point(((row * width) + (width / 2)) - (tileBitmap.Width / 2), ((col * height) + (height / 2)) - (tileBitmap.Height / 2));
                Size size = new Size(tileBitmap.Width, tileBitmap.Height);
                Graphics.FromImage(image).DrawImage(tileBitmap, new Rectangle(location, size));
                n++;
            }
        }
        else
        {
            foreach (Bitmap tileBitmap in listOfTileBitmaps)
            {
                int row = (int)Math.Floor((double)(((double)n) / ((double)rowCount)));
                int col = n % rowCount;
                Point location = new Point(((row * width) + (width / 2)) - (tileBitmap.Width / 2), ((col * height) + (height / 2)) - (tileBitmap.Height / 2));
                Size size = new Size(tileBitmap.Width, tileBitmap.Height);
                Graphics.FromImage(image).DrawImage(tileBitmap, new Rectangle(location, size));
                n++;
            }
        }
        return image;
    }

    public static void MergeFolder(bool del, bool hor, int col)
    {
        string rootFolder = Environment.CurrentDirectory;
        Bitmap bitmap;
        
        // ПЕРЕБОР ВСЕХ ПАПОК
        foreach (var folder in Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories))
        {
            var listImages = new List<Bitmap>();
            // ПЕРЕБОР ВСЕХ ФАЙЛОВ В ПАПКАХ
            foreach (var file in Directory.EnumerateFiles(folder, "*.png"))
            {
                listImages.Add((Bitmap)Image.FromFile(file));
            }
            if(listImages.Count > 0)
            {
                try
                {
                    bitmap = MergeBitmaps(listImages, col, hor);
                    try
                    {
                        var name = rootFolder + @"\" + GetName(folder, rootFolder) + ".png";
                        SaveImage(bitmap, name);
                        Console.WriteLine("File saved: " + name);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Save Image Exception: " + exception.Message);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Merge Bitmaps Exception: " + exception.Message);
                }
                if (del)
                {
                    foreach (var filepath in Directory.EnumerateFiles(folder, "*.png"))
                    {
                        if (File.Exists(filepath.))
                        {
                            try
                            {
                                File.Delete(filepath);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine("Delete Image Exceptionz: " + exception.Message);
                            }
                        }
                    }
                }
            }
        }
    }

    private static string GetName(string file, string rootFolder)
    {
        string s = file.Substring(rootFolder.Length + 1); // локальный путь
        string name = "";
        string[] words = s.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries); // разделяем строку по символу "\"
        for (int i = 0; i < words.Length; i++)
        {
            name += words[i];
        }

        return name;
    }

    public static void SaveImage(Bitmap image, string fileTarget)
    {
        image.Save(fileTarget, ImageFormat.Png);
    }
}