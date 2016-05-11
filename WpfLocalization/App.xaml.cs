using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace WpfLocalization {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public const string NeutralCulture = "en-US";

        public App() {
            SupportedCultures.Clear();
            SupportedCultures.Add(new CultureInfo(NeutralCulture)); // neutral
            SupportedCultures.Add(new CultureInfo("ru-RU"));
        }

        public static List<CultureInfo> SupportedCultures { get; } = new List<CultureInfo>();

        /// <summary>
        /// Application property that allows to view the current culture and change culture on-the-fly.
        /// </summary>
        public static CultureInfo Culture {
            get {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                // if the new culture is same as current, do nothing
                if (
                    value.Equals(
                        System.Threading.Thread.CurrentThread.CurrentUICulture))
                    return;

                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                var newDict = new ResourceDictionary {
                    Source = value.Name.Equals(NeutralCulture)
                        ? new Uri("Resources/Strings.xaml", UriKind.Relative)
                        : new Uri($"Resources/Strings.{value.Name}.xaml",
                            UriKind.Relative)
                };

                // replace the old ResourceDictionary with the new one
                var oldDict =
                    Current.Resources.MergedDictionaries.FirstOrDefault(
                        x =>
                            x.Source != null &&
                            x.Source.OriginalString.StartsWith("Resources/lang."));
                if (oldDict != null) {
                    var dictIndex = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(dictIndex, newDict);
                }
                else {
                    Current.Resources.MergedDictionaries.Add(newDict);
                }

                // notify UI that culture has changed
                CultureChanged?.Invoke(Current, new EventArgs());
            }
        }

        /// <summary>
        /// Workaround for switching culture in an app that supports only two.
        /// </summary>
        public static void SwitchCulture() {
            Culture =
                new CultureInfo(Culture.Name.Equals(NeutralCulture)
                    ? "ru-Ru"
                    : NeutralCulture);
        }

        /// <summary>
        /// Event for notifying UI that culture has changed
        /// </summary>
        public static event EventHandler CultureChanged;
    }
}