namespace StockSharp.Messages
{
	using System;

	using Ecng.Interop;
	using Ecng.Localization;

	/// <summary>
	/// ����������������.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class TargetPlatformAttribute : Attribute
	{
		/// <summary>
		/// ������� ���������.
		/// </summary>
		public Languages PreferLanguage { get; private set; }

		/// <summary>
		/// ���������.
		/// </summary>
		public Platforms Platform { get; private set; }

		/// <summary>
		/// ������� <see cref="TargetPlatformAttribute"/>.
		/// </summary>
		/// <param name="preferLanguage">������� ���������.</param>
		/// <param name="platform">���������.</param>
		public TargetPlatformAttribute(Languages preferLanguage = Languages.English, Platforms platform = Platforms.AnyCPU)
		{
			PreferLanguage = preferLanguage;
			Platform = platform;
		}
	}
}