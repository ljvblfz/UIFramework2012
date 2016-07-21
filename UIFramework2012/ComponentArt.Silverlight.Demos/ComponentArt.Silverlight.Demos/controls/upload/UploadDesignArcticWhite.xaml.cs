using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ComponentArt.Silverlight.Demos {
	public partial class UploadDesignArcticWhite : UserControl {
		public UploadDesignArcticWhite() {
			InitializeComponent();
			//ImageFilterButton.IsChecked = true;
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e) {
			RadioButton oButton = (RadioButton) sender;

			UploadControl.AllowedFileExtensions = oButton.Tag.ToString();
		}

		private void Upload_UploadGroupProgressChanged(object sender, ComponentArt.Silverlight.UI.Data.UploadEventArgs uce) {
			Debug.WriteLine("Upload_UploadGroupProgressChanged." + uce.uploadProgress);
		}

		private void Upload_UploadGroupSuccess(object sender, ComponentArt.Silverlight.UI.Data.UploadEventArgs uce) {
			Debug.WriteLine("Upload_UploadGroupSuccess.");
		}

		private void Upload_UploadGroupFailed(object sender, ComponentArt.Silverlight.UI.Data.UploadEventArgs uce) {
			Debug.WriteLine("Upload_UploadGroupFailed.");
		}

		private void Upload_UploadItemProgressChanged(object sender, ComponentArt.Silverlight.UI.Data.UploadEventArgs uce) {
			Debug.WriteLine("Upload_UploadItemProgressChanged." + uce.uploadProgress);
		}

		private void Upload_UploadItemSuccess(object sender, ComponentArt.Silverlight.UI.Data.UploadEventArgs uce) {
			Debug.WriteLine("Upload_UploadItemSuccess. " + uce.uploadSource.FileName);
		}

		private void Upload_UploadItemFailed(object sender, ComponentArt.Silverlight.UI.Data.UploadEventArgs uce) {
			Debug.WriteLine("Upload_UploadItemFailed. " + uce.uploadSource.FileName);
		}

		private void Upload_UploadItemRemoved(object sender, ComponentArt.Silverlight.UI.Data.UploadEventArgs uce) {
			Debug.WriteLine("Upload_UploadItemRemoved." + uce.uploadSource.FileName);
		}
	}
}
