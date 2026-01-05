#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.IO;
using System.Threading.Tasks;
using Com.Teamdev.Dotnetbrowser.Prefs;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace ReactExample.Desktop.Rpc;

public class PrefsService : Com.Teamdev.Dotnetbrowser.Prefs.PrefsService.PrefsServiceBase
{
    private readonly ILogger<PrefsService> logger;
    private readonly Prefs appPrefs;

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
        => Task.FromResult(appPrefs.Account);

    public override Task<Appearance> GetAppearance(Empty request, ServerCallContext context)
        => Task.FromResult(appPrefs.Appearance);

    public override Task<General> GetGeneral(Empty request, ServerCallContext context)
        => Task.FromResult(appPrefs.General);

    public override Task<Notifications> GetNotifications(
        Empty request, ServerCallContext context) => Task.FromResult(appPrefs.Notifications);

    public override Task<ProfilePicture>
        GetProfilePicture(Empty request, ServerCallContext context)
        => Task.FromResult(appPrefs.ProfilePicture ?? new ProfilePicture());

    public override Task<Empty> SetAccount(Account request, ServerCallContext context)
    {
        appPrefs.Account = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> SetAppearance(Appearance request, ServerCallContext context)
    {
        appPrefs.Appearance = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> SetGeneral(General request, ServerCallContext context)
    {
        appPrefs.General = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> SetNotifications(Notifications request,
                                                 ServerCallContext context)
    {
        appPrefs.Notifications = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> SetProfilePicture(ProfilePicture request,
                                                  ServerCallContext context)
    {
        appPrefs.ProfilePicture = request;
        PrefsFile.Write(appPrefs);
        return Task.FromResult(new Empty());
    }

    private Prefs InitPreferences()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(
                                                        Environment.GetFolderPath(Environment
                                                           .SpecialFolder
                                                           .ApplicationData)
                                                        + "\\ReactExample")!);

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
