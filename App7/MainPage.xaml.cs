using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App7
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap _writeableBitmap;
        private WriteableBitmap _thumbnailImageBitmap=new WriteableBitmap(100,100);
        public MainPage()
        {
            this.InitializeComponent();
            LoadDefaultImageAsync();
        }

        private async void LoadDefaultImageAsync()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new System.Uri("ms-appx:///Assets/Wide310x150Logo.scale-200.png"));
            await ApplyEffectAsync(file);
        }
        private void ShowEffectOnListView()
        {
            ObservableCollection<EffectPreview> itemSource = new ObservableCollection<EffectPreview>();

            lvEffect.ItemsSource = itemSource;
        }
        private async void PickImage(FileOpenPicker openPicker)
        {
            // Open the file picker.
            StorageFile file = await openPicker.PickSingleFileAsync();

            // File is null if the user cancels the file picker.
            if (file != null)
            {
                if (!(await ApplyEffectAsync(file)))
                    return;

                SaveButton.IsEnabled = true;
            }
        }
        private async Task<bool> ApplyEffectAsync(StorageFile file)
        {
            // Open a stream for the selected file.
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);

            string errorMessage = null;

            try
            {
                // Show a thumbnail of the original image.
                _thumbnailImageBitmap.SetSource(fileStream);
                OriginalImage.Source = _thumbnailImageBitmap;

                // Rewind the stream to start.
                fileStream.Seek(0);

                //// Set the imageSource on the effect and render.
                //((IImageConsumer)_grayscaleEffect).Source = new Lumia.Imaging.RandomAccessStreamImageSource(fileStream);
                //await m_renderer.RenderAsync();

            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                var dialog = new MessageDialog(errorMessage);
                await dialog.ShowAsync();
                return false;
            }

            return true;
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Openutton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };

            // Filter to include a sample subset of file types.
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");
            PickImage(openPicker);
        }
    }
    class EffectPreview { }


}
