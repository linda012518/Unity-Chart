using QRCoder;
using UnityEngine;
using System;
namespace Utils {
    public class UnityQRCode : AbstractQRCode, IDisposable
    {
        public UnityQRCode() { }
        public UnityQRCode(QRCodeData data) : base(data) { }
    }
    public class QRCodeInterface
    {
        static Color[] _whiteArr;
        static Color[] _blackArr;

        public static byte _pixelsPerModule = 20;
        static Texture2D _bmp=new Texture2D (10,10);
        public static Texture2D GenerateQrCode( string _qrCodeKey, QRCodeGenerator.ECCLevel _eccLevel)
        {

            InitConst();
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(_qrCodeKey, _eccLevel))
                {
                    using (UnityQRCode qrCode = new UnityQRCode(qrCodeData))
                    {
                        int size = qrCode.QrCodeData.ModuleMatrix.Count * _pixelsPerModule;
                        _bmp.Resize(size, size);
                        for (int x = 0; x < size; x = x + _pixelsPerModule)
                        {
                            for (int y = 0; y < size; y = y + _pixelsPerModule)
                            {
                                var module = qrCode.QrCodeData.ModuleMatrix[(y + _pixelsPerModule) / _pixelsPerModule - 1][(x + _pixelsPerModule) / _pixelsPerModule - 1];
                                Rect rect = new Rect(x, y, _pixelsPerModule, _pixelsPerModule);
                                if (module)
                                    _bmp.SetPixels(x, size - y - _pixelsPerModule, (int)rect.width, (int)rect.height, _blackArr);
                                else
                                    _bmp.SetPixels(x, size - y - _pixelsPerModule, (int)rect.width, (int)rect.height, _whiteArr);
                            }
                        }
                    }
                }
            }
            _bmp.Apply();
            return _bmp;
        }
        static void InitConst()
        {
            if (_whiteArr != null && _blackArr != null) return;
            _whiteArr = new Color[_pixelsPerModule * _pixelsPerModule];
            _blackArr = new Color[_pixelsPerModule * _pixelsPerModule];

            for (int i = 0; i < _whiteArr.Length; i++)
            {
                _whiteArr[i] = Color.white;
                _blackArr[i] = Color.black;
            }
        }
    }
}

