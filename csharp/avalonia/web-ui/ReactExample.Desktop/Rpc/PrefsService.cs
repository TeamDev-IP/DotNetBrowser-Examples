#region Copyright
// Copyright (c) 2000-${CurrentDate.Year} TeamDev Ltd. All rights reserved.
// TeamDev PROPRIETARY and CONFIDENTIAL.
// Use is subject to license terms.
#endregion

using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using Com.Teamdev.Dotnetbrowser.Prefs;

namespace ReactExample.Desktop.Rpc;

public class PrefsService : Com.Teamdev.Dotnetbrowser.Prefs.PrefsService.PrefsServiceBase
{
    private readonly ILogger<PrefsService> logger;
    private Prefs appPrefs;

    public PrefsService(ILogger<PrefsService> logger)
    {
        this.logger = logger;
        if (PrefsFile.Exists())
        {
            appPrefs = PrefsFile.Read();
        }
        else
        {
            appPrefs = InitPreferences();
        }
    }

    public override Task<Account> GetAccount(Empty request, ServerCallContext context)
    {
        return Task.FromResult(appPrefs.Account);
    }

    public override Task<Empty> SetAccount(Account request, ServerCallContext context)
    {
        appPrefs.Account = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> SetProfilePicture(ProfilePicture request, ServerCallContext context)
    {
        appPrefs.ProfilePicture = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<ProfilePicture> GetProfilePicture(Empty request, ServerCallContext context)
    {
        return Task.FromResult(appPrefs.ProfilePicture ?? new ProfilePicture());
    }

    public override Task<Empty> SetGeneral(General request, ServerCallContext context)
    {
        appPrefs.General = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<General> GetGeneral(Empty request, ServerCallContext context)
    {
        return Task.FromResult(appPrefs.General);
    }

    public override Task<Empty> SetAppearance(Appearance request, ServerCallContext context)
    {
        appPrefs.Appearance = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<Appearance> GetAppearance(Empty request, ServerCallContext context)
    {
        return Task.FromResult(appPrefs.Appearance);
    }

    public override Task<Empty> SetNotifications(Notifications request, ServerCallContext context)
    {
        appPrefs.Notifications = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<Notifications> GetNotifications(Empty request, ServerCallContext context)
    {
        return Task.FromResult(appPrefs.Notifications);
    }

    private Prefs InitPreferences()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ReactExample")!);

        var account = new Account
        {
            FullName = "John Doe",
            Email = "john.doe@mail.com",
            BiometricAuthentication = false,
            TwoFactorAuthentication = TwoFactorAuthentication.Email
        };

        var appearance = new Appearance
        {
            FontSize = FontSize.Default,
            Theme = Theme.System
        };

        var prefs = new Prefs
        {
            Account = account,
            Appearance = appearance,
            ProfilePicture = new ProfilePicture(),
            General = new General(),
            Notifications = new Notifications()
        };

        PrefsFile.Write(prefs);
        return prefs;
    }
}
