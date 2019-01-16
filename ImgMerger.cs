using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

public static class ImgMerger
{
    public static Bitmap MergeBitmaps(List<Bitmap> listOfTileBitmaps, int columnCount, bool horizontalTiling)
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

    public static void MergeFolder()
    {
        string rootFolder = Environment.CurrentDirectory;
        foreach (var file in Directory.EnumerateFiles(rootFolder, "*.png", SearchOption.AllDirectories))
        {
            Console.WriteLine(file);
        }
    }
}