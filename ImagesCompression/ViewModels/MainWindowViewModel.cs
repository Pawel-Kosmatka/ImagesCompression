using ImagesCompression.Core;
using ImagesCompression.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImagesCompression.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private ICompression _compressionService;
        private string _sourceFilePath;
        private string _compressionMethod;
        private byte[] _sourceFileBitMap;
        private byte[] _decodedFileBitMap;
        private double _sourceFileSize;
        private double _decodedFileSize;
        private byte[] _compressionResult;
        private double _compressionRatio;
        private double _compressedFileSize;
        private bool _isMenuEnabled = true;

        public bool IsMenuEnabled
        {
            get => _isMenuEnabled;
            set
            {
                _isMenuEnabled = value;
                OnPropertyChanged(nameof(IsMenuEnabled));
            }
        }
        public double CompressedFileSize
        {
            get => _compressedFileSize;
            set
            {
                _compressedFileSize = value / 1048576;
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
        public double SourceFileSize
        {
            get => _sourceFileSize / 1048576;
            set
            {
                _sourceFileSize = value;
                OnPropertyChanged(nameof(SourceFileSize));
            }
        }
        public double DecodedFileSize
        {
            get => _decodedFileSize / 1048576;
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
                if (_compressionResult != null)
                {
                    CompressedFileSize = CompressionResult.Count();
                    CompressionRatio = (double)_sourceFileSize / _compressionResult.Count();
                }
                OnPropertyChanged(nameof(CompressionResult));
            }
        }

        public ICommand DecodeFile { get; }
        public ICommand StartCompression { get; }
        public List<string> CompressionMethods { get; set; } = Models.CompressionMethod.CompressionMethodsList;

        public MainWindowViewModel()
        {
            StartCompression = new CommandHandler(ExecuteCompressionAsync, CanExecuteCompression);
            DecodeFile = new CommandHandler(ExecuteDecodingAsync, CanExecuteDecoding);
        }
        private void ResetPropertyValues()
        {
            CompressionMethod = default;
            SourceFileSize = default;
            SourceFileBitMap = default;
            DecodedFileSize = default;
            DecodedFileBitMap = default;
            CompressionResult = default;
            CompressionRatio = default;
            CompressedFileSize = default;
        }

        private bool CanExecuteDecoding()
        {
            return _compressionResult != null && _compressionMethod != null;
        }

        private async Task ExecuteDecodingAsync()
        {
            IsMenuEnabled = false;
            DecodedFileBitMap = await Task.Run(() => _compressionService.DecompressImage(_compressionResult));
            DecodedFileSize = BmpHeader.GetFileSizeFromHeader(_decodedFileBitMap);
            IsMenuEnabled = true;
        }

        private bool CanExecuteCompression()
        {
            return _sourceFileBitMap != null && _compressionMethod != null;
        }

        private async Task ExecuteCompressionAsync()
        {
            if (_compressionMethod == Models.CompressionMethod.RLE)
            {
                _compressionService = new RleCompression();
            };
            IsMenuEnabled = false;
            CompressionResult = await Task.Run(() => _compressionService.CompressImage(SourceFileBitMap, SourceFilePath));
            var decodeFile = DecodeFile as CommandHandler;
            decodeFile.RaiseCanExecuteChanged();
            IsMenuEnabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
