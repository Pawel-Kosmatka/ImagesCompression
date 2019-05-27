using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagesCompression.Models;
using System.Runtime.InteropServices;
using System.Windows.Input;
using ImagesCompression.Core;
using System.Windows;
using ImagesCompression.Services;
using System.Drawing;

namespace ImagesCompression.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private ICompression _compressionService;
        private string _sourceFilePath;
        private string _compressionMethod;
        private byte[] _sourceFileBitMap;
        private byte[] _decodedFileBitMap;
        private int _sourceFileSize;
        private int _decodedFileSize;
        private byte[] _compressionResult;
        private double _compressionRatio;
        private int _compressedFileSize;

        public int CompressedFileSize
        {
            get => _compressedFileSize; 
            set
            {
                _compressedFileSize = value;
                OnPropertyChanged(nameof(CompressedFileSize));
            }
        }

        public byte[] DecodedFileBitMap
        {
            get => _decodedFileBitMap;
            set
            {
                _decodedFileBitMap = value;
                OnPropertyChanged(nameof(DecodedFileBitMap));
            }
        }
        public byte[] SourceFileBitMap
        {
            get => _sourceFileBitMap;
            set
            {
                _sourceFileBitMap = value;
                OnPropertyChanged(nameof(SourceFileBitMap));
            }
        }
        public double CompressionRatio
        {
            get => _compressionRatio;
            set
            {
                _compressionRatio = value;
                OnPropertyChanged(nameof(CompressionRatio));
            }
        }
        public string SourceFilePath
        {
            get => _sourceFilePath;
            set
            {
                ResetPropertyValues();
                _sourceFilePath = value;
                SourceFileBitMap = File.ReadAllBytes(_sourceFilePath);
                SourceFileSize = BmpHeader.GetFileSizeFromHeader(_sourceFileBitMap);
                OnPropertyChanged(nameof(SourceFilePath));
            }
        }
        public string CompressionMethod
        {
            get => _compressionMethod;
            set
            {
                _compressionMethod = value;
                OnPropertyChanged(nameof(CompressionMethod));
            }
        }
        public int SourceFileSize
        {
            get => _sourceFileSize;
            set
            {
                _sourceFileSize = value;
                OnPropertyChanged(nameof(SourceFileSize));
            }
        }
        public int DecodedFileSize
        {
            get => _decodedFileSize;
            set
            {
                _decodedFileSize = value;
                OnPropertyChanged(nameof(DecodedFileSize));
            }
        }
        public byte[] CompressionResult
        {
            get => _compressionResult;
            set
            {
                _compressionResult = value;
                CompressedFileSize = CompressionResult.Count();
                CompressionRatio = (double)_sourceFileSize / _compressionResult.Count();
                OnPropertyChanged(nameof(CompressionResult));
            }
        }

        public ICommand DecodeFile { get; }
        public ICommand StartCompression { get; }
        public List<string> CompressionMethods { get; set; } = Models.CompressionMethod.CompressionMethodsList;

        public MainWindowViewModel()
        {
            StartCompression = new CommandHandler(ExecuteCompression, CanExecuteCompression);
            DecodeFile = new CommandHandler(ExecuteDecoding, CanExecuteDecoding);
        }
        private void ResetPropertyValues()
        {
            if (_sourceFileBitMap != null)
            {
                CompressionMethod = default;
                SourceFileSize = default;
                SourceFileBitMap = default;
                DecodedFileSize = default;
                DecodedFileBitMap = default;
                CompressionResult = default;
                CompressionRatio = default;
            }
        }

        private bool CanExecuteDecoding()
        {
            return _compressionResult != null && _compressionMethod != null;
        }

        private void ExecuteDecoding()
        {
            DecodedFileBitMap = _compressionService.DecompressImage(_compressionResult);
            DecodedFileSize = BmpHeader.GetFileSizeFromHeader(_decodedFileBitMap);
        }

        private bool CanExecuteCompression()
        {
            return _sourceFileBitMap != null && _compressionMethod != null;
        }

        private void ExecuteCompression()
        {
            if (_compressionMethod == Models.CompressionMethod.RLE)
            {
                _compressionService = new RleCompression();

                CompressionResult = _compressionService.CompressImage(SourceFileBitMap, SourceFilePath);
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
