﻿using CommunityToolkit.Maui.Alerts;
using System.Diagnostics.Contracts;

namespace Dzone.Mobile.Views.AuthViews;

[QueryProperty(nameof(username), "username")]
[QueryProperty(nameof(otpType), "otpType")]
[QueryProperty(nameof(title), "title")]
[QueryProperty(nameof(emailAddress), "email")]
public partial class ConfirmOtpView : ContentPage
{
    public IAuthService authService;
    public string username { get; set; } = "";
    public string otpType { get; set; } = "";
    public string title { get; set; } = "";
    public string emailAddress { get; set; } = "";

    public ConfirmOtpView(IAuthService authService)
    {
        InitializeComponent();
        this.authService = authService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        try
        {
            base.OnNavigatedTo(args);

            if (!string.IsNullOrEmpty(username) &&
                !string.IsNullOrEmpty(otpType) &&
                !string.IsNullOrEmpty(title) &&
                !string.IsNullOrEmpty(emailAddress))
            {
                LblTitle.Text = title;
                LblUserName.Text = "مرحبأ بكـ " + username;
            }

            if (otpType == "RestPassword")
                TxtNewPassword.IsVisible = true;
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async void BtnConfirmOtp_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(TxtOneTimePassword.Text))
            {
                await Toast.Make("الرجاء كتابة رمز التحقق والمحاولة مرة اخرئ").Show();
                return;
            }

            if (otpType == "ConfirmEmail")
            {
                var Contract = new ConfirmEmailContract
                {
                    code = Convert.ToInt32(TxtOneTimePassword.Text),
                    email = emailAddress
                };

                var IsEmailConfirmedResult = await authService.ConfirmEmail(Contract);

                if (IsEmailConfirmedResult.IsError)
                {
                    await Toast.Make(IsEmailConfirmedResult.FirstError.Description).Show();
                    return;
                }

                await Shell.Current.GoToAsync($"../{nameof(LoginView)}");
            }
            else if (otpType == "RestPassword")
            {
                var Contract = new RestPasswordRequest
                {
                    otpCode = Convert.ToInt32(TxtOneTimePassword.Text),
                    email = emailAddress,
                    newPassword = TxtNewPassword.Text
                };

                var IsEmailConfirmedResult = await authService.RestPassword(Contract);

                if (IsEmailConfirmedResult.IsError)
                {
                    await Toast.Make(IsEmailConfirmedResult.FirstError.Description).Show();
                    return;
                }

                await Toast.Make("تم تغير كلمة المرور بنجاح,").Show();

                await Shell.Current.GoToAsync($"../{nameof(LoginView)}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}