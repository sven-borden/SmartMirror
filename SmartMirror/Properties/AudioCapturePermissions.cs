using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace SmartMirror.Voice
{
	internal class AudioCapturePermissions
	{
		internal async static Task<bool> RequestMicrophonePermission()
		{
			try
			{
				// Request access to the microphone only, to limit the number of capabilities we need
				// to request in the package manifest.
				MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
				settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
				settings.MediaCategory = MediaCategory.Speech;
				MediaCapture capture = new MediaCapture();
				await capture.InitializeAsync(settings);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				return false;
			}
		}
	}
}