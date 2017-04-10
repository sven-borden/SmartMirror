using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SmartMirror.Hue
{
	[DataContract]
	public class Light
	{
		/// <summary>
		/// And internal backlink to the light's bridge
		/// </summary>
		internal Bridge _bridge;

		/// <summary>
		/// Gets the light's id
		/// </summary>
		[DataMember(Name = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Get the light's state
		/// </summary>
		[DataMember(Name = "state")]
		public LightState State { get; set; }

		/// <summary>
		/// Gets the light type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; private set; }

		/// <summary>
		/// Gets the light Name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; private set; }

		/// <summary>
		/// Gets the light's model id
		/// </summary>
		[DataMember(Name = "modelid")]
		public string ModelId { get; private set; }

		/// <summary>
		/// Gets the software version
		/// </summary>
		[DataMember(Name = "swversion")]
		public string SoftwareVersion { get; private set; }

		/// <summary>
		/// Attempts to refresh this object to match the current state of its physical light
		/// </summary>
		/// <returns></returns>
		public async Task SyncAsync()
		{
			Light copy = await _bridge.GetLightAsync(Id);
			if (copy != null)
				State = copy.State;
		}

		/// <summary>
		/// Attempts to rename the light
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task RenameAsync(string name) =>
			await _bridge.HttpPutAsync($"lights/{Id}", $"{{\"name\":\"{name}\"}}");

		/// <summary>
		/// Attempts to change the state of the physical light to match the state of this object
		/// </summary>
		/// <returns></returns>
		public async Task ChangeStateAsync() =>
			await _bridge.HttpPutAsync($"lights/{Id}/state", JsonConvert.SerializeObject(this));

		/// <summary>
		/// Attemps to change the state of the physical light to match the state of the selected property
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="selector"></param>
		/// <returns></returns>
		public async Task ChangeStateAsync<T>(Expression<Func<LightState, T>> selector)
		{
			var expression = selector.Body as MemberExpression;
			if (null == expression)
			{
				throw new ArgumentException(
					"This method can only modify LightState properties (like bri or hue)");
			}
			string attribute = expression.Member.CustomAttributes.First()
				.NamedArguments.First().TypedValue.Value.ToString();
			object property = selector.Compile().Invoke(State);
			string val = property.GetType().IsArray ?
				$"[{String.Join(", ", ((IEnumerable)property).Cast<double>().Select(x => x.ToString()))}]" :
				property.ToString().ToLower();
			string json = $"{{\"{attribute}\": {val}}}";
			await _bridge.HttpPutAsync($"lights/{Id}/state", json);
		}
	}

	[DataContract]
	public class LightState
	{
		/// <summary>
		/// An internal backlink to this state's light
		/// </summary>
		internal Light _light;

		[DataMember(Name = "on")]
		private bool on;

		[LightProperty(Name = "on")]
		public bool On
		{
			get { return on; }
			set
			{
				on = value;
				Task task = _light.ChangeStateAsync(x => x.On);
			}
		}

			[DataMember(Name = "bri")]
			private byte bri;

		[LightProperty(Name = "bri")]
		public byte Brightness
		{
			get { return bri; }
			set
			{
				bri = value;
				Task task = _light.ChangeStateAsync(x => x.Brightness);
			}
		}

		[DataMember(Name = "hue")]
		private ushort hue;
		/// <summary>
		/// Gets or sets the lights hue.
		/// </summary>
		[LightProperty(Name = "hue")]
		public ushort Hue
		{
			get { return hue; }
			set
			{
				hue = value;
				Task task = _light.ChangeStateAsync(x => x.Hue);
			}
		}

		[DataMember(Name = "sat")]
		private byte sat;
		/// <summary>
		/// Gets or sets the light's saturation.
		/// </summary>
		[LightProperty(Name = "sat")]
		public byte Saturation
		{
			get { return sat; }
			set
			{
				sat = value;
				Task task = _light.ChangeStateAsync(x => x.Saturation);
			}
		}

		[DataMember(Name = "xy")]
		private double[] xy;
		/// <summary>
		/// Gets or sets the light's xy color coordinates.
		/// </summary>
		[LightProperty(Name = "xy")]
		public double[] ColorCoordinates
		{
			get { return xy; }
			set
			{
				xy = value;
				Task task = _light.ChangeStateAsync(x => x.ColorCoordinates);
			}
		
		}

		[DataMember(Name = "alert")]
		private string alert;
		/// <summary>
		/// Gets or sets the light's brightness.
		/// </summary>
		[LightProperty(Name = "alert")]
		public string Alert
		{
			get { return alert; }
			set
			{
				alert = value;
				Task task = _light.ChangeStateAsync(x => x.Alert);
			}
		}

		[DataMember(Name = "effect")]
		private string _effect;
		/// <summary>
		/// Gets or sets the light's effect.
		/// </summary>
		[LightProperty(Name = "effect")]
		public string Effect
		{
			get { return _effect; }
			set
			{
				_effect = value;
				Task task = _light.ChangeStateAsync(x => x.Effect);
			}
		}

		[DataMember(Name = "colormode")]
		private string colormode;
		/// <summary>
		/// Gets or sets the light's color mode.
		/// </summary>
		[LightProperty(Name = "colormode")]
		public string ColorMode
		{
			get { return colormode; }
			set
			{
				colormode = value;
				Task task = _light.ChangeStateAsync(x => x.ColorMode);
			}
		}

		/// <summary>
		/// Gets if the light is reachable.
		/// </summary>
		[DataMember(Name = "reachable")]
		public bool Reachable { get; private set; }
	}

	/// <summary>
	/// Maps the properties of LightState to their representation in json.
	/// </summary>
	public class LightProperty : Attribute
	{
		/// <summary>
		/// Gets the name of the property in the Hue REST API.
		/// </summary>
		public string Name { get; set; }
	}
}