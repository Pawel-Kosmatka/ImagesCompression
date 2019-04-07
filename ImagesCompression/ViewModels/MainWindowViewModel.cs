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

namespace ImagesCompression.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private ICompressionService _compressionService;
        private string _sourceFilePath;
        private string _compressionMethod;
        private byte[] _sourceFileBitMap = null;
        private byte[] _decodedFileBitMap = null;
        private int _sourceFileSize;
        private int _decodedFileSize;
        private CompressedFile _compressionResult;

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
            get => _sourceFileBitMap; set
            {
                _sourceFileBitMap = value;
                OnPropertyChanged(nameof(SourceFileBitMap));
            }
        }
        public string SourceFilePath
        {
            get => _sourceFilePath;
            set
            {
                _sourceFilePath = value;
                SourceFileBitMap = File.ReadAllBytes(_sourceFilePath);
                SourceFileSize = BmpHeaderService.GetFileSizeFromHeader(_sourceFileBitMap);
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
        public CompressedFile CompressionResult
        {
            get => _compressionResult;
            set
            {
                _compressionResult = value;
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

        private bool CanExecuteDecoding()
        {
            return _compressionResult != null && _compressionMethod != null;
        }

        private void ExecuteDecoding()
        {
            DecodedFileBitMap = _compressionService.DecompressImage(_compressionResult.FileBitMap);
            DecodedFileSize = BmpHeaderService.GetFileSizeFromHeader(_decodedFileBitMap);
        }

        private bool CanExecuteCompression()
        {
            return _sourceFileBitMap != null && _compressionMethod != null;
        }

        private void ExecuteCompression()
        {
            if (_compressionMethod == Models.CompressionMethod.RLE)
            {
                _compressionService = new RleCompressionService();

                CompressionResult = _compressionService.CompressImage(SourceFileBitMap);
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
