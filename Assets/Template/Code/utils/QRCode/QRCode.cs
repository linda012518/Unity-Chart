using System;
using UnityEngine;

namespace QRCoder
{
    public class QRCode : AbstractQRCode, IDisposable
    {
        Texture2D bmp;
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public QRCode() { }

        public QRCode(QRCodeData data) : base(data) { }

        public Texture2D GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            var size = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
            var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

            bmp = new Texture2D(size, size);

            SetPixel(0, 0, bmp.width, bmp.height, Color.white);

            Color lightBrush = Color.white;
            Color darkBrush = Color.black;

            for (int x = 0; x < size + offset; x = x + pixelsPerModule)
            {
                for (int y = 0; y < size + offset; y = y + pixelsPerModule)
                {

                    var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
                    Rect rect = new Rect(x - offset, y - offset, pixelsPerModule, pixelsPerModule);
                    if (module)
                        SetPixel((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, darkBrush);
                    else
                        SetPixel((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, lightBrush);
                }
            }

            return bmp;
        }

        void SetPixel(int x, int y, int width, int height, Color color)
        {
            //Debug.Log("x=" + x + "  y=" + y + "  width=" + width + "    height" + height + "    color" + color.r);
            for (int i = x; i < width; i++)
            {
                for (int j = y; j < height; j++)
                {
                    bmp.SetPixel(i, j, color);
                }
            }
            //bmp.Apply();
        }

    }
}
