using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Phoneword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private string translatedNumber;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Core.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = $"Call {translatedNumber}";
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }

        private async void OnCall(object sender, EventArgs e)
        {
            if (await this.DisplayAlert(
                "Dial a number",
                $"Would you like to call {translatedNumber}",
                "Yes",
                "No"
            ))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await (DisplayAlert("Unable to dial", "Phone number was not valid.", "OK"));
                }
                catch (FeatureNotSupportedException)
                {
                    await (DisplayAlert("Unable  to dial", "Phone dialing not supported.", "OK"));
                }
                catch (Exception)
                {
                    await (DisplayAlert("Unable to call", "Phone dialing failed.", "OK"));
                }
            }
        }
    }
}