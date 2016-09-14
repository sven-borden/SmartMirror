using System;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.IO;

namespace SmartMirror.Class.ComputerVision
{
	class Emotional
	{
		public static async Task<Emotion[]> GetEmotionFromFile(Stream imageFileStream)
		{
			EmotionServiceClient emotionServiceClient = new EmotionServiceClient("b31e347bbb0241a3a1d20f87623d91ca");
			try
			{
				Emotion[] emotionResult;
				emotionResult = await emotionServiceClient.RecognizeAsync(imageFileStream);
				return emotionResult;
			}
			catch (Exception exception)
			{
				return null;
			}
		}
	}
}
