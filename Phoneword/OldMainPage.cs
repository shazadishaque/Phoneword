using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Phoneword
{
    public class OldMainPage : ContentPage
    {
        private Entry phoneNumberText;
        private Button translateButton;
        private Button callButton;
        private string translatedNumber;

        public OldMainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Text = "Enter a Phoneword"
            }); ;

            panel.Children.Add(phoneNumberText = new Entry
            {
                Text = "1-855-XAMARIN"
            });

            panel.Children.Add(translateButton = new Button
            {
                Text = "Translate"
            });

            panel.Children.Add(callButton = new Button
            {
                IsEnabled = false,
                Text = "Call"
            });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Core.PhonewordTranslator.ToNumber(enteredNumber);

            if(!string.IsNullOrEmpty(translatedNumber))
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
            if(await this.DisplayAlert(
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
                catch(ArgumentNullException)
                {
                    await (DisplayAlert("Unable to dial", "Phone number was not valid.", "OK"));
                }
                catch(FeatureNotSupportedException)
                {
                    await (DisplayAlert("Unable  to dial", "Phone dialing not supported.", "OK"));
                }
                catch(Exception)
                {
                    await (DisplayAlert("Unable to call", "Phone dialing failed.", "OK"));
                }
            }
        }
    }
}
